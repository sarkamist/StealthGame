using UnityEngine;

public class CharacterIdleBehaviour : StateMachineBehaviour
{
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector2 currentVelocity = animator.gameObject.GetComponentInParent<Rigidbody2D>().linearVelocity;

        if (currentVelocity.x != 0)
        {
            animator.SetBool("IsWalkingSide", true);
        }
        else if (currentVelocity.y < 0)
        {
            animator.SetBool("IsWalkingFront", true);
        }
        else if (currentVelocity.y > 0)
        {
            animator.SetBool("IsWalkingBack", true);
        }
    }
}
