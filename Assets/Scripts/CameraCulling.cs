using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraCulling : MonoBehaviour
{
    // Public variables to assign in the inspector
    public Camera mainCamera;             // Die Kamera, die überwacht wird
    public GameObject interiorObject;     // Das Interieur-Objekt, in dessen Bounds wir die Kamera prüfen
    public List<MeshRenderer> interiorRenderers;  // Liste von MeshRenderern für das Interieur
    public List<MeshRenderer> exteriorRenderers;  // Liste von MeshRenderern für das Exterieur

    private bool isInInterior = false;    // Verfolgt, ob die Kamera im Interieur ist

    void Start()
    {
        // Starte die regelmäßige Überprüfung der Kamera-Position
        StartCoroutine(CheckCameraPosition());
    }

    // Coroutine, die die Kamera-Position jede Sekunde überprüft
    IEnumerator CheckCameraPosition()
    {
        while (true)
        {
            // Check if the camera is inside the interior object bounds
            if (IsCameraInsideInterior())
            {
                // Wenn die Kamera im Interieur ist und der Layer nicht bereits gesetzt ist
                if (!isInInterior)
                {
                    DisableRenderers(exteriorRenderers);
                    EnableRenderers(interiorRenderers);
                    isInInterior = true;
                }
            }
            else
            {
                // Wenn die Kamera außerhalb ist und der Layer nicht bereits gesetzt ist
                if (isInInterior)
                {
                    EnableRenderers(exteriorRenderers);
                    DisableRenderers(interiorRenderers);
                    isInInterior = false;
                }
            }

            // Warte eine Sekunde, bevor die Position erneut überprüft wird
            yield return new WaitForSeconds(1f);
        }
    }

    // Check if the camera is within the bounds of the interior object
    bool IsCameraInsideInterior()
    {
        // Get the renderer of the interior object to access its bounds
        Renderer interiorRenderer = interiorObject.GetComponent<Renderer>();

        if (interiorRenderer != null)
        {
            // Check if the camera's position is within the bounds of the interior object
            return interiorRenderer.bounds.Contains(mainCamera.transform.position);
        }

        // If there is no renderer or no bounds, return false
        return false;
    }

    // Disable all renderers in the provided list
    void DisableRenderers(List<MeshRenderer> renderers)
    {
        foreach (MeshRenderer renderer in renderers)
        {
            if (renderer != null)
            {
                renderer.enabled = false;
            }
        }
    }

    // Enable all renderers in the provided list
    void EnableRenderers(List<MeshRenderer> renderers)
    {
        foreach (MeshRenderer renderer in renderers)
        {
            if (renderer != null)
            {
                renderer.enabled = true;
            }
        }
    }
}
