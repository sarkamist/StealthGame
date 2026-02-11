using UnityEngine;

public class BouncerAnimationBehaviour : StateMachineBehaviour
{
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateinfo, int layerindex)
    {
        PatrollingEnemy pe = animator.GetComponentInParent<PatrollingEnemy>();
        if (pe != null)
        {
            animator.SetBool("IsWalking", true);
            return;
        }

        WaitingEnemy we = animator.GetComponentInParent<WaitingEnemy>();
        if (we != null)
        {
            if (we.CurrentState == WaitingEnemyStates.Chase || we.CurrentState == WaitingEnemyStates.Returning)
            {
                animator.SetBool("IsWalking", true);
            }
            else
            {
                animator.SetBool("IsWalking", false);
            }
                
            return;
        }

        RandomEnemy re = animator.GetComponentInParent<RandomEnemy>();
        if (re != null)
        {
            if (
                re.CurrentState == RandomEnemyStates.Roaming
                || re.CurrentState == RandomEnemyStates.Chase
                || re.CurrentState == RandomEnemyStates.Returning
            )
            {
                animator.SetBool("IsWalking", true);
            }
            else
            {
                animator.SetBool("IsWalking", false);
            }

            return;
        }
    }
}
