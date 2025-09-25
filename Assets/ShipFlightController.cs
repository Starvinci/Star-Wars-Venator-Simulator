using UnityEngine;

public class ShipFlightController : MonoBehaviour
{
    public enum ShipType { Fighter, Transporter, Bomber }
    [Header("Schiffsauswahl")]
    public ShipType shipType = ShipType.Fighter;

    [Header("Allgemeine Steuerung")]
    public bool startFlight = false;
    public bool isInHangarZone = false;

    [Header("Kamera")]
    public Camera shipCamera;

    [Header("Flugverhalten")]
    public float mouseSensitivity;
    public float thrustPower;
    public float strafePower;
    public float pitchYawSmoothing;
    public float maxSpeed;

    [Header("Landeverhalten")]
    public float landingThrustPower;
    public float landingStrafePower;
    public float landingVerticalSpeed;

    [Header("Rigidbody")]
    public Rigidbody rb;

    private Vector2 currentMouseInput;

    void OnValidate()
    {
        // Schiffseigenschaften je nach Typ setzen
        switch (shipType)
        {
            case ShipType.Fighter:
                mouseSensitivity = 2f;
                thrustPower = 25f;
                strafePower = 12f;
                pitchYawSmoothing = 2f;
                maxSpeed = 60f;
                landingThrustPower = 12f;
                landingStrafePower = 6f;
                landingVerticalSpeed = 6f;
                break;
            case ShipType.Transporter:
                mouseSensitivity = 1.2f;
                thrustPower = 15f;
                strafePower = 7f;
                pitchYawSmoothing = 1.5f;
                maxSpeed = 35f;
                landingThrustPower = 7f;
                landingStrafePower = 3.5f;
                landingVerticalSpeed = 3f;
                break;
            case ShipType.Bomber:
                mouseSensitivity = 1.5f;
                thrustPower = 18f;
                strafePower = 8f;
                pitchYawSmoothing = 1.8f;
                maxSpeed = 40f;
                landingThrustPower = 8f;
                landingStrafePower = 4f;
                landingVerticalSpeed = 4f;
                break;
        }
    }

    private void Start()
    {
        if (shipCamera != null)
        {
            shipCamera.enabled = true;
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (!startFlight)
            return;

        // Maussteuerung aufnehmen
        currentMouseInput.x = Input.GetAxis("Mouse X");
        currentMouseInput.y = -Input.GetAxis("Mouse Y");

        // Nur lenken, wenn Maus weit genug vom Mittelpunkt entfernt ist
        if (currentMouseInput.magnitude > 0.05f)
        {
            Vector3 targetRotation = new Vector3(currentMouseInput.y, currentMouseInput.x, 0f) * mouseSensitivity;
            transform.Rotate(targetRotation * Time.deltaTime * pitchYawSmoothing, Space.Self);
        }

        // Bewegung steuern
        if (!isInHangarZone)
        {
            HandleFlight();
        }
        else
        {
            HandleLandingMode();
        }
    }

    private void HandleFlight()
    {
        if (Input.GetKey(KeyCode.UpArrow))
            rb.AddForce(transform.forward * thrustPower);

        if (Input.GetKey(KeyCode.DownArrow))
            rb.AddForce(-transform.forward * thrustPower * 0.5f);

        if (Input.GetKey(KeyCode.LeftArrow))
            rb.AddForce(-transform.right * strafePower);

        if (Input.GetKey(KeyCode.RightArrow))
            rb.AddForce(transform.right * strafePower);
    }

    private void HandleLandingMode()
    {
        if (Input.GetKey(KeyCode.UpArrow))
            rb.AddForce(transform.forward * landingThrustPower);

        if (Input.GetKey(KeyCode.DownArrow))
            rb.AddForce(-transform.forward * landingThrustPower * 0.5f);

        if (Input.GetKey(KeyCode.LeftArrow))
            rb.AddForce(-transform.right * landingStrafePower);

        if (Input.GetKey(KeyCode.RightArrow))
            rb.AddForce(transform.right * landingStrafePower);

        if (Input.GetKey(KeyCode.Space))
            rb.AddForce(Vector3.up * landingVerticalSpeed);

        if (Input.GetKey(KeyCode.LeftShift))
            rb.AddForce(Vector3.down * landingVerticalSpeed);
    }
}