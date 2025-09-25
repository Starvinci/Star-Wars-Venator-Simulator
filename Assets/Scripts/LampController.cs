using System.Collections.Generic;
using UnityEngine;

public class LampController : MonoBehaviour
{
    // Public attributes for flexible lamp and object control
    public List<Light> normalLights; // List of normal lights
    public List<Renderer> normalLampMaterials; // List of normal lamp materials (objects)

    public List<Light> alarmLights;  // List of alarm lights
    public List<Renderer> alarmLampMaterials;  // List of alarm lamp materials (objects)

    public Color emissionOffColor = Color.black;  // Emission color when lights are off
    public Color normalEmissionColor = Color.white; // Emission color for normal mode (white)
    public Color alarmEmissionColor = Color.red;   // Emission color for alarm mode (red)
    public Color alarmLightColor = Color.red;      // Light color for alarm lights

    // Enum for modes
    public enum LampMode
    {
        Normal,
        Off,
        Alarm
    }

    public LampMode currentMode = LampMode.Normal; // Public mode to be controlled externally
    private LampMode lastMode = LampMode.Normal;   // Store last mode for change detection

    void Start()
    {
        // Set initial mode
        SetLampMode(currentMode);
    }

    void Update()
    {
        // Detect if the mode has changed
        if (currentMode != lastMode)
        {
            SetLampMode(currentMode);
            lastMode = currentMode; // Update last mode
        }
    }

    // Function to set the mode of the lamps and objects
    public void SetLampMode(LampMode mode)
    {
        switch (mode)
        {
            case LampMode.Normal:
                SetNormalMode();
                break;
            case LampMode.Off:
                SetOffMode();
                break;
            case LampMode.Alarm:
                SetAlarmMode();
                break;
        }
    }

    // Function to set the LampMode using a string
    public void SetLampModeFromString(string mode)
    {
        switch (mode.ToLower()) // Convert string to lowercase to handle case-insensitive comparison
        {
            case "normal":
                SetLampMode(LampMode.Normal);
                break;
            case "off":
                SetLampMode(LampMode.Off);
                break;
            case "alarm":
                SetLampMode(LampMode.Alarm);
                break;
            default:
                Debug.LogError("Invalid LampMode string: " + mode);
                break;
        }
    }

    // Set all lamps and objects to normal mode: Normal lights and objects on, Alarm off
    void SetNormalMode()
    {
        // Turn on normal lights (with default white color)
        foreach (Light light in normalLights)
        {
            light.enabled = true;
            light.color = normalEmissionColor; // Ensure normal lights are white
        }

        // Enable emission on normal lamp materials (white)
        foreach (Renderer renderer in normalLampMaterials)
        {
            renderer.material.SetColor("_EmissionColor", normalEmissionColor);
            renderer.material.EnableKeyword("_EMISSION"); // Enable emission
        }

        // Turn off alarm lights and objects
        foreach (Light light in alarmLights)
        {
            light.enabled = false;
        }

        foreach (Renderer renderer in alarmLampMaterials)
        {
            renderer.material.SetColor("_EmissionColor", emissionOffColor); // Turn off emission for alarm materials
            renderer.material.DisableKeyword("_EMISSION");
        }
    }

    // Turn off all lights and objects
    void SetOffMode()
    {
        // Turn off normal lights
        foreach (Light light in normalLights)
        {
            light.enabled = false;
        }

        // Turn off alarm lights
        foreach (Light light in alarmLights)
        {
            light.enabled = false;
        }

        // Disable emission on normal lamp materials
        foreach (Renderer renderer in normalLampMaterials)
        {
            renderer.material.SetColor("_EmissionColor", emissionOffColor);
            renderer.material.DisableKeyword("_EMISSION");
        }

        // Disable emission on alarm lamp materials
        foreach (Renderer renderer in alarmLampMaterials)
        {
            renderer.material.SetColor("_EmissionColor", emissionOffColor);
            renderer.material.DisableKeyword("_EMISSION");
        }
    }

    // Set alarm mode: Normal lights and objects off, Alarm lights and objects on
    void SetAlarmMode()
    {
        // Turn off normal lights
        foreach (Light light in normalLights)
        {
            light.enabled = false;
        }

        // Disable emission on normal lamp materials
        foreach (Renderer renderer in normalLampMaterials)
        {
            renderer.material.SetColor("_EmissionColor", emissionOffColor);
            renderer.material.DisableKeyword("_EMISSION");
        }

        // Turn on alarm lights and set them to red
        foreach (Light light in alarmLights)
        {
            light.enabled = true;
            light.color = alarmLightColor; // Set light color to red
        }

        // Enable emission on alarm lamp materials and set emission color to red
        foreach (Renderer renderer in alarmLampMaterials)
        {
            renderer.material.SetColor("_EmissionColor", alarmEmissionColor); // Set emission color to red
            renderer.material.EnableKeyword("_EMISSION");
        }
    }
}
