using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{

    [SerializeField] ParticleSystem CrazyParticles;
    [SerializeField] Button[] buttons;

    private void Awake()
    {
        int unlockedLevels = PlayerPrefs.GetInt("UnlockedLevel",1);
        for(int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }
        for (int i = 0; i < unlockedLevels; i++)
        {
            buttons[i].interactable = true;
        }

    }
    public void PlayGame()
    {
        SceneManager.LoadScene(1); 
    }
    public void EndGame()
    {
        Debug.Log("Quit");
        Application.Quit();

    }
    public void OpenSettings()
    {

    }
    public void InstantiateParticleSys()
    {
        Vector3 currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Instantiate(CrazyParticles, currentMousePosition,Quaternion.identity);
        //Instantiate(CrazyParticles,);
    }
    public void OpenLevel(int levelId)
    {
        //QualitySettings.vSyncCount = 0; // Disable vSync
        //Application.targetFrameRate = 60; // Set target frame rate to 60 FPS
        SceneManager.LoadScene("Level " + levelId);
    }
}