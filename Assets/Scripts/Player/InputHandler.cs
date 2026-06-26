using UnityEngine;
using UnityEngine.InputSystem;
public class InputHandler : MonoBehaviour
{
    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset playerController;

    [Header("Action Map Name Refrence")]
    [SerializeField] private string actionMapName = "Default";

    [Header("Action Name Refrence")]
    [SerializeField] private string Move = "Move";
    [SerializeField] private string Jump = "Jump";
    [SerializeField] private string Sprint = "Sprint";
    [SerializeField] private string NormalAttack = "NormalAttack";

    private InputAction movementAction;
    private InputAction jumpAction;
    private InputAction sprintAction;
    private InputAction normalAttackAction;

    public Vector2 MovementInput { get; private set; }

    public bool attackTriggered { get; private set; }

    public bool JumpTriggered { get; private set; }

    public bool SprintTriggered { get; private set; }


    void Awake()
    {
        movementAction = playerController.FindActionMap(actionMapName).FindAction(Move);
        jumpAction = playerController.FindActionMap(actionMapName).FindAction(Jump);
        sprintAction = playerController.FindActionMap(actionMapName).FindAction(Sprint);
        normalAttackAction = playerController.FindActionMap(actionMapName).FindAction(NormalAttack);
        SubscribeActionValuesToInputEvents();
    }

    private void SubscribeActionValuesToInputEvents()
    {
        movementAction.performed += inputInfo => MovementInput = inputInfo.ReadValue<Vector2>();
        movementAction.canceled += inputInfo => MovementInput = Vector2.zero;

        normalAttackAction.performed += inputInfo => attackTriggered = true;

        jumpAction.performed += inputInfo => JumpTriggered = true;
        jumpAction.canceled += inputInfo => JumpTriggered = false;

        sprintAction.performed += inputInfo => SprintTriggered = true;
        sprintAction.canceled += inputInfo => SprintTriggered = false;
    }

    private void OnEnable()
    {
        playerController.FindActionMap(actionMapName).Enable();
    }

    private void OnDisable()
    {
        playerController.FindActionMap(actionMapName).Disable();
    }

    public void ConsumeAttack()
    {
        attackTriggered = false;
    }
}
