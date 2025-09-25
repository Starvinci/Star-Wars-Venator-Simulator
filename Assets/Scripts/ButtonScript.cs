using System.Collections;
using UnityEngine;
using UnityEngine.Events; // Erlaubt das Verwenden von UnityEvents

public class ButtonScript : MonoBehaviour
{
    public Vector3 pressedOffset = new Vector3(0, -0.2f, 0); // Offset nach unten bei Druck
    public float pressDuration = 0.2f; // Dauer, wie lange der Button gedrückt bleibt
    public Color pressedColor = Color.red; // Farbe nach dem Drücken
    public Color originalColor = Color.white; // Ursprüngliche Farbe des Buttons

    public UnityEvent onButtonPressed; // Event, das bei Button-Druck ausgelöst wird

    private Vector3 originalPosition; // Originalposition des Buttons
    private Renderer buttonRenderer; // Renderer des Buttons

    void Start()
    {
        // Speichere die Originalposition und den Renderer
        originalPosition = transform.localPosition;
        buttonRenderer = GetComponent<Renderer>();

        // Setze die ursprüngliche Farbe des Buttons
        buttonRenderer.material.color = originalColor;
    }

    // Diese Funktion wird aufgerufen, wenn der Button gedrückt wird
    public void OnButtonPressed()
    {
        // Starte den Press-Button-Prozess
        StartCoroutine(PressButton());

        // Führe die im Inspector zugewiesene Funktion aus
        if (onButtonPressed != null)
        {
            onButtonPressed.Invoke();
        }
    }

    // Coroutine für das Drücken des Buttons
    private IEnumerator PressButton()
    {
        buttonRenderer.material.color = pressedColor;

        // Warte für eine kurze Zeit (Dauer des Drucks)
        yield return new WaitForSeconds(pressDuration);
        buttonRenderer.material.color = originalColor;
    }
}
