using UnityEngine;

public class HangarDoorController : MonoBehaviour
{
    [Header("Animator & State-Namen")]
    public Animator animator;

    [Tooltip("Name des Öffnen-States im Animator")]
    public string openStateName = "Hangar_open";

    [Tooltip("Name des Schließen-States im Animator")]
    public string closeStateName = "Hangar_close";

    private bool isOpen = true;

    private void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    [ContextMenu("Close Door")]
    public void CloseDoor()
    {
        if (isOpen)
        {
            Debug.Log("Spiele 'Hangar_close'");
            animator.Play(closeStateName, 0, 0f); // Spiele Close-Animation von Anfang
            isOpen = false;
        }
    }

    [ContextMenu("Open Door")]
    public void OpenDoor()
    {
        if (!isOpen)
        {
            Debug.Log("Spiele 'Hangar_open'");
            animator.Play(openStateName, 0, 0f); // Spiele Open-Animation von Anfang
            isOpen = true;
        }
    }
}
