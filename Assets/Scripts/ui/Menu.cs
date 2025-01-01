using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class Menu : MonoBehaviour
{
    [SerializeField] Button[] buttons;
    [SerializeField] bool unlockAllLevels = false;

    private Queue<ParticleSystem> particlePool;

    [Header("Particle effect")]
    [SerializeField] ParticleSystem crazyParticles; //  particle prefab
    [SerializeField] int poolSize = 10; // Size of the object pool
    [SerializeField] float effectTime = 1.5f;

    private void Awake()
    {
        // Unlocks all levels according to scene buildIndex
        if (unlockAllLevels)
        {
            PlayerPrefs.DeleteAll();
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

        // Initialize the particle system pool
        particlePool = new Queue<ParticleSystem>();
        for (int i = 0; i < poolSize; i++)
        {
            ParticleSystem particle = Instantiate(crazyParticles);
            particle.gameObject.SetActive(false);
            particlePool.Enqueue(particle);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SpawnParticle();
        }
    }

    public void SpawnParticle()
    {
        if (particlePool.Count > 0)
        {
            // Retrieve a particle system from the pool
            ParticleSystem particle = particlePool.Dequeue();

            // Set its position based on mouse position
            float zDepth = Camera.main.nearClipPlane + 70f; // Adjust this based on your scene
            Vector3 screenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, zDepth);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

            particle.transform.position = worldPosition;
            particle.gameObject.SetActive(true);
            particle.Play();

            // Schedule the particle system to return to the pool after its duration
            StartCoroutine(ReturnParticleToPool(particle));
        }
        else
        {
            Debug.LogWarning("Particle pool exhausted! Consider increasing the pool size.");
        }
    }

    private System.Collections.IEnumerator ReturnParticleToPool(ParticleSystem particle)
    {
        yield return new WaitForSeconds(effectTime);

        // Stop and deactivate the particle system
        particle.Stop();
        particle.gameObject.SetActive(false);

        // Return it to the pool
        particlePool.Enqueue(particle);
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

    public void OpenLevel(int levelId)
    {
        QualitySettings.vSyncCount = 0; // Disable vSync
        Application.targetFrameRate = 60; // Set target frame rate to 60 FPS
        SceneManager.LoadSceneAsync("Level " + levelId);
    }
}
