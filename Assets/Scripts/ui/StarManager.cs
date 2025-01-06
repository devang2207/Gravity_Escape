using UnityEngine;

[System.Serializable]
public class StarManager
{
    public GameObject levelStarsContainer; // Parent object holding the stars
    public SpriteRenderer[] stars; // Array of star renderers for this level

    public void UpdateStarColors(int gemCount)
    {
        Color achievedColor = Color.yellow;
        Color defaultColor = Color.gray;

        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].color = i < gemCount ? achievedColor : defaultColor;
        }
    }
}
