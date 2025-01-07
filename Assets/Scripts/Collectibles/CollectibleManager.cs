using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    public static CollectibleManager instance;
    [SerializeField] private TMPro.TextMeshProUGUI gemCountTMP;

    private int totalGemCount;
    private Dictionary<int, int> levelGemCounts = new Dictionary<int, int>();

    [SerializeField] private List<StarManager> starManagers = new List<StarManager>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitializeLevelGemCounts();
        UpdateTotalGemCount();
        UpdateUI();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0) // Assuming the UI scene is build index 0
        {
            UpdateUI();
        }
    }

    private void InitializeLevelGemCounts()
    {
        int levelCount = SceneManager.sceneCountInBuildSettings - 1;
        for (int i = 1; i <= levelCount; i++) // Levels start at buildIndex 1
        {
            int gemCount = PlayerPrefs.GetInt($"Level_{i}_GemCount", 0);
            levelGemCounts[i] = gemCount;
        }
    }

    private void UpdateTotalGemCount()
    {
        totalGemCount = 0;
        foreach (var count in levelGemCounts.Values)
        {
            totalGemCount += count;
        }
    }

    private void UpdateUI()
    {
        if (gemCountTMP != null)
        {
            gemCountTMP.text = totalGemCount.ToString();
        }else { Debug.LogWarning("Gem Count is Null"); }

        for (int i = 0; i < starManagers.Count; i++)
        {
            int gemCount = GetLevelGemCount(i + 1); // Level indices start from 1
            starManagers[i].UpdateStarColors(gemCount);
        }
    }

    public void UpdateGemCount(int gemCount, int levelIndex)
    {
        PlayerPrefs.SetInt($"Level_{levelIndex}_GemCount", gemCount);
        levelGemCounts[levelIndex] = gemCount; // Update dictionary
        UpdateTotalGemCount();
    }

    public int GetLevelGemCount(int levelIndex)
    {
        return levelGemCounts.ContainsKey(levelIndex) ? levelGemCounts[levelIndex] : 0;
    }
}
