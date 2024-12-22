using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    public static CollectibleManager instance;
    [SerializeField] TMPro.TextMeshProUGUI gemCountTMP;
    int gemCount;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }
    private void Start()
    {
        gemCount = PlayerPrefs.GetInt("GemCount",0);
        gemCountTMP.text = gemCount.ToString();
    }
}
