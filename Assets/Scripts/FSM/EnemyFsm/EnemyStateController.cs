using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateController : MonoBehaviour
{
    public GameObject pointGo;
    public Animator animator;
    public IEnemy enemy;

    FSMSystem fsm;
    
    
     void Start()
    {
        InitFSM();
    }
    private void Update()
    {
        fsm.Update(this.gameObject);
    }
    void InitFSM()
    {
        fsm = new FSMSystem();
        FSMState returnState = new ReturnState(fsm,enemy.player.transform,pointGo.transform.position,animator);
        returnState.AddTransition(Transition.SeePlayer, StateID.Chase);

        FSMState chaseState = new ChaseState(fsm,enemy.player.transform,animator,enemy.attackScope);
        chaseState.AddTransition(Transition.LostPlayer, StateID.Return);
        chaseState.AddTransition(Transition.NearPlayer, StateID.Attack);
        chaseState.AddTransition(Transition.Victory, StateID.Victory);

        FSMState attackState = new AttackState(fsm,enemy.player.transform, animator,enemy.attackScope);
        attackState.AddTransition(Transition.SeePlayer, StateID.Chase);
        attackState.AddTransition(Transition.LostPlayer, StateID.Return);
        attackState.AddTransition(Transition.Victory, StateID.Victory);

        FSMState vcitoryState = new VictoryState(fsm,animator);

        fsm.AddState(returnState);
        fsm.AddState(chaseState);
        fsm.AddState(attackState);
        fsm.AddState(vcitoryState);
    }
    public void SetNormalState()
    {
        fsm.PerformTransition(Transition.LostPlayer);
        this.enabled = true;
    }
    
}
