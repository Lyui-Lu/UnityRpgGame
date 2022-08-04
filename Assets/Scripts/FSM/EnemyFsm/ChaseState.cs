using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// Enemy追踪状态
/// </summary>
public class ChaseState : FSMState
{
    public Transform playerTrans;
    public Animator animator;
    float AttackDis;
    public ChaseState (FSMSystem fSMSystem,Transform playerTrs,Animator animator,float attackDistance) : base(fSMSystem)
    {
        stateID = StateID.Chase;
        this.animator = animator;
        AttackDis = attackDistance;
        playerTrans =playerTrs;
    }
    public override void DoBeforeEnter()
    {
        animator.SetBool(FieldManager.EnemyRun, true);
    }
    public override void Act(GameObject npc)
    {
          npc.transform.GetComponent<NavMeshAgent>().SetDestination(playerTrans.position);
    }
    public override void Reason(GameObject npc)
    {
        //距离太远的时候，将转换条件设置为跟丢目标(返回初始点)
        if (Vector3.Distance(playerTrans.position, npc.transform.position) > 15)
        {
            fSMSystem.PerformTransition(Transition.LostPlayer);
        }
        //距离太近，将转换条件设置为攻击
        if (Vector3.Distance(playerTrans.position, npc.transform.position) < AttackDis)
        {
            npc.transform.GetComponent<NavMeshAgent>().ResetPath();
              fSMSystem.PerformTransition(Transition.NearPlayer);
        }
        if (isPlayerDie)
        {
            npc.transform.GetComponent<NavMeshAgent>().ResetPath();
            fSMSystem.PerformTransition(Transition.Victory);
        }
    }
}