using UnityEngine;
using UnityEngine.UI;

public class ImageEffect : MonoBehaviour
{
    [SerializeField] float oscillationSpeed = 2f;
    Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the alpha value using a sine wave
        float alpha = Mathf.Sin(Time.time * oscillationSpeed);

        // Update the image color with the calculated alpha value
        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }
}
