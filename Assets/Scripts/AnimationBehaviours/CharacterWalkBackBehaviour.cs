using UnityEngine;

public class CharacterWalkBackBehaviour : StateMachineBehaviour
{
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector2 currentVelocity = animator.gameObject.GetComponentInParent<Rigidbody2D>().linearVelocity;

        if (currentVelocity.x != 0)
        {
            animator.SetBool("IsWalkingSide", true);
            animator.SetBool("IsWalkingBack", false);
        }
        else if (currentVelocity.y <= 0)
        {
            animator.SetBool("IsWalkingBack", false);
        }
    }
}
