using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Camera playerCamera;  // Die Kamera, die im Inspector hinterlegt wird
    public float zoomSpeed = 2f; // Geschwindigkeit des Zoomens
    public float zoomFactor = 0.8f; // Der Zoomfaktor (20% reinzoomen)
    private float defaultFOV;    // Speichert das ursprüngliche Sichtfeld der Kamera

    public bool handsFree = true; // Gibt an, ob die Hände frei sind oder nicht (z.B. ob eine Waffe gehalten wird)

    public Transform thirdPersonPosition; // GameObject ohne Renderer, das die Third-Person-Position markiert
    public Transform firstPersonPositionTransform; // GameObject ohne Renderer, das die Ego-Perspektive markiert

    private bool isFirstPerson = true;     // Gibt an, ob die Kamera in der Ego-Perspektive ist

    public bool is_locked = false;

    void Start()
    {
        if (playerCamera != null)
        {
            defaultFOV = playerCamera.fieldOfView; // Speichert das ursprüngliche Sichtfeld der Kamera
        }
    }

    void Update()
    {
        if (!is_locked)
        {
            // Linksklick für Interaktionen
            if (Input.GetMouseButtonDown(0))
            {
                HandleInteraction();
            }

            // Rechtsklick gedrückt halten für Zoom
            if (Input.GetMouseButton(1))
            {
                HandleZoom(true); // Reinzoomen
            }
            else
            {
                HandleZoom(false); // Zurück auf das normale Sichtfeld
            }

            // Mausrad Scrollen nach oben oder unten für Kamera-Position Wechsel
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                SwitchToFirstPerson();  // Scrollen nach vorne -> Ego-Perspektive
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                SwitchToThirdPerson();  // Scrollen nach hinten -> Third-Person-Perspektive
            }
        }
    }

    // Methode zur Handhabung der Interaktion (Linksklick)
    void HandleInteraction()
    {
        if (handsFree)
        {
            // Nur interagieren, wenn die Hände frei sind
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Raycast trifft auf jedes Objekt in der Szene
            if (Physics.Raycast(ray, out hit))
            {
                // Überprüfe, ob das getroffene Objekt das "Button"-Tag hat
                if (hit.transform.CompareTag("Button"))
                {
                    // Hole das ButtonScript des getroffenen Objekts
                    ButtonScript button = hit.transform.GetComponent<ButtonScript>();

                    // Wenn das Objekt ein ButtonScript hat, dann führe die Button-Funktion aus
                    if (button != null)
                    {
                        button.OnButtonPressed();
                    }
                }
                // Überprüfe, ob das getroffene Objekt das "Interactable"-Tag hat
                else if (hit.transform.CompareTag("Interactable"))
                {
                    // Hole das InteractableScript des getroffenen Objekts
                    Interactable interactable = hit.transform.GetComponent<Interactable>();

                    // Wenn das Objekt ein InteractableScript hat, dann führe die Interaktion aus
                    if (interactable != null)
                    {
                        interactable.StartInteraction(gameObject);  // Der Spieler tritt in die Interaktion ein
                    }
                }
            }
        }
        else
        {
            // Wenn die Hände nicht frei sind (z.B. eine Waffe gehalten wird), dann schießen oder zielen
            HandleWeaponInteraction();
        }
    }

    // Methode zur Handhabung des Zoomens (Rechtsklick)
    void HandleZoom(bool isZooming)
    {
        if (playerCamera != null)
        {
            if (isZooming)
            {
                // Zoom während der rechten Maustaste gedrückt gehalten wird
                playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, defaultFOV * zoomFactor, Time.deltaTime * zoomSpeed);
            }
            else
            {
                // Zurück zum Standard-Sichtfeld, wenn die rechte Maustaste losgelassen wird
                playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, defaultFOV, Time.deltaTime * zoomSpeed);
            }
        }
    }

    // Methode zur Handhabung von Aktionen, wenn eine Waffe gehalten wird
    void HandleWeaponInteraction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Waffe: Schießen!"); // Hier könntest du den Schussmechanismus implementieren
        }
        if (Input.GetMouseButton(1))
        {
            Debug.Log("Waffe: Zielen!"); // Hier könntest du das Zielen implementieren
        }
    }

    // Wechsel zur Ego-Perspektive (First-Person) ohne sanften Übergang
    void SwitchToFirstPerson()
    {
        if (!isFirstPerson) // Wechsel nur, wenn nicht schon in der Ego-Perspektive
        {
            // Setze die Position und Rotation direkt auf die Ego-Perspektive
            playerCamera.transform.position = firstPersonPositionTransform.position;
            playerCamera.transform.rotation = firstPersonPositionTransform.rotation;

            isFirstPerson = true;
            Debug.Log("Wechsel zur Ego-Perspektive.");
        }
    }

    public void set_lock(bool value)
    {
        is_locked = value;
    }

    // Wechsel zur Third-Person-Perspektive ohne sanften Übergang
    void SwitchToThirdPerson()
    {
        if (isFirstPerson) // Wechsel nur, wenn nicht schon in der Third-Person-Perspektive
        {
            // Setze die Position und Rotation direkt auf die Third-Person-Position
            playerCamera.transform.position = thirdPersonPosition.position;
            playerCamera.transform.rotation = thirdPersonPosition.rotation;

            isFirstPerson = false;
            Debug.Log("Wechsel zur Third-Person-Perspektive.");
        }
    }
}
