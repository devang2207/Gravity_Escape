using UnityEngine;

public class TextureScrolling : MonoBehaviour
{
    [SerializeField] bool goingUP = true;
    public float scrollSpeed = 0.5f; // Speed of scrolling
    private Renderer _renderer;

    void Start()
    {
        // Cache the Renderer component to minimize GetComponent calls
        _renderer = GetComponent<Renderer>();
    }

    void FixedUpdate()
    {
        if (goingUP)
        {
            float offsetY = Time.time * -scrollSpeed;
            // Directly apply the offset to the material
            _renderer.material.mainTextureOffset = new Vector2(0, offsetY);
        }
        else if (!goingUP)
        {
            float offsetY = Time.time * scrollSpeed;
            // Directly apply the offset to the material
            _renderer.material.mainTextureOffset = new Vector2(0, offsetY);
        }
        
    }
}
