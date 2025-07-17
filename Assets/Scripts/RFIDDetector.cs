using UnityEngine;
using TMPro; // ← untuk TextMeshPro
public class RFIDDetector : MonoBehaviour
{
    public Transform target;            // orang yang akan dideteksi
    public float detectionRadius = 10f; // radius sinyal RFID
    public TMP_Text detectionText;         // UI teks yang akan dimunculkan
    private bool hasDetected = false;

    void Update()
    {
        if (target == null || detectionText == null)
            return;

        float distance = Vector3.Distance(transform.position, target.position);

        if (distance <= detectionRadius && !hasDetected)
        {
            Debug.Log("🎯 RFID Detected!");

            detectionText.enabled = true;
            hasDetected = true;

            // Mulai hilangkan tulisan setelah 3 detik
            Invoke("HideDetectionText", 3f);
        }
    }
    void HideDetectionText()
    {
        detectionText.enabled = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
