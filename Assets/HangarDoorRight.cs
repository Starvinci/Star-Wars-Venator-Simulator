using UnityEngine;

public class HangarDoorRight : MonoBehaviour
{
    public Animator animator;
    public string openState = "open_right";
    public string closeState = "close_right";

    public AudioSource audioSource;
    public AudioClip openSound;
    public AudioClip closeSound;

    private bool isOpen = false;

    [ContextMenu("Open Right Door")]
    public void OpenDoor()
    {
        if (!isOpen)
        {
            animator.Play(openState, 0, 0f);
            if (audioSource && openSound)
                audioSource.PlayOneShot(openSound);
            isOpen = true;
            Debug.Log("Rechte Tür geöffnet");
        }
    }

    [ContextMenu("Close Right Door")]
    public void CloseDoor()
    {
        if (isOpen)
        {
            animator.Play(closeState, 0, 0f);
            if (audioSource && closeSound)
                audioSource.PlayOneShot(closeSound);
            isOpen = false;
            Debug.Log("Rechte Tür geschlossen");
        }
    }
}
