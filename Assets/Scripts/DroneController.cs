using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DroneController : MonoBehaviour
{
    public float speed = 8f;
    public float yawSpeed = 60f;
    public float pitchSpeed = 30f;
    public float altitudeSpeed = 4f;
    public float takeoffHeight = 2f;
    public float maxHeight = 300f;
    public float landingSpeed = 2f;

    private Rigidbody rb;
    private bool isFlying = false;
    private bool isTakingOff = false;
    private bool isLanding = false;
    private bool isReturning = false;
    private Vector3 basePosition;
    private float baseY;
    private float targetAltitude;
    private float verticalVelocity = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        basePosition = transform.position;
        baseY = transform.position.y;
        targetAltitude = baseY;
    }

    void Update()
    {
        HandleInput();
    }

    void FixedUpdate()
    {
        if (isTakingOff)
        {
            HandleTakeOff();
        }
        else if (isLanding)
        {
            HandleLanding();
        }
        else if (isReturning)
        {
            HandleReturnToBase();
        }
        else if (isFlying)
        {
            HandleManualFlight();
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }

    void HandleInput()
    {
        // Takeoff
        if (Input.GetKeyDown(KeyCode.T) && !isFlying && !isTakingOff)
        {
            isTakingOff = true;
            isLanding = false;
            isReturning = false;
            targetAltitude = baseY + takeoffHeight;
        }

        // Landing
        if (Input.GetKeyDown(KeyCode.L) && isFlying)
        {
            isLanding = true;
            isTakingOff = false;
            isReturning = false;
        }

        // Return to base
        if (Input.GetKeyDown(KeyCode.B) && isFlying)
        {
            isReturning = true;
            isLanding = false;
            isTakingOff = false;
        }

        // Saat sudah terbang, atur ketinggian pakai anak panah
        if (isFlying && !isLanding && !isReturning)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                targetAltitude = Mathf.Min(targetAltitude + altitudeSpeed * Time.deltaTime, baseY + maxHeight);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                targetAltitude = Mathf.Max(targetAltitude - altitudeSpeed * Time.deltaTime, baseY);
            }
        }

        // Kontrol rotasi (A/D = yaw, W/S = pitch)
        if (isFlying && !isLanding && !isReturning)
        {
            // Yaw (belok kiri/kanan)
            float yaw = 0f;
            if (Input.GetKey(KeyCode.A)) yaw = -1f;
            if (Input.GetKey(KeyCode.D)) yaw = 1f;
            if (yaw != 0)
            {
                transform.Rotate(0, yaw * yawSpeed * Time.deltaTime, 0, Space.Self);
            }

            // Pitch (naik/turun hidung)
            float pitch = 0f;
            if (Input.GetKey(KeyCode.W)) pitch = 1f;
            if (Input.GetKey(KeyCode.S)) pitch = -1f;
            if (pitch != 0)
            {
                transform.Rotate(pitch * pitchSpeed * Time.deltaTime, 0, 0, Space.Self);
            }
        }
    }

    void HandleTakeOff()
    {
        float currentY = transform.position.y;
        if (currentY < targetAltitude - 0.1f)
        {
            verticalVelocity = Mathf.Lerp(verticalVelocity, altitudeSpeed, Time.fixedDeltaTime * 2f);
            rb.velocity = new Vector3(0, verticalVelocity, 0);
        }
        else
        {
            rb.velocity = Vector3.zero;
            isTakingOff = false;
            isFlying = true;
            targetAltitude = transform.position.y;
        }
    }

    void HandleLanding()
    {
        float currentY = transform.position.y;
        if (currentY > baseY + 0.1f)
        {
            verticalVelocity = Mathf.Lerp(verticalVelocity, -landingSpeed, Time.fixedDeltaTime * 2f);
            rb.velocity = new Vector3(0, verticalVelocity, 0);
        }
        else
        {
            rb.velocity = Vector3.zero;
            isLanding = false;
            isFlying = false;
            transform.position = new Vector3(transform.position.x, baseY, transform.position.z);
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0); // reset pitch/roll
        }
    }

    void HandleReturnToBase()
    {
        Vector3 current = transform.position;
        Vector3 target = new Vector3(basePosition.x, current.y, basePosition.z);

        Vector3 horizontal = (target - current);
        horizontal.y = 0;

        if (horizontal.magnitude > 1f)
        {
            // Rotasi smooth ke arah base
            Quaternion targetRot = Quaternion.LookRotation(horizontal.normalized, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, yawSpeed * Time.deltaTime);

            // Maju ke base
            rb.velocity = transform.forward * speed + Vector3.up * verticalVelocity;

            // Jaga ketinggian di targetAltitude
            float altitudeError = targetAltitude - current.y;
            verticalVelocity = Mathf.Lerp(verticalVelocity, Mathf.Clamp(altitudeError, -altitudeSpeed, altitudeSpeed), Time.fixedDeltaTime * 2f);
        }
        else
        {
            // Setelah sampai base, langsung landing
            isReturning = false;
            isLanding = true;
        }
    }

    void HandleManualFlight()
    {
        // Maju terus
        rb.velocity = transform.forward * speed;

        // Atur ketinggian smooth menuju targetAltitude
        float currentY = transform.position.y;
        float altitudeError = targetAltitude - currentY;
        verticalVelocity = Mathf.Lerp(verticalVelocity, Mathf.Clamp(altitudeError, -altitudeSpeed, altitudeSpeed), Time.fixedDeltaTime * 2f);
        rb.velocity += Vector3.up * verticalVelocity;

        // Clamp max height
        if (transform.position.y > baseY + maxHeight)
        {
            transform.position = new Vector3(transform.position.x, baseY + maxHeight, transform.position.z);
            verticalVelocity = 0;
        }
        if (transform.position.y < baseY)
        {
            transform.position = new Vector3(transform.position.x, baseY, transform.position.z);
            verticalVelocity = 0;
        }
    }

    // Untuk UI/Script lain
    public bool IsFlying()
    {
        return isFlying;
    }
}