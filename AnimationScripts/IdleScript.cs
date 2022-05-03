using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleScript : StateMachineBehaviour
{
    public int nexatk;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(AttackCecile.instance.delay && AttackCecile.instance.attack == 1)
        {
            Debug.Log(AttackCecile.instance.natk_name+nexatk.ToString());
            AttackCecile.instance.anim.Play(AttackCecile.instance.natk_name+nexatk.ToString());
        }
        if(AttackCecile.instance.delay && AttackCecile.instance.attack == 2)
        {
            AttackCecile.instance.anim.Play("Windup");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AttackCecile.instance.delay = false;
        //AttackCecile.instance.Mobility();
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
