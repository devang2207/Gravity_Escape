using UnityEngine;
using UnityEngine.EventSystems;

public class RocketThrustButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private GameObject target;

    // Called when the button is pressed down
    public void OnPointerDown(PointerEventData eventData)
    {
        if (target != null)
        {
            ParticleSystem ps = target.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Play(); // Play the particle system
            }
            else
            {
                Debug.LogWarning("No ParticleSystem found on the target!");
            }
        }
        else
        {
            Debug.LogWarning("Target is not assigned!");
        }
    }

    // Called when the button is released
    public void OnPointerUp(PointerEventData eventData)
    {
        if (target != null)
        {
            ParticleSystem ps = target.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Stop(); // Stop the particle system
            }
            else
            {
                Debug.LogWarning("No ParticleSystem found on the target!");
            }
        }
        else
        {
            Debug.LogWarning("Target is not assigned!");
        }
    }
}
