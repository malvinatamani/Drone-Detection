using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DronePhysicsController : MonoBehaviour
{
    public float liftForce = 20f;       // Kekuatan ke atas (SPACE)
    public float movementForce = 50f;   // Kekuatan gerak
    public float rotationTorque = 10f;  // Kekuatan putar (Q/E)

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Terbang ke atas / bawah
        if (Input.GetKey(KeyCode.Space))
            rb.AddForce(Vector3.up * liftForce);

        if (Input.GetKey(KeyCode.LeftControl))
            rb.AddForce(Vector3.down * liftForce);
        if (Input.GetKey(KeyCode.Space))
        {
            Debug.Log("Naik!");
            rb.AddForce(Vector3.up * liftForce);
        }

        // Gerakan ke kiri/kanan/maju/mundur
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        rb.AddRelativeForce(move * movementForce);

        // Putar drone (yaw)
        if (Input.GetKey(KeyCode.Q))
            rb.AddTorque(Vector3.up * -rotationTorque);
        if (Input.GetKey(KeyCode.E))
            rb.AddTorque(Vector3.up * rotationTorque);
    }
}
