using UnityEngine;
using System.Collections;

public class AutomaticDoor : MonoBehaviour
{
    public Animator doorAnimator;       // Referenz zum Animator der Tür
    public string tagToDetect = "Player";  // Tag des Spielers (oder Klon)
    public float closeDelay = 5f;       // Verzögerung, bevor die Tür schließt (optional)
    private bool isPlayerInRange = false;  // Verfolgt, ob der Spieler oder ein Klon im Bereich ist
    public bool doorOpened = false;     // Verfolgt, ob die Tür geöffnet ist
    private Coroutine closeDoorCoroutine;  // Referenz zur Schließ-Coroutine

    public AudioSource audioSource;
    public AudioClip openSound;   // <--- Öffnen-Sound
    public AudioClip closeSound;  // <--- Schließen-Sound

    private bool isHeldOpen = false;    // Verfolgt, ob die Tür manuell offen gehalten wird

    void OnTriggerEnter(Collider other)
    {
        // Überprüfen, ob der Spieler oder ein "Clone"-Objekt den Trigger betritt
        if ((other.CompareTag(tagToDetect) || other.name.Contains("Clone")) && !doorOpened)
        {
            Debug.Log("Trigger DOOR open");
            OpenDoor();
        }
        else if ((other.CompareTag(tagToDetect) || other.name.Contains("Clone")) && doorOpened)
        {
            // Setzt die Schließ-Coroutine zurück, wenn der Spieler erneut in den Bereich tritt
            if (closeDoorCoroutine != null)
            {
                StopCoroutine(closeDoorCoroutine);
                closeDoorCoroutine = null;
            }
        }
    }

    // Tür öffnen
    public void OpenDoor()
    {
        // Überprüfen, ob die Tür manuell offen gehalten wird
        if (isHeldOpen) return;

        // Setze den Trigger im Animator, um die Tür zu öffnen
        doorAnimator.SetTrigger("OpenTrigger");
        doorOpened = true; // Tür ist nun geöffnet
        isPlayerInRange = true;
        if (audioSource && openSound)
            audioSource.PlayOneShot(openSound); // <--- Öffnen-Sound

        // Falls eine Schließ-Coroutine läuft, stoppen wir sie
        if (closeDoorCoroutine != null)
        {
            StopCoroutine(closeDoorCoroutine);
            closeDoorCoroutine = null;
        }
    }

    // Trigger-Bereich verlassen
    void OnTriggerExit(Collider other)
    {
        // Überprüfen, ob der Spieler oder ein "Clone"-Objekt den Trigger verlässt
        if ((other.CompareTag(tagToDetect) || other.name.Contains("Clone")) && !isHeldOpen)
        {
            Debug.Log("Trigger DOOR close");
            isPlayerInRange = false;

            // Starte die Schließanimation nach einer Verzögerung
            if (closeDoorCoroutine == null)
            {
                closeDoorCoroutine = StartCoroutine(CloseDoorAfterDelay());
            }
        }
    }

    // Coroutine, um die Tür nach einer Verzögerung zu schließen
    IEnumerator CloseDoorAfterDelay()
    {
        yield return new WaitForSeconds(closeDelay);  // Optionale Verzögerung

        // Nur schließen, wenn der Spieler nicht mehr im Bereich ist und die Tür nicht manuell gehalten wird
        if (!isPlayerInRange && !isHeldOpen)
        {
            Debug.Log("Closing door...");
            doorAnimator.SetTrigger("CloseTrigger");
            doorOpened = false; // Tür ist nun geschlossen
            if (audioSource && closeSound)
                audioSource.PlayOneShot(closeSound); // <--- Schließen-Sound
        }

        // Coroutine zurücksetzen
        closeDoorCoroutine = null;
    }

    // Methode, um die Tür manuell offen zu halten
    public void HoldUp()
    {
        // Setze den bool Wert isHeldOpen auf true, um anzuzeigen, dass die Tür manuell offen gehalten wird
        isHeldOpen = true;

        // Wenn die Tür noch geschlossen ist, öffne sie
        if (!doorOpened)
        {
            // Setze den Trigger im Animator, um die Tür zu öffnen
            doorAnimator.SetTrigger("OpenTrigger");
            doorOpened = true; // Tür ist nun geöffnet
            if (audioSource && openSound)
                audioSource.PlayOneShot(openSound); // <--- Öffnen-Sound
        }

        // Stoppe die Schließ-Coroutine, falls sie läuft, um sicherzustellen, dass die Tür nicht schließt
        if (closeDoorCoroutine != null)
        {
            StopCoroutine(closeDoorCoroutine);
            closeDoorCoroutine = null;
        }
        
        Debug.Log("Door is now held open.");
    }

    // Methode, um das manuelle Offenhalten zu beenden
    public void LoseHolding()
    {
        isHeldOpen = false;

        // Wenn der Spieler nicht im Bereich ist, schließe die Tür nach der Verzögerung
        if (!isPlayerInRange)
        {
            if (closeDoorCoroutine == null)
            {
                closeDoorCoroutine = StartCoroutine(CloseDoorAfterDelay());
            }
        }

        Debug.Log("Manual hold released, door will behave normally.");
    }
}
