using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class DroneController : MonoBehaviour
{
    public float liftForce = 9.8f;
    public float moveSpeed = 5f;
    public float rotationSpeed = 100f;
    public float stabilization = 2f;
    public float landingSpeed = 2f;
    public float turnSpeed = 2f;
    public float waypointTolerance = 1f;

    private Rigidbody rb;
    private bool isFlying = false;

    // Waypoint fitur belok X ke Z
    public List<Vector3> waypoints = new List<Vector3>();
    private int currentWaypoint = 0;
    public bool isReturning = false;

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

        // Tombol simulasi belok dari X ke Z
        if (Input.GetKeyDown(KeyCode.B))
        {
            // Contoh: drone dari X=0,Z=0 ke X=20,Z=0 lalu ke X=20,Z=10
            float yLevel = transform.position.y;
            SetPath(
                new Vector3(20, yLevel, 0),   // Titik belok di X
                new Vector3(20, yLevel, 10)   // Titik lanjut di Z
            );
        }
    }

    void FixedUpdate()
    {
        if (isFlying)
        {
            HandleLift();

            if (isReturning && waypoints.Count > 0)
            {
                FollowWaypoints();
            }
            else
            {
                HandleMovement();
            }
            StabilizeRotation();
        }
        else
        {
            LandDrone();
        }
    }

    void HandleLift()
    {
        float ascendInput = 0f;

        if (Input.GetKey(KeyCode.Space)) ascendInput = 1f;
        else if (Input.GetKey(KeyCode.LeftControl)) ascendInput = -1f;

        float hoverForce = liftForce - Physics.gravity.y;
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

        float roll = -horizontal;
        rb.AddTorque(transform.forward * roll * rotationSpeed);
    }

    // SET JALUR DARI X KE Z
    public void SetPath(Vector3 waypointX, Vector3 waypointZ)
    {
        waypoints.Clear();
        waypoints.Add(waypointX); // Titik belok di X
        waypoints.Add(waypointZ); // Titik lanjut di Z
        currentWaypoint = 0;
        isReturning = true;
    }

    void FollowWaypoints()
    {
        if (currentWaypoint >= waypoints.Count)
        {
            isReturning = false;
            return;
        }

        Vector3 target = waypoints[currentWaypoint];
        Vector3 toTarget = target - transform.position;
        toTarget.y = 0; // hanya sumbu XZ

        if (toTarget.magnitude < waypointTolerance)
        {
            currentWaypoint++;
            return;
        }

        if (toTarget.magnitude > 0.1f)
        {
            Quaternion targetRot = Quaternion.LookRotation(toTarget.normalized, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, turnSpeed * Time.deltaTime * 30f);
        }

        Vector3 moveDir = transform.forward;
        rb.AddForce(moveDir * moveSpeed, ForceMode.Acceleration);
    }

    void StabilizeRotation()
    {
        Quaternion desiredRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, stabilization * Time.deltaTime);
    }

    void LandDrone()
    {
        rb.AddForce(Vector3.down * landingSpeed, ForceMode.Acceleration);
        StabilizeRotation();
    }

    public bool IsFlying()
    {
        return isFlying;
    }
}