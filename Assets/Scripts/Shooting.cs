using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject bulletPrefab;    // Erstes Projektil
    public GameObject bulletPrefab2;   // Zweites Projektil
    public float bulletSpeed = 50f;    // Geschwindigkeit der Projektile
    public float targetRange = 1000f;  // Reichweite nach der das Projektil gelöscht wird

    public void Shoot()
    {
        // Skalierungsvektor für beide Projektile
        Vector3 scale = new Vector3(0.7f, 2f, 0.7f);
        
        // *** Klonen des ersten Projektils (bulletPrefab) ***
        GameObject bullet1 = Instantiate(bulletPrefab, bulletPrefab.transform.position, bulletPrefab.transform.rotation);
        
        // Setze die Skalierung des Klons auf (0.7, 2, 0.7)
        bullet1.transform.localScale = scale;
        
        Rigidbody rb1 = bullet1.GetComponent<Rigidbody>();
        if (rb1 != null)
        {
            // Beschleunige das Projektil entlang der lokalen Y-Achse des Projektils
            rb1.velocity = -bullet1.transform.up * bulletSpeed;  // Bewege das Projektil entlang der negativen Y-Achse (nach oben relativ zur lokalen Achse)
            rb1.constraints = RigidbodyConstraints.FreezeRotation;  // Rotation einfrieren
            
            // Lösche das Projektil nach Erreichen der Zielreichweite
            StartCoroutine(DestroyAfterDistance(bullet1, targetRange));
        }
        else
        {
            Debug.LogWarning("Kein Rigidbody auf bulletPrefab gefunden!");
        }

        // *** Klonen des zweiten Projektils (bulletPrefab2) ***
        GameObject bullet2 = Instantiate(bulletPrefab2, bulletPrefab2.transform.position, bulletPrefab2.transform.rotation);
        
        // Setze die Skalierung des Klons auf (0.7, 2, 0.7)
        bullet2.transform.localScale = scale;
        
        Rigidbody rb2 = bullet2.GetComponent<Rigidbody>();
        if (rb2 != null)
        {
            // Beschleunige das Projektil entlang der lokalen Y-Achse des Projektils
            rb2.velocity = -bullet2.transform.up * bulletSpeed;  // Bewege das Projektil entlang der negativen Y-Achse (nach oben relativ zur lokalen Achse)
            rb2.constraints = RigidbodyConstraints.FreezeRotation;  // Rotation einfrieren

            // Lösche das Projektil nach Erreichen der Zielreichweite
            StartCoroutine(DestroyAfterDistance(bullet2, targetRange));
        }
        else
        {
            Debug.LogWarning("Kein Rigidbody auf bulletPrefab2 gefunden!");
        }
    }

    // Coroutine, um das Projektil nach einer bestimmten Reichweite zu löschen
    IEnumerator DestroyAfterDistance(GameObject bullet, float maxDistance)
    {
        Vector3 startPosition = bullet.transform.position;

        while (Vector3.Distance(startPosition, bullet.transform.position) < maxDistance)
        {
            yield return null;  // Warte bis zum nächsten Frame
        }

        // Lösche das Projektil, sobald es die maximale Reichweite erreicht hat
        Destroy(bullet);
    }
}
