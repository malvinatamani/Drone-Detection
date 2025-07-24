using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public GameObject instructionPanel;
    public GameObject creditPanel;

    public void PlayGame()
    {
        // Ganti "NamaSceneGame" dengan nama scene gameplay kamu
        SceneManager.LoadScene("GamePlay");
    }

    public void ShowInstruction()
    {
        if (instructionPanel != null)
            instructionPanel.SetActive(true);
    }

    public void HideInstruction()
    {
        if (instructionPanel != null)
            instructionPanel.SetActive(false);
    }

    public void ShowCredit()
    {
        if (creditPanel != null)
            creditPanel.SetActive(true);
    }

    public void HideCredit()
    {
        if (creditPanel != null)
            creditPanel.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}