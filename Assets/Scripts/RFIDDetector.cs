using UnityEngine;
using TMPro;

public class RFIDDetector : MonoBehaviour
{
    [System.Serializable]
    public class RFIDTarget
    {
        public Transform target;
        public TMP_Text detectionText;
        [HideInInspector] public bool hasDetected = false;
    }

    public float detectionRadius = 10f;
    public RFIDTarget[] targets = new RFIDTarget[5];

    void Update()
    {
        foreach (var rfid in targets)
        {
            if (rfid.target == null || rfid.detectionText == null)
                continue;

            float distance = Vector3.Distance(transform.position, rfid.target.position);

            if (distance <= detectionRadius && !rfid.hasDetected)
            {
                Debug.Log("🎯 RFID Detected for " + rfid.target.name);

                rfid.detectionText.enabled = true;
                rfid.hasDetected = true;

                // Mulai hilangkan tulisan setelah 3 detik
                StartCoroutine(HideDetectionTextAfterDelay(rfid, 3f));
            }
        }
    }

    System.Collections.IEnumerator HideDetectionTextAfterDelay(RFIDTarget rfid, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (rfid.detectionText != null)
            rfid.detectionText.enabled = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}