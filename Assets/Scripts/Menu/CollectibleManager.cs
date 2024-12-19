using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI gemCountTMP;
    int gemCount;

    private void Start()
    {
        gemCount = PlayerPrefs.GetInt("GemCount",0);
        gemCountTMP.text = gemCount.ToString();
    }
}
