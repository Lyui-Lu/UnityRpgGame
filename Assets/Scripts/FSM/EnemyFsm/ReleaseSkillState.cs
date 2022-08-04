//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
///// <summary>
///// 技能释放状态
///// </summary>
//public class ReleaseSkillState : FSMState
//{
//    public Animator animator;

//    public ReleaseSkillState(FSMSystem fSMSystem,Animator animator): base(fSMSystem)
//    {
//        this.fSMSystem = fSMSystem;
//        this.animator = animator;
//    }
//    public override void Act(GameObject npc)
//    {

//    }
//    public override void Reason(GameObject npc)
//    {
//        if(animator.GetCurrentAnimatorStateInfo (0).IsName("Skill")&&(animator.GetCurrentAnimatorStateInfo (0).normalizedTime >= 1.0f))
//        {
//            fSMSystem.PerformTransition(Transition.LostPlayer);
//        }
//    }
//}