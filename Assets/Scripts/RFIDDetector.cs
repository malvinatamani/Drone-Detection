using UnityEngine;

public class RFIDDetector : MonoBehaviour
{
    public Transform target;            // orang yang akan dideteksi
    public float detectionRadius = 10f; // radius sinyal RFID

    void Update()
    {
        float distance = Vector3.Distance(transform.position, target.position);

        if (distance <= detectionRadius)
        {
            Debug.Log("🎯 RFID Detected!");
        }
    }

    // Visual radius di Scene
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
