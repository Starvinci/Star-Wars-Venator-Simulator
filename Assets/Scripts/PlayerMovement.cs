using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float walkSpeed = 5f;   // Geschwindigkeit für normales Gehen
    public float runSpeed = 10f;   // Geschwindigkeit für Sprinten
    public Rigidbody rb;  // Rigidbody zur Handhabung der physikbasierten Bewegung
    public Animator animator;     // Referenz zum Animator

    private Vector3 moveDirection; // Bewegungsrichtung des Charakters
    private float currentSpeed;    // Die aktuelle Geschwindigkeit des Charakters

    public bool is_locked = false;

    void Start()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }

        // Freeze rotation on the X and Z axes to prevent tipping over
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    public void set_lock(bool value)
    {
        is_locked = value;
    }
    void Update()
    {
        if (!is_locked){
            // WASD Input abfragen
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");

            // Bewegung basierend auf Eingabe berechnen
            moveDirection = transform.right * moveX + transform.forward * moveZ;

            // Bewegungslänge ermitteln, um festzustellen, ob der Charakter sich bewegt
            float moveMagnitude = moveDirection.magnitude;

            // Setze den Animator-Parameter "Speed" auf den Bewegungsbetrag, um zwischen Animationen zu wechseln
            animator.SetFloat("Speed", moveMagnitude);

            // Überprüfen, ob der Spieler sprintet (Shift-Taste)
            bool isSprinting = Input.GetKey(KeyCode.LeftShift);
            animator.SetBool("isSprinting", isSprinting);

            // Geschwindigkeit basierend auf Sprint-Status einstellen
            currentSpeed = isSprinting ? runSpeed : walkSpeed;

            // Charakter bewegen
            if (moveMagnitude > 0.1f) // Bewegen nur, wenn der Charakter eine signifikante Bewegung macht
            {
                MoveCharacter(currentSpeed);
            }
            else
            {
                rb.velocity = Vector3.zero; // Stoppt das Schlittern nach dem Laufen
            }
        }
    }

    void MoveCharacter(float speed)
    {
        // Berechne die neue Position basierend auf der Bewegungsrichtung und der Geschwindigkeit
        Vector3 newPosition = rb.position + moveDirection.normalized * speed * Time.fixedDeltaTime;

        // Bewege den Charakter unter Berücksichtigung von Kollisionen
        rb.MovePosition(newPosition);
    }
}
