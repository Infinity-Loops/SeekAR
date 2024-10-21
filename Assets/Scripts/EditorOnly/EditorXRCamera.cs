using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EditorXRCamera : MonoBehaviour
{
    public float speed = 2f;
    public float sensitivity = 5.0f;

    Vector2 move;
    Vector2 rawMove;
    Vector2 look;

    float height;

    float cameraRotationX;
    float cameraRotationY;

    private float keyScale = 0.05f;

    private bool allowedToRun;

    void HandleGetMoveInput()
    {
        if (Keyboard.current.wKey.isPressed)
        {
            rawMove.y = 1 * keyScale;
        }
        else if (Keyboard.current.sKey.isPressed)
        {
            rawMove.y = -1 * keyScale;
        }
        else
        {
            rawMove.y = 0;
        }

        if (Keyboard.current.aKey.isPressed)
        {
            rawMove.x = -1 * keyScale;
        }
        else if (Keyboard.current.dKey.isPressed)
        {
            rawMove.x = 1 * keyScale;
        }
        else
        {
            rawMove.x = 0;
        }

        move = Vector2.Lerp(move, rawMove, 25 * Time.deltaTime);
    }

    void HandleGetInput()
    {
        look = Mouse.current.delta.ReadValue() * Time.deltaTime;
        HandleGetMoveInput();

        if (Keyboard.current.eKey.isPressed)
        {
            height = Mathf.Lerp(height, 1f, 5 * Time.deltaTime);
        }
        else if (Keyboard.current.qKey.isPressed)
        {
            height = Mathf.Lerp(height, -1f, 5 * Time.deltaTime);
        }
        else
        {
            height = 0;
        }

    }

    void HandleCameraMovement()
    {

        if (Mouse.current.rightButton.isPressed)
        {

            cameraRotationX += look.x * sensitivity;
            cameraRotationY -= look.y * sensitivity;

            Vector3 velocity = (transform.forward * move.y + transform.right * move.x + transform.up * height).normalized;
            transform.position += velocity * speed * 10 * Time.deltaTime;

            Cursor.visible = false;
        }
        else
        {
            Cursor.visible = true;
        }

        transform.eulerAngles = new Vector3(cameraRotationY, cameraRotationX, 0);

    }

    private void Awake()
    {
        allowedToRun = Application.isEditor;
    }

    void Update()
    {
        if (!allowedToRun)
        {
            return;
        }

        HandleGetInput();
        HandleCameraMovement();
    }
}
