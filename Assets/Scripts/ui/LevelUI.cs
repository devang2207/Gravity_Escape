using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    [SerializeField] GameObject PauseStuff;

    [Header("Buttons")]
    [SerializeField] Button pauseButton;
    [SerializeField] Button resumeButton;
    [SerializeField] Button menuButton;
    [SerializeField] Button retryButton;
    [SerializeField] Button retryButton2;
    [SerializeField] Button nextLevelButton;
    private void Start()
    {
        BindButtonsToFunctions();
        TurnStuffOnOff();
    }
    void TurnStuffOnOff()
    {
        PauseStuff.SetActive(false);
    }
    void BindButtonsToFunctions()
    {
        pauseButton.onClick.AddListener(Pause);
        resumeButton.onClick.AddListener(Resume);
        menuButton.onClick.AddListener(Home);

        retryButton.onClick.AddListener(RetryLevel);
        nextLevelButton.onClick.AddListener(LevelChange);
    }
    public void Pause()
    {
        Time.timeScale = 0f;
        PauseStuff.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        PauseStuff.SetActive(false);
    }
    public void Home()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main_Menu");
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
    public void RetryLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void PlayClickSound()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonSFX();
        }
    }
    internal void LevelChange()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        int maxUnlockedLevels = PlayerPrefs.GetInt("UnlockedLevel");
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings && nextSceneIndex > maxUnlockedLevels)
        {
            PlayerPrefs.SetInt("UnlockedLevel", nextSceneIndex);
        }
        // Check if the next scene index is within the valid range
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            // Load the next scene if it exists
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            // If no next scene exists (it's the last scene), reload the current scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    public void PlayTransitionSFX()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.transitionClip);
        }
    }
}
