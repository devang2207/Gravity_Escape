using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [Header("InputActionAsset")]
    [SerializeField] InputActionAsset playerController;

    [Header("ActionMapName")]
    [SerializeField] string actionMapName = "RocketMove";

    [Header("Rotate Values")]
    [SerializeField] string leftMove = "LeftMove";
    [SerializeField] string rightMove = "RightMove";

    [Header("ThrustInput Value")]
    [SerializeField] string thrustValue = "Thrust";

    private InputAction leftAction;
    private InputAction rightAction;
    private InputAction thrustAction;

    public bool RotateLeft { get; private set; }
    public bool RotateRight { get; private set; }
    public bool Thrust { get; private set; }

    public static InputHandler Instance { get; private set; }
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        leftAction = playerController.FindActionMap(actionMapName).FindAction(leftMove);
        rightAction = playerController.FindActionMap(actionMapName).FindAction(rightMove);
        thrustAction = playerController.FindActionMap(actionMapName).FindAction(thrustValue);

        RegisterInput();
    }
    void RegisterInput()
    {
        leftAction.performed += context => RotateLeft = true;
        leftAction.canceled += context => RotateLeft = false; 

        rightAction.performed += context => RotateRight = true;
        rightAction.canceled += context => RotateRight = false;

        thrustAction.performed += context => Thrust = true;
        thrustAction.canceled += context => Thrust = false;
    }

    private void OnEnable()
    {
        leftAction.Enable();
        rightAction.Enable();
        thrustAction.Enable();
    }
    private void OnDisable()
    {
        if (leftAction != null)
            leftAction.Disable();

        if (rightAction != null)
            rightAction.Disable();

        if (thrustAction != null)
            thrustAction.Disable();
    }
}
