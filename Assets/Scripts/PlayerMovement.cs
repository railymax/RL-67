using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Camera cam;

    [Header("Movement")]
    [SerializeField] float speed = 5;
    [SerializeField] Vector2 mouseSensivity = new(1, 1);

    Vector2 inputVector;
    float camRotationX;
    Rigidbody rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {
        Vector3 camForward;
        Vector3 camRight;

        GetCamDirections(out camForward, out camRight);

        Vector3 movementDirection = (camForward * inputVector.y) + (camRight * inputVector.x);

        Vector3 movementVector = new(movementDirection.x * speed, rb.linearVelocity.y, movementDirection.z * speed);
        rb.linearVelocity = movementVector;
    }

    public void MoveEvent(InputAction.CallbackContext context)
    {
        inputVector = context.ReadValue<Vector2>();
    }

    public void LookEvent(InputAction.CallbackContext context)
    {
        Vector2 mouseDelta = context.ReadValue<Vector2>();
        transform.Rotate(Vector3.up * mouseDelta.x * mouseSensivity.x);

        camRotationX -= mouseDelta.y * mouseSensivity.y;
        camRotationX = Mathf.Clamp(camRotationX, -90, 90);

        cam.transform.localRotation = Quaternion.Euler(camRotationX, 0, 0);
    }

    void GetCamDirections(out Vector3 forward, out Vector3 right)
    {
        forward = cam.transform.forward;
        right = cam.transform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();
    }
}
