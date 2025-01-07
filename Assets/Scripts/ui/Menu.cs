using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class Menu : MonoBehaviour
{
    [SerializeField] Button[] buttons;
    [SerializeField] bool unlockAllLevels = false;

    private void Awake()
    {
        //PlayerPrefs.DeleteAll();
        // Unlocks all levels according to scene buildIndex
        if (unlockAllLevels)
        {
            PlayerPrefs.SetInt("UnlockedLevel", SceneManager.sceneCountInBuildSettings-1);
        }

        int unlockedLevels = PlayerPrefs.GetInt("UnlockedLevel", 1);
        foreach (Button button in buttons)
        {
            button.interactable = false;
        }
        for (int i = 0; i < unlockedLevels; i++)
        {
            buttons[i].interactable = true;
        }

    }

    public void EndGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void OpenLevel(int levelId)
    {
        QualitySettings.vSyncCount = 0; // Disable vSync
        Application.targetFrameRate = 60; // Set target frame rate to 60 FPS
        SceneManager.LoadScene("Level " + levelId);
    }
}
