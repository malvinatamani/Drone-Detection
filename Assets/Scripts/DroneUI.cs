using UnityEngine;
using UnityEngine.UI; // pakai Text biasa
// Kalau kamu pakai TextMeshPro, ganti ke:
// using TMPro;
using UnityEngine.SceneManagement;

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

    public void back_to_menu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
