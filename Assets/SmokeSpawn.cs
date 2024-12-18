using UnityEngine;

public class SmokeSpawn : MonoBehaviour
{
    [SerializeField] ParticleSystem[] particleObjects;
    [SerializeField] Transform ObjectLocation;

    // Maximum distance at which the particle system is fully active
    public float maxDistance = 5f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the distance between the rocket and the landing position
        float distance = Vector3.Distance(transform.position, ObjectLocation.position);

        // Calculate the normalized distance ratio
        float distanceRatio = 1f - Mathf.Clamp01(distance / maxDistance);

        // Adjust particle system properties based on distance
        foreach (ParticleSystem particleSystem in particleObjects)
        {
            var mainModule = particleSystem.main;
            mainModule.startSizeMultiplier *= distanceRatio; // Example: Adjust particle size
            mainModule.startSpeedMultiplier *= distanceRatio; // Example: Adjust particle speed
            // You can adjust other properties like emission rate, color, etc. based on your requirements
        }
    }
}
