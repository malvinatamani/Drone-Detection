using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 3, -8);
    public float smoothSpeed = 5f;
    public float rotSmoothSpeed = 5f;

    void LateUpdate()
    {
        if (target == null) return;

        // Posisi di belakang drone sesuai rotasinya
        Vector3 desiredPosition = target.position + target.TransformDirection(offset);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Rotasi kamera ikut rotasi drone
        Quaternion desiredRot = Quaternion.LookRotation(target.forward, target.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRot, rotSmoothSpeed * Time.deltaTime);
    }
}