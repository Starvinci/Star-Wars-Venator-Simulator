using UnityEngine;
using UnityEngine.Video;

public class PlayAndFreezeVideo : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Der VideoPlayer, der das Video abspielt
    public Camera mainCamera; // Referenz auf die Hauptkamera
    public RenderTexture renderTexture; // RenderTexture für das Video
    public float scale = 1.0f; // Variable zur Skalierung des Videos

    private GameObject videoQuad; // Das Quad, auf dem das Video angezeigt wird

    void Start()
    {
        // Wenn keine Kamera gesetzt ist, standardmäßig die MainCamera verwenden
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        // Video in der Mitte der Kamera platzieren und skalieren
        CreateVideoQuad();

        // Starte das Video
        PlayVideo();
    }

    // Methode zum Erstellen des Quads für das Video
    private void CreateVideoQuad()
    {
        // Quad erstellen
        videoQuad = GameObject.CreatePrimitive(PrimitiveType.Quad);

        // Quad in der Mitte der Kamera platzieren
        Vector3 cameraCenter = mainCamera.transform.position + mainCamera.transform.forward * 5f;
        videoQuad.transform.position = cameraCenter;

        // Quad in Richtung der Kamera drehen
        videoQuad.transform.rotation = Quaternion.LookRotation(mainCamera.transform.forward);

        // Quad skalieren
        videoQuad.transform.localScale = new Vector3(scale, scale, 1);

        // Material mit RenderTexture festlegen
        if (renderTexture != null)
        {
            videoQuad.GetComponent<Renderer>().material.mainTexture = renderTexture;
        }
    }

    // Methode zum Starten des Videos
    public void PlayVideo()
    {
        if (videoPlayer != null)
        {
            videoPlayer.targetTexture = renderTexture; // Video auf die RenderTexture rendern
            videoPlayer.Play(); // Video abspielen

            // Event, wenn das Video fertig ist
            videoPlayer.loopPointReached += OnVideoEnd;
        }
    }

    // Methode, die aufgerufen wird, wenn das Video endet
    private void OnVideoEnd(VideoPlayer vp)
    {
        // Das letzte Frame bleibt stehen, da das Video angehalten wird
        vp.Pause();
    }

    // Methode zum Entfernen des Videos und Quads
    public void RemoveVideo()
    {
        if (videoPlayer != null)
        {
            videoPlayer.Stop(); // Video stoppen
        }

        if (videoQuad != null)
        {
            Destroy(videoQuad); // Das Quad löschen
        }
    }
}
