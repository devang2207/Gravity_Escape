using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelUI : MonoBehaviour
{
    [SerializeField] GameObject PauseStuff;
    bool isPaused = false;
    public void Pause()
    {
        if (isPaused)
        {
            Time.timeScale = 1f;
            PauseStuff.SetActive(false);
        }
        else if(!isPaused)
        {
            Time.timeScale = 0f;
            PauseStuff.SetActive(true);
        }

        isPaused = !isPaused;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        PauseStuff.SetActive(false);
    }
    public void Home()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("UI");
    }

}
