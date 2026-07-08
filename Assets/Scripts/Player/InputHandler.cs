using UnityEngine;
using UnityEngine.InputSystem;
public class InputHandler : MonoBehaviour
{
    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset playerController;
    [SerializeField] private GameManeger gameManeger;

    [Header("Action Map Name Refrence")]
    [SerializeField] private string actionMapName = "Default";

    [Header("Action Name Refrence")]
    [SerializeField] private string Move = "Move";
    [SerializeField] private string Jump = "Jump";
    [SerializeField] private string Sprint = "Sprint";
    [SerializeField] private string NormalAttack = "NormalAttack";
    [SerializeField] private string StingAttack = "StingAttack";
    [SerializeField] private string Pause = "PauseGame";

    private InputAction movementAction;
    private InputAction jumpAction;
    private InputAction sprintAction;
    private InputAction normalAttackAction;
    private InputAction stingAttackAction;
    private InputAction pauseAction;
    public Vector2 MovementInput { get; private set; }

    public bool attackTriggered { get; private set; }

    public bool stingAttackTriggered { get; private set; }

    public bool JumpTriggered { get; private set; }

    public bool SprintTriggered { get; private set; }

    public bool PauseTriggered { get; private set; }


    void Awake()
    {
        movementAction = playerController.FindActionMap(actionMapName).FindAction(Move);
        jumpAction = playerController.FindActionMap(actionMapName).FindAction(Jump);
        sprintAction = playerController.FindActionMap(actionMapName).FindAction(Sprint);
        normalAttackAction = playerController.FindActionMap(actionMapName).FindAction(NormalAttack);
        stingAttackAction = playerController.FindActionMap(actionMapName).FindAction(StingAttack);
        pauseAction = playerController.FindActionMap(actionMapName).FindAction(Pause);
        SubscribeActionValuesToInputEvents();
    }

    private void Update()
    {
        if (pauseAction.WasPressedThisFrame())
        {
            if (gameManeger.isGamePaused)
            {
                gameManeger.ResumeGame();
            }
            else
            {
                gameManeger.PauseGame();
            }
        }
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

        stingAttackAction.performed += inputInfo => stingAttackTriggered = true;
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

    public void ConsumeStingAttack()
    {
        stingAttackTriggered = false;
    }

}
