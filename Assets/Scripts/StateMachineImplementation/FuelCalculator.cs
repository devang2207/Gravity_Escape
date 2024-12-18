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
        if (InputHandler.Instance.Thrust)
        {
            currentFuel -= BurnRate * Time.deltaTime;
        }
        image.fillAmount = currentFuel / fuelCapacity;
        if(currentFuel <= 0)
        {
            SM_Ref.RocketThrust = 0;
            SM_Ref.flying.DisableVfx_ResetRotationSpeed();
        }
    }
}
