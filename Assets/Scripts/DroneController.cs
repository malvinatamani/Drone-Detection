using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DroneController : MonoBehaviour
{
    public float liftForce = 9.8f;
    public float moveSpeed = 5f;
    public float rotationSpeed = 100f;
    public float stabilization = 2f;
    public float landingSpeed = 2f;

    private Rigidbody rb;
    private bool isFlying = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
    }

    void Update()
    {
        // Tombol untuk mulai terbang
        if (Input.GetKeyDown(KeyCode.T))
        {
            isFlying = true;
        }

        // Tombol untuk mendarat
        if (Input.GetKeyDown(KeyCode.L))
        {
            isFlying = false;
        }
    }

    void FixedUpdate()
    {
        if (isFlying)
        {
            HandleLift();
            HandleMovement();
            StabilizeRotation();
        }
        else
        {
            LandDrone(); // Turun perlahan saat tidak terbang
        }
    }

    void HandleLift()
    {
        float ascendInput = 0f;

        if (Input.GetKey(KeyCode.Space)) ascendInput = 1f;
        else if (Input.GetKey(KeyCode.LeftControl)) ascendInput = -1f;

        // Hover force tetap diberikan agar drone tidak jatuh
        float hoverForce = liftForce - Physics.gravity.y;

        // Tambahkan kontrol naik/turun dari input
        Vector3 totalLift = Vector3.up * (hoverForce + ascendInput * moveSpeed);
        rb.AddForce(totalLift, ForceMode.Force);
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float ascend = 0f;

        if (Input.GetKey(KeyCode.Space)) ascend = 1f;
        if (Input.GetKey(KeyCode.LeftControl)) ascend = -1f;

        Vector3 moveDir = (transform.forward * vertical + transform.right * horizontal + Vector3.up * ascend).normalized;
        rb.AddForce(moveDir * moveSpeed, ForceMode.Acceleration);

        float yaw = 0f;
        if (Input.GetKey(KeyCode.Q)) yaw = -1f;
        if (Input.GetKey(KeyCode.E)) yaw = 1f;

        rb.AddTorque(Vector3.up * yaw * rotationSpeed);
    }

    void StabilizeRotation()
    {
        Quaternion desiredRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        Quaternion rotationDiff = Quaternion.Inverse(transform.rotation) * desiredRotation;
        Vector3 torque = new Vector3(rotationDiff.x, 0, rotationDiff.z) * stabilization;
        rb.AddTorque(torque);
    }

    void LandDrone()
    {
        // Turun perlahan saat mendarat
        rb.AddForce(Vector3.down * landingSpeed, ForceMode.Acceleration);

        // Stabilkan rotasi saat mendarat
        StabilizeRotation();
    }

    public bool IsFlying()
    {
        return isFlying;
    }

}
