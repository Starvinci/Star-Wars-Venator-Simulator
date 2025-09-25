using System.Collections;
using UnityEngine;

public class AlarmSystem : MonoBehaviour
{
    private Material objectMaterial; // Materialinstanz für dieses spezifische Objekt
    public Color normalColor = Color.white; // Standardfarbe des Materials
    public Color alarmColor = Color.red; // Alarmfarbe, in die das Material wechselt
    public float emissionIntensity = 2f; // Die Stärke der Emission beim Aufleuchten

    private bool isAlarmActive = false; // Verfolgt, ob der Alarm aktiv ist
    private Coroutine alarmCoroutine;

    void Start()
    {
        // Erstelle eine Kopie des Materials nur für dieses Objekt
        objectMaterial = GetComponent<Renderer>().material;

        // Setze das Material auf die Standardfarbe
        objectMaterial.color = normalColor;
        objectMaterial.DisableKeyword("_EMISSION");
    }

    // Diese Methode aktiviert den Alarm
    public void SetAlarm()
    {
        if (!isAlarmActive)
        {
            isAlarmActive = true;

            // Starte die Coroutine, um den Alarm zu aktivieren
            alarmCoroutine = StartCoroutine(AlarmEffect());
        }
    }

    // Diese Methode deaktiviert den Alarm
    public void StopAlarm()
    {
        if (isAlarmActive)
        {
            isAlarmActive = false;

            // Stoppe die Alarm-Coroutine und setze die Farben zurück
            if (alarmCoroutine != null)
            {
                StopCoroutine(alarmCoroutine);
                alarmCoroutine = null;
            }

            // Setze das Material zurück auf die Standardfarbe und deaktiviere die Emission
            objectMaterial.color = normalColor;
            objectMaterial.DisableKeyword("_EMISSION");
            objectMaterial.SetColor("_EmissionColor", Color.black);
        }
    }

    // Coroutine, um den Alarm-Effekt zu steuern
    IEnumerator AlarmEffect()
    {
        while (isAlarmActive)
        {
            // Setze die Alarmfarbe und Emission
            objectMaterial.color = alarmColor;
            objectMaterial.EnableKeyword("_EMISSION");
            objectMaterial.SetColor("_EmissionColor", alarmColor * emissionIntensity);

            // Warte 0.5 Sekunden
            yield return new WaitForSeconds(0.5f);

            // Setze die Standardfarbe zurück und deaktiviere die Emission
            objectMaterial.color = normalColor;
            objectMaterial.DisableKeyword("_EMISSION");
            objectMaterial.SetColor("_EmissionColor", Color.black);

            // Warte wieder 0.5 Sekunden
            yield return new WaitForSeconds(0.5f);
        }
    }
}
