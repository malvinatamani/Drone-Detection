using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 2, -6); // Coba Y = 2, Z = -6
    public float smoothSpeed = 7f;
    public float rotSmoothSpeed = 7f;

    void LateUpdate()
    {
        if (target == null) return;

        // Posisi kamera mengikuti drone
        Vector3 desiredPosition = target.position + target.TransformDirection(offset);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Kamera selalu menghadap ke drone (horizontal)
        Quaternion desiredRot = Quaternion.LookRotation(target.position - transform.position, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRot, rotSmoothSpeed * Time.deltaTime);
    }
}