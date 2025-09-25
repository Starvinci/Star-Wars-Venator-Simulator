using UnityEngine;
using System.Collections.Generic;

public class MaterialFarbtonAnpasser : MonoBehaviour
{
    public Material material;
    [Range(0, 10)]
    public int intWert = 7;

    [System.Serializable]
    public class FireData
    {
        public Transform position;
        public GameObject fireObject;
    }
    public List<FireData> fireDataList = new List<FireData>();

    void Update()
    {
        if (material == null)
            return;

        UpdateMaterialColorAndEmission();

        if (intWert >= 6)
        {
            ActivateAndAdjustFires();
        }
        else
        {
            DeactivateFires();
        }
    }

    void UpdateMaterialColorAndEmission()
    {
        if (intWert == 0)
        {
            // Farbe auf Schwarz setzen und Emission deaktivieren
            material.color = Color.black;
            material.DisableKeyword("_EMISSION");
            material.SetColor("_EmissionColor", Color.black);
        }
        else
        {
            float hue;
            float saturation = 1f;
            float value;

            // Mappe intWert von 1-10 auf einen Hue-Wert zwischen 240° (Blau) und 180° (Türkis)
            hue = (240f - ((intWert - 1) * (60f / 9f))) / 360f; // Normalisiert auf 0-1 für HSV

            // Berechne den Helligkeitswert (Value) basierend auf intWert
            // Höhere Werte führen zu helleren Farben
            value = 0.4f + ((intWert - 1) * (0.6f / 9f)); // Value zwischen 0.4 und 1.0

            // Erzeuge die neue Farbe im HSV-Farbraum
            Color neueFarbe = Color.HSVToRGB(hue, saturation, value);
            material.color = neueFarbe;

            // Emissionsintensität basierend auf intWert
            float emissionIntensity = (float)intWert / 10f;

            // Emission aktivieren und setzen
            material.EnableKeyword("_EMISSION");
            Color emissionColor = neueFarbe * emissionIntensity;
            material.SetColor("_EmissionColor", emissionColor);
        }
    }

    void ActivateAndAdjustFires()
    {
        foreach (FireData fireData in fireDataList)
        {
            GameObject fire = fireData.fireObject;
            if (fire != null)
            {
                if (!fire.activeSelf)
                    fire.SetActive(true);

                // Position und Rotation anpassen
                if (fireData.position != null)
                {
                    fire.transform.position = fireData.position.position;
                }

                // Partikelsystem anpassen
                AdjustParticleSystem(fire);
            }
        }
    }

    void DeactivateFires()
    {
        foreach (FireData fireData in fireDataList)
        {
            GameObject fire = fireData.fireObject;
            if (fire != null && fire.activeSelf)
                fire.SetActive(false);
        }
    }

    void AdjustParticleSystem(GameObject fire)
    {
        ParticleSystem ps = fire.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            // Hauptmodul anpassen
            var main = ps.main;
            main.startColor = new ParticleSystem.MinMaxGradient(material.color);

            // Startgeschwindigkeit abhängig von intWert (6-10)
            float minSpeed = 10f; // Mindestgeschwindigkeit
            float maxSpeed = 100f; // Höchstgeschwindigkeit
            main.startSpeed = Mathf.Lerp(minSpeed, maxSpeed, (intWert - 6f) / 4f); // intWert von 6 bis 10

            // Emissionsrate abhängig von intWert
            var emission = ps.emission;
            float minEmission = 100f; // Minimale Emissionsrate
            float maxEmission = 800f; // Maximale Emissionsrate
            emission.rateOverTime = Mathf.Lerp(minEmission, maxEmission, (intWert - 6f) / 4f);

            // Farbe über die Lebensdauer anpassen
            var colorOverLifetime = ps.colorOverLifetime;
            if (colorOverLifetime.enabled)
            {
                Gradient gradient = new Gradient();
                gradient.SetKeys(
                    new GradientColorKey[] { new GradientColorKey(material.color, 0.0f), new GradientColorKey(material.color, 1.0f) },
                    new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) }
                );
                colorOverLifetime.color = new ParticleSystem.MinMaxGradient(gradient);
            }

            // Trails anpassen
            var trails = ps.trails;
            if (trails.enabled)
            {
                trails.colorOverLifetime = new ParticleSystem.MinMaxGradient(material.color);
            }
        }
    }
}
