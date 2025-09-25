using UnityEngine;

public class HangarDoorLeft : MonoBehaviour
{
    public Animator animator;
    public string openState = "open_left";
    public string closeState = "close_left";

    public AudioSource audioSource;
    public AudioClip openSound;
    public AudioClip closeSound;

    private bool isOpen = false;

    [ContextMenu("Open Left Door")]
    public void OpenDoor()
    {
        if (!isOpen)
        {
            animator.Play(openState, 0, 0f);
            if (audioSource && openSound)
                audioSource.PlayOneShot(openSound);
            isOpen = true;
            Debug.Log("Linke Tür geöffnet");
        }
    }

    [ContextMenu("Close Left Door")]
    public void CloseDoor()
    {
        if (isOpen)
        {
            animator.Play(closeState, 0, 0f);
            if (audioSource && closeSound)
                audioSource.PlayOneShot(closeSound);
            isOpen = false;
            Debug.Log("Linke Tür geschlossen");
        }
    }
}
