using UnityEngine;

public class TextureScrolling : MonoBehaviour
{
    public float scrollSpeed = 0.5f; // Speed of scrolling
    private Renderer _renderer;
    private Vector2 offset;

    void Start()
    {
        // Get the Renderer component of the object
        _renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        // Calculate offset based on time and speed
        offset = new Vector2(0, Time.time * scrollSpeed);

        // Apply the offset to the material
        _renderer.material.mainTextureOffset = offset;
    }
}
