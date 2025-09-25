using UnityEngine;
using UnityEngine.Events;

public class GunControl : MonoBehaviour
{
    public Camera turretCamera;  // Die Kamera des Geschützturms, die im Inspector gesetzt wird
    public Transform verticalPivot;  // Objekt für die vertikale Bewegung (z.B. die Kanone)
    public Transform horizontalPivot;  // Objekt für die horizontale Bewegung (z.B. die Drehscheibe)

    public float verticalMinOffset = -10f;  // Minimale vertikale Rotation relativ zur aktuellen Ausrichtung
    public float verticalMaxOffset = 30f;   // Maximale vertikale Rotation relativ zur aktuellen Ausrichtung
    public float horizontalMinOffset = -60f;  // Minimale horizontale Rotation relativ zur aktuellen Ausrichtung
    public float horizontalMaxOffset = 60f;   // Maximale horizontale Rotation relativ zur aktuellen Ausrichtung

    public float rotationSpeed = 50f;  // Geschwindigkeit der Maussteuerung
    public float zoomFactor = 0.8f;    // Der Zoomfaktor
    private float defaultFOV;          // Standard-Sichtfeld der Kamera

    public UnityEvent onShoot;  // Event, das beim Schießen ausgelöst wird
    public UnityEvent onZoomIn; // Event, das beim Zoomen ausgelöst wird
    public UnityEvent onZoomOut; // Event, das ausgelöst wird, wenn das Zoomen aufhört

    private bool isControlling = false;  // Gibt an, ob der Geschützturm aktuell gesteuert wird
    private Camera originalCamera;       // Die ursprüngliche Kamera des Spiels

    public GameObject objectToHide; // Objekt, das bei Verwendung des Turms nicht gerendert werden soll

    // Ausgangsrotationen zur Begrenzung der Bewegung
    private float initialHorizontalRotation;
    private float initialVerticalRotation;

    void Start()
    {
        // Speichere das Standard-Sichtfeld der Kamera
        if (turretCamera != null)
        {
            defaultFOV = turretCamera.fieldOfView;
        }
    }

    void Update()
    {
        if (isControlling)
        {
            HandleTurretControl();
            HandleShooting();
            HandleZoom();
        }
    }

    // Aktiviert die Steuerung des Geschützturms
    public void EnterTurretControl(Camera currentCamera)
    {
        // Setze die aktuelle Kamera als Hauptkamera und aktiviere die Steuerung
        originalCamera = currentCamera;
        turretCamera.enabled = true;
        originalCamera.enabled = false;
        isControlling = true;

        // Speichere die Ausgangsrotationen, um den Bewegungsbereich dynamisch zu begrenzen
        initialHorizontalRotation = horizontalPivot.localEulerAngles.z;
        initialVerticalRotation = verticalPivot.localEulerAngles.x;

        // Objekt ausblenden, damit es nicht von der Turm-Kamera gerendert wird
        if (objectToHide != null)
        {
            objectToHide.layer = LayerMask.NameToLayer("Ignore Turret Camera");
            turretCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("Ignore Turret Camera"));
        }
    }

    // Verlässt die Steuerung des Geschützturms und wechselt zurück zur alten Kamera
    public void ExitTurretControl()
    {
        if (originalCamera != null)
        {
            originalCamera.enabled = true;
            turretCamera.enabled = false;
        }
        isControlling = false;

        // Objekt wieder einblenden
        if (objectToHide != null)
        {
            objectToHide.layer = LayerMask.NameToLayer("Default");  // Setze zurück auf Standard
        }
    }

    // Handhabt die Steuerung des Geschützturms (Mausbewegungen)
    void HandleTurretControl()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

        // HORIZONTALE DREHUNG: Rotieren des horizontalPivot-Objekts um die lokale Z-Achse basierend auf der Mausbewegung
        float newHorizontalRotation = horizontalPivot.localEulerAngles.z + mouseX;

        // Begrenze den horizontalen Winkel relativ zur Ausgangsrotation
        newHorizontalRotation = Mathf.Clamp(WrapAngle(newHorizontalRotation), 
            initialHorizontalRotation + horizontalMinOffset, 
            initialHorizontalRotation + horizontalMaxOffset);

        // Anwenden der Rotation auf das horizontalPivot-Objekt (um die Z-Achse)
        horizontalPivot.localRotation = Quaternion.Euler(0f, 0f, newHorizontalRotation);

        // VERTIKALE DREHUNG: Rotieren des verticalPivot-Objekts um die lokale X-Achse basierend auf der Mausbewegung
        float newVerticalRotation = verticalPivot.localEulerAngles.x + mouseY;

        // Begrenze den vertikalen Winkel relativ zur Ausgangsrotation
        newVerticalRotation = Mathf.Clamp(WrapAngle(newVerticalRotation), 
            initialVerticalRotation + verticalMinOffset, 
            initialVerticalRotation + verticalMaxOffset);

        // Anwenden der Rotation auf das verticalPivot-Objekt
        verticalPivot.localRotation = Quaternion.Euler(newVerticalRotation, 0f, 0f);
    }

    // Methode zur Handhabung des Schießens (Linksklick)
    void HandleShooting()
    {
        if (Input.GetMouseButtonDown(0)) // Linksklick
        {
            onShoot.Invoke();  // Auslösen des Schieß-Events
        }
    }

    // Methode zur Handhabung des Zoomens (Rechtsklick)
    void HandleZoom()
    {
        if (Input.GetMouseButton(1))  // Rechtsklick gedrückt halten für Zoom
        {
            turretCamera.fieldOfView = Mathf.Lerp(turretCamera.fieldOfView, defaultFOV * zoomFactor, Time.deltaTime * 5);
            onZoomIn.Invoke();  // Auslösen des Zoom-Events
        }
        else
        {
            turretCamera.fieldOfView = Mathf.Lerp(turretCamera.fieldOfView, defaultFOV, Time.deltaTime * 5);
            onZoomOut.Invoke(); // Auslösen des Events für das Ende des Zoomens
        }
    }

    // Hilfsmethode zur Anpassung von Winkeln (zwischen 0 und 360 Grad)
    float WrapAngle(float angle)
    {
        if (angle > 180)
            angle -= 360;
        return angle;
    }
}
