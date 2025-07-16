using UnityEngine;
using UnityEngine.UI; // pakai Text biasa
// Kalau kamu pakai TextMeshPro, ganti ke:
// using TMPro;

public class DroneUI : MonoBehaviour
{
    public Text statusText;
    // Kalau pakai TextMeshPro, ubah menjadi: public TMP_Text statusText;

    public DroneController drone;

    void Update()
    {
        if (drone == null || statusText == null) return;

        if (!drone.IsFlying())
        {
            statusText.text = "Tekan T untuk Terbang";
        }
        else
        {
            statusText.text = "Tekan L untuk Mendarat";
        }
    }
}
