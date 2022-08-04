using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Enemy返回状态
/// </summary>
public class ReturnState :FSMState
{
    public Vector3 returnPoint;
    public Transform playerTransform;
    public Animator animator;
    public ReturnState(FSMSystem fSMSystem, Transform playerTrs,Vector3 point,Animator anim) : base(fSMSystem)
    {
        stateID = StateID.Return;
        animator = anim;
        returnPoint = point;
        playerTransform = playerTrs;
    }
    public override void Act(GameObject npc)
    {
        npc.GetComponent<NavMeshAgent>().SetDestination(returnPoint);
        if (Vector3.Distance(npc.transform.position, returnPoint) <= 1.1f)
        {
            npc.GetComponent<NavMeshAgent>().ResetPath();
            animator.SetBool(FieldManager.EnemyRun, false);
            animator.SetBool(FieldManager.EnemyIdle, true);
        }
    }

    public override void Reason(GameObject npc)
    {
        if (Vector3.Distance(npc.transform.position, playerTransform.position) <=6)
        {
            fSMSystem.PerformTransition(Transition.SeePlayer);
        }
    }
}