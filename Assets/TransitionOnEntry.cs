using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionOnEntry : MonoBehaviour
{
    [SerializeField] string parameterName = "DoTransition";
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool(parameterName, true);
        }
        else
        {
            Debug.LogWarning("hell O");
        }
    }
}
