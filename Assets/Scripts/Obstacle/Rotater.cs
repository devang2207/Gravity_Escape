using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotater : MonoBehaviour
{
    [SerializeField]
    Vector3 RotatingVector;

    // FixedUpdate is called once per frame according to deltatime.
    private void FixedUpdate()
    {
        transform.Rotate(RotatingVector);

    }
}
