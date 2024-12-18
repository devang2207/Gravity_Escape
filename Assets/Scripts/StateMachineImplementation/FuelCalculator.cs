using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class FuelCalculator
{
    StateMachine SM_Ref;
    float fuelCapacity = 100;
    float currentFuel;
    [SerializeField]float BurnRate = 20;

    [SerializeField] Image image;
    internal void StartState(StateMachine stateMachine)
    {
        SM_Ref = stateMachine;
        currentFuel = fuelCapacity;
    }
    internal void UpdateState()
    {
        if (InputHandler.Instance == null) { Debug.LogWarning("INPUT Handler Aint there"); return; }
        if (InputHandler.Instance.Thrust)
        {
            currentFuel -= BurnRate * Time.deltaTime;
        }
        if(image != null)
        {
            image.fillAmount = currentFuel / fuelCapacity;
        }
        else { Debug.LogWarning("FUELBAR IMAGE is not there"); }
        if(currentFuel <= 0)
        {
            SM_Ref.RocketThrust = 0;
            SM_Ref.flying.DisableVfx_ResetRotationSpeed();
        }
    }
}
