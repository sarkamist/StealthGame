using UnityEngine;

public class CharacterWalkSideBehaviour : StateMachineBehaviour
{
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector2 currentVelocity = animator.gameObject.GetComponentInParent<Rigidbody2D>().linearVelocity;

        if (currentVelocity.x == 0)
        {
            animator.SetBool("IsWalkingSide", false);
        }
    }
}
