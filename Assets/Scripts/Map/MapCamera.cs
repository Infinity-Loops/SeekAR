using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
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

    private float currentYaw = 0f;    // Ângulo de rotação em torno do alvo (eixo Y)
    private float currentPitch = 20f; // Ângulo de rotação para inclinar a câmera (eixo X)

    public float pitchMin = 10f;  // Limite mínimo de inclinação (para evitar inclinar demais)
    public float pitchMax = 80f;  // Limite máximo de inclinação
    private bool canTouch;
    private void Awake()
    {
        mapCamera = GetComponent<Camera>();
        if (Touchscreen.current != null)
        {
            EnhancedTouchSupport.Enable();
        }
    }

    void LateUpdate()
    {
        if (!gameObject.activeInHierarchy)
        {
            return;
        }

        HandleCameraMove();
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
                    if (!EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                    {
                        canTouch = true;
                    }
                    else
                    {
                        canTouch = false;
                    }
                }

                if (touch.phase == TouchPhase.Moved)
                {
                    if (canTouch)
                    {
                        RotateCamera(touch.deltaPosition);
                    }
                }
            }
        }

    }

    private void HandleCameraMove()
    {
        if (target == null)
            return;

        // Calcular a nova posição da câmera em função da distância, rotação em Y (yaw) e inclinação em X (pitch)
        Vector3 offset = new Vector3(0, 0, -(followDistance + map.zoom)); // A câmera segue o alvo a uma distância constante

        // Criar rotação com base em pitch (eixo X) e yaw (eixo Y)
        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0);
        Vector3 rotatedOffset = rotation * offset;

        // Definir a posição desejada da câmera
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
        followDistance = Mathf.Clamp(followDistance + pinchAmount, 5f, 20f); // Manter distância mínima e máxima
    }

    void RotateCamera(Vector2 rotationDelta)
    {
        // Ajustar yaw (rotação no eixo Y) com o movimento horizontal do toque
        currentYaw += rotationDelta.x * rotationSpeed;// * Time.deltaTime;

        // Ajustar pitch (inclinação no eixo X) com o movimento vertical do toque
        currentPitch -= rotationDelta.y * rotationSpeed;// * Time.deltaTime;

        // Limitar a inclinação da câmera para evitar ângulos estranhos
        currentPitch = Mathf.Clamp(currentPitch, pitchMin, pitchMax);
    }
}
