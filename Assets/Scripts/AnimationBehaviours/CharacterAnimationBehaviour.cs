using UnityEngine;

public class CharacterAnimationBehaviour : StateMachineBehaviour
{
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerMovement pm = animator.GetComponentInParent<PlayerMovement>();

        animator.SetBool("IsWalking", pm.IsMoving);
    }
}
