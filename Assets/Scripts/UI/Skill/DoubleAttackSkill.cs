using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;


public class DoubleAttackSkill : ISkill,IController
{
    /// <summary>
    /// 释放技能需要的蓝量
    /// </summary>
    public int expendMP;
    /// <summary>
    /// 蓝量是否充足
    /// </summary>
    bool isMpEnough;
    void Awake()
    {
        this.RegisterEvent<EquipMpEnoughEvent>(IsSkillMpEnough);
    }
    void Start()
    {
        SkillManager.instance.AddSkill(this.gameObject);
    }
    protected override void Update()
    {
        base.Update();
    }
    public override void Release()
    {
        //是否可以执行此方法
        if (isRealse)
        {
            this.SendCommand(new JudgeMpEnough(expendMP));
            if (isMpEnough)
            {
                isRealse = false;
                mask.fillAmount = 1;
                this.SendCommand(new UpdatePlayerDataCommand(new PlayerData(0, 0, 0, -expendMP, 0, 0)));
                this.SendCommand<ReleaseDoubleAtkCommand>();
            }
            else
            {
                HintManager.instance.ShowMpScantry();
            }
        }
    }
    void IsSkillMpEnough(EquipMpEnoughEvent e)
    {
        isMpEnough = e.isEnough;
    }
    /// <summary>
    /// 更改层级
    /// </summary>
    public override void ChangeOrder()
    {
        throw new System.NotImplementedException();
    }

    public IArchitecture GetArchitecture()
    {
        return GameFramework.Interface;
    }
}
/// <summary>
/// 释放双连击Atk
/// </summary>
public class ReleaseDoubleAtkEvent
{

}