using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// Enemy攻击状态
/// </summary>
public class AttackState : FSMState
{
    public Transform playerTransform;
    public Animator animator;
    float attackDis;
    public AttackState (FSMSystem fSMSystem,Transform playerTrs,Animator anim,float attackDistance) : base(fSMSystem)
    {
        stateID = StateID.Attack;
        animator = anim;
        attackDis = attackDistance;
        playerTransform = playerTrs;
    }
    public override void DoBeforeEnter()
    {
        animator.SetBool(FieldManager.EnemyAttack, true);
         animator.SetBool(FieldManager.EnemyRun, false);
    }
    public override void DoAfterLeave()
    {
        animator.SetBool(FieldManager.EnemyAttack, false);
    }
    public override void Act(GameObject npc)
    {

    }
    public override void Reason(GameObject npc)
    {
        //如果大于攻击距离并且满足其一（动画播放完毕或者当前动画不是攻击动画）
        if (Vector3.Distance(npc.transform.position, playerTransform.position) >=
            attackDis&&(animator.GetCurrentAnimatorStateInfo(0).normalizedTime>=1.0F
            ||!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")))
        {
            fSMSystem.PerformTransition(Transition.SeePlayer);
        }
        if (isPlayerDie)
        {
            npc.transform.GetComponent<NavMeshAgent>().ResetPath();
            fSMSystem.PerformTransition(Transition.Victory);
        }
    }
}
