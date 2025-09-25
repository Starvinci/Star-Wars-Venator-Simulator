using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    // Position, an die der Spieler/KI teleportiert wird
    public Transform interactionPosition;

    // Unity Event für benutzerdefinierte Aktionen im Inspector
    public UnityEvent onInteractionStart;
    public UnityEvent onInteractionEnd;

    // Animation, die während der Interaktion abgespielt wird
    public string interactionAnimation;

    // Startposition und -rotation speichern, um sie später wiederherzustellen
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    // Referenz auf den Spieler oder die KI
    private GameObject interactingEntity;

    // Wird aufgerufen, um die Interaktion zu starten
    public void StartInteraction(GameObject entity)
    {
        interactingEntity = entity;
        
        // Position und Rotation des Spielers/KI speichern
        originalPosition = entity.transform.position;
        originalRotation = entity.transform.rotation;

        // Spieler/KI an die Interaktionsposition setzen
        entity.transform.position = interactionPosition.position;
        entity.transform.rotation = interactionPosition.rotation;

        // Rigidbody-Komponente holen und Geschwindigkeiten nullen
        Rigidbody rb = entity.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;          // Setzt die Geschwindigkeit auf 0
            rb.angularVelocity = Vector3.zero;   // Setzt die Rotation auf 0
        }

        // Start-Event aufrufen
        onInteractionStart.Invoke();

        // Animation abspielen (sofern Animator vorhanden ist)
        Animator animator = entity.GetComponent<Animator>();
        if (animator != null && !string.IsNullOrEmpty(interactionAnimation))
        {
            animator.Play(interactionAnimation);
        }
    }

    // Wird aufgerufen, um die Interaktion zu beenden
    public void EndInteraction()
    {
        if (interactingEntity == null)
            return;

        // Spieler/KI an die ursprüngliche Position zurücksetzen
        interactingEntity.transform.position = originalPosition;
        interactingEntity.transform.rotation = originalRotation;

        // End-Event aufrufen
        onInteractionEnd.Invoke();

        // Interaktion beenden
        interactingEntity = null;
    }
}
