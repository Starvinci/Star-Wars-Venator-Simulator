using System.Collections;
using UnityEngine;

public class LoopingAnimation : MonoBehaviour
{
    public Animator animator; // Referenz auf den Animator
    public string animationName = "Animation"; // Name der Animation, die wiederholt werden soll
    public float delayBetweenRepeats = 10.0f; // Wartezeit zwischen den Wiederholungen

    void Start()
    {
        // Startet die Coroutine, die die Animation wiederholt abspielt
        StartCoroutine(PlayAnimationWithDelay());
    }

    IEnumerator PlayAnimationWithDelay()
    {
        while (true) // Endlosschleife für die Animation
        {
            // Spiele die Animation
            animator.Play(animationName);
            
            // Warte, bis die Animation gestartet wurde
            yield return new WaitForEndOfFrame(); // Warten bis zum Ende des Frames, um sicherzustellen, dass die Animation startet

            // Warte, bis die Animation fertig abgespielt ist
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
            
            // Wartezeit von 10 Sekunden nach der Animation
            yield return new WaitForSeconds(delayBetweenRepeats);
        }
    }
}
