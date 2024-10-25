using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.TouchPhase;

public class MapCamera : MonoBehaviour
{
    public static Camera mapCamera;
    public MapSystem map;

    public Transform target;

    public float zoomSpeed = 0.1f;
    public float rotationSpeed = 0.2f;

    public float followDistance = 10f;
    public float followHeight = 5f;
    public float followSmoothness = 0.1f;

    private Vector2 prevTouchPos1;
    private Vector2 prevTouchPos2;

    private float currentYaw = 0f;
    private float currentPitch = 20f;

    public float pitchMin = 10f;
    public float pitchMax = 80f;
    private Dictionary<int, bool> canTouch = new Dictionary<int, bool>();
    private PointerEventData pointerEventData;
    public GraphicRaycaster raycaster;
    public GameObject targetUIElement;
    private void Awake()
    {
        mapCamera = GetComponent<Camera>();
        if (Touchscreen.current != null)
        {
            EnhancedTouchSupport.Enable();
        }

        pointerEventData = new PointerEventData(EventSystem.current);
    }

    void Update()
    {
        if (!gameObject.activeInHierarchy)
        {
            return;
        }

        HandleCameraMove();
    }
    bool IsTouchOverSpecificUIElement(Vector2 touchPosition)
    {
        pointerEventData.position = touchPosition;

        List<RaycastResult> results = new List<RaycastResult>();

        raycaster.Raycast(pointerEventData, results);

        foreach (var result in results)
        {
            if (result.gameObject == targetUIElement)
            {
                return true;
            }
        }
        return false;
    }

    void HandleTouchMove()
    {

        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                var touch = Input.GetTouch(i);

                if (touch.phase == TouchPhase.Began)
                {
                    if (!IsTouchOverSpecificUIElement(touch.position))
                    {
                        canTouch[i] = true;
                    }
                    else
                    {
                        canTouch[i] = false;
                    }

                    Debug.Log($"Touch Count: {Input.touchCount}");
                    Debug.Log($"Touch Phase: {touch.phase}");
                    Debug.Log($"FingerId: {touch.fingerId}, Can Touch: {canTouch[i]}");

                }

                if (touch.phase == TouchPhase.Moved)
                {
                    if (canTouch[i])
                    {
                        RotateCamera(touch.deltaPosition);
                    }

                    Debug.Log($"Touch Count: {Input.touchCount}");
                    Debug.Log($"Touch Phase: {touch.phase}");
                    Debug.Log($"FingerId: {touch.fingerId}, Can Touch: {canTouch[i]}");

                }

            }
        }

    }

    private void HandleCameraMove()
    {
        if (target == null)
            return;

        // Calcular a nova posi��o da c�mera em fun��o da dist�ncia, rota��o em Y (yaw) e inclina��o em X (pitch)
        Vector3 offset = new Vector3(0, 0, -(followDistance + map.zoom)); // A c�mera segue o alvo a uma dist�ncia constante

        // Criar rota��o com base em pitch (eixo X) e yaw (eixo Y)
        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0);
        Vector3 rotatedOffset = rotation * offset;

        // Definir a posi��o desejada da c�mera
        Vector3 desiredPosition = target.position + rotatedOffset + (Vector3.up * followHeight);

        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSmoothness);

        transform.LookAt(target);

        if (Touchscreen.current != null)
        {
            HandleTouchMove();
        }
    }

    void ZoomCamera(float pinchAmount)
    {
        followDistance = Mathf.Clamp(followDistance + pinchAmount, 5f, 20f); // Manter dist�ncia m�nima e m�xima
    }

    void RotateCamera(Vector2 rotationDelta)
    {
        // Ajustar yaw (rota��o no eixo Y) com o movimento horizontal do toque
        currentYaw += rotationDelta.x * rotationSpeed * Time.deltaTime;

        // Ajustar pitch (inclina��o no eixo X) com o movimento vertical do toque
        currentPitch -= rotationDelta.y * rotationSpeed * Time.deltaTime;

        // Limitar a inclina��o da c�mera para evitar �ngulos estranhos
        currentPitch = Mathf.Clamp(currentPitch, pitchMin, pitchMax);
    }
}
