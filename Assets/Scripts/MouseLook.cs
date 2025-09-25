using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] private float _speed = 7f;
    public bool is_Locked = false;

    [SerializeField] private float _mouseSensitivity = 50f;
    [SerializeField] private float _minCameraview = -70f, _maxCameraview = 80f;
    [SerializeField] private float smoothTime = 0.1f;  // Smoothing factor for mouse movement
    public Rigidbody rb;  // Rigidbody reference for player
    public Camera _camera;
    private float xRotation = 0f;
    private Vector2 smoothVelocity;
    private Vector2 currentMouseDelta;

    public float rotationResetSpeed = 5f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
    }

    public void set_lock(bool value)
    {
        is_Locked = value;
    }

    void Update()
    {
        if (is_Locked)
        {
            // Read mouse input
            float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity;

            // Apply deadzone to prevent small residual movements
            float threshold = 0.01f;
            if (Mathf.Abs(mouseX) < threshold) mouseX = 0f;
            if (Mathf.Abs(mouseY) < threshold) mouseY = 0f;

            Vector2 targetMouseDelta = new Vector2(mouseX, mouseY);

            // Smooth mouse input using SmoothDamp
            currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref smoothVelocity, smoothTime);

            // If there's no input, do not update the rotation
            if (currentMouseDelta != Vector2.zero)
            {
                // Rotate the camera based on the Y input of the mouse
                xRotation -= currentMouseDelta.y;
                xRotation = Mathf.Clamp(xRotation, _minCameraview, _maxCameraview);
                _camera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

                // Rotate the player based on the X input of the mouse
                transform.Rotate(Vector3.up * currentMouseDelta.x);
            }
        }
    }
}
