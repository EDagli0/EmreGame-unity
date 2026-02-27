using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class RTSCameraController : MonoBehaviour
{
    [Header("Pan")]
    public float panSpeed = 20f;
    public float dragSpeed = 1f;

    [Header("Zoom")]
    public float zoomSpeed = 12f;   // Input System'de scroll daha küçük gelir
    public float minHeight = 8f;
    public float maxHeight = 35f;

    [Header("Bounds")]
    public Vector2 xLimits = new(-40, 40);
    public Vector2 zLimits = new(-40, 40);

    private Vector3 lastMousePos;

    private void Update()
    {
        HandleKeyboardPan();
        HandleMouseDragPan();
        HandleZoom();
        ClampToBounds();
    }

    private void HandleKeyboardPan()
    {
        // WASD / ok tuþlarý
        Vector2 move = Vector2.zero;
        var kb = Keyboard.current;
        if (kb == null) return;

        if (kb.aKey.isPressed || kb.leftArrowKey.isPressed) move.x -= 1f;
        if (kb.dKey.isPressed || kb.rightArrowKey.isPressed) move.x += 1f;
        if (kb.wKey.isPressed || kb.upArrowKey.isPressed) move.y += 1f;
        if (kb.sKey.isPressed || kb.downArrowKey.isPressed) move.y -= 1f;

        if (move.sqrMagnitude > 1f) move.Normalize();

        Vector3 dir = new Vector3(move.x, 0f, move.y);
        transform.position += dir * panSpeed * Time.deltaTime;
    }

    private void HandleMouseDragPan()
    {
        var mouse = Mouse.current;
        if (mouse == null) return;

        // Orta tuþ drag
        if (mouse.middleButton.wasPressedThisFrame)
            lastMousePos = mouse.position.ReadValue();

        if (mouse.middleButton.isPressed)
        {
            Vector3 current = mouse.position.ReadValue();
            Vector3 delta = current - lastMousePos;
            lastMousePos = current;

            Vector3 right = transform.right; right.y = 0;
            Vector3 forward = transform.forward; forward.y = 0;

            Vector3 move = (-right * delta.x - forward * delta.y) * (dragSpeed * Time.deltaTime);
            transform.position += move;
        }
    }

    private void HandleZoom()
    {
        var mouse = Mouse.current;
        if (mouse == null) return;

        float scroll = mouse.scroll.ReadValue().y; // genelde 120/-120 gibi gelir
        if (Mathf.Abs(scroll) < 0.01f) return;

        Vector3 pos = transform.position;
        pos.y = Mathf.Clamp(pos.y - (scroll * zoomSpeed * Time.deltaTime), minHeight, maxHeight);
        transform.position = pos;
    }

    private void ClampToBounds()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, xLimits.x, xLimits.y);
        pos.z = Mathf.Clamp(pos.z, zLimits.x, zLimits.y);
        transform.position = pos;
    }
}