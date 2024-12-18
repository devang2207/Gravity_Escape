using UnityEngine;

public class ObstacleMove : MonoBehaviour
{
    Vector3 StartLocation;
    [SerializeField] Vector3 movementVector;
    [SerializeField] [Range(0,1)]float movementFactor;
    [SerializeField] float period;
    
    private void Awake()
    {
        StartLocation = transform.position;
    }
    private void Update()
    {
        //SUS CODE SUS CODE 
        if(period <= Mathf.Epsilon) { return; }//to remove Nan error(Not a number)


        float cycles = Time.time / period;

        const float tau = Mathf.PI * 2;
        float rawSinWave = Mathf.Sin(cycles * tau);

        movementFactor = (rawSinWave + 1f) / 2f;
        //SUS CODE SUS CODE 

        Vector3 Offset = movementVector * movementFactor;
        transform.position = StartLocation + Offset;
    }
}
