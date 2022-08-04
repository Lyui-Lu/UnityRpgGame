using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryState : FSMState
{
    public Transform playerTransform;
    public Animator animator;
    public VictoryState(FSMSystem fSMSystem,  Animator anim) : base(fSMSystem)
    {
        stateID = StateID.Victory;
        animator = anim;
    }
    public override void DoBeforeEnter()
    {
        animator.SetBool(FieldManager.EnemyVictory, true);
    }
    public override void Act(GameObject npc)
    {
    }

    public override void Reason(GameObject npc)
    {
    }
}
