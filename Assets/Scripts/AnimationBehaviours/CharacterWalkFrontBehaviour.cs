using UnityEngine;

public class CharacterWalkFrontBehaviour : StateMachineBehaviour
{
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector2 currentVelocity = animator.gameObject.GetComponentInParent<Rigidbody2D>().linearVelocity;

        if (currentVelocity.y >= 0)
        {
            animator.SetBool("IsWalkingFront", false);
        }
    }
}
