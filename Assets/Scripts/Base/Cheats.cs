using UnityEngine;
using UnityEngine.SceneManagement;

public class Cheats : MonoBehaviour
{
#pragma warning disable
    BoxCollider  Rocketcollider;
    bool collisionEnable = true;
    [SerializeField] Transform GoToPos;
    private void Awake()
    {
        Rocketcollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        GoTo();
        LoadNextLevel();
    }
    
    void GoTo()
    {
        //Rocketcollider.enabled = collisionEnable;
        if (Input.GetKeyDown(KeyCode.C)) 
        {
            transform.position = GoToPos.position;
           // collisionEnable = !collisionEnable;
        }
    }
    void LoadNextLevel()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            
            int currentScene = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentScene+1);
        }
    }
}