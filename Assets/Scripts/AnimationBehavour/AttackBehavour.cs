using UnityEngine;

public class AttackBehavour : StateMachineBehaviour
{
    private PlayerController controller;
    private EnemyController enemyController;
    private BossController bossController;
    [SerializeField] private bool isPlayer;
    [SerializeField] private bool isBoss;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (isPlayer)
        {
            if (controller == null)
            {
                controller = animator.GetComponentInParent<PlayerController>();
            }
            controller.isAttacking = true;
        }else if (isBoss)
        {
            if (bossController == null)
            {
                bossController = animator.GetComponentInParent<BossController>();
            }
            bossController.IsAttacking = true;
        }
        else
        {
            if (enemyController == null)
            {
                enemyController = animator.GetComponentInParent<EnemyController>();
            }
            enemyController.IsAttacking = true;
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (isPlayer)
        {
            controller.isAttacking = false;
        }else if (isBoss)
        {
            bossController.IsAttacking = false;
        }
        else
        {
            enemyController.IsAttacking = false;
        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
