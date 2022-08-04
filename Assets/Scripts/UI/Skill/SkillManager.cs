using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 技能管理类
/// </summary>
public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;
    /// <summary>
    /// 技能列表
    /// </summary>
     List<ISkill> skillsList = new List<ISkill>();
    /// <summary>
    /// 层级对技能
    /// </summary>
     Dictionary<int, ISkill> skillsKeyDic = new Dictionary<int, ISkill>();
    /// <summary>
    /// 最大技能数量
    /// </summary>
    int skillLimit = 2;



    void Awake()
    {
        instance = this;
    }
    void Update()
    {
        SkillRelease();
    }
    /// <summary>
    /// 添加技能
    /// </summary>
    /// <param name="skillGo"></param>
    public void AddSkill(GameObject skillGo)
    {
        if (skillsList.Count <= 2)
        {
            skillsList.Add(skillGo.GetComponent<ISkill>());
            skillsKeyDic.Add(skillGo.transform.GetSiblingIndex(), skillGo.GetComponent<ISkill>());
        }
        else
        {
            //todo ui显示超额
        }
    }
    /// <summary>
    /// 更新技能列表
    /// </summary>
    /// <param name="key">Layer</param>
    /// <param name="skill"></param>
    public void UpdateSkill(int key, ISkill skill)
    {
        if (skillsKeyDic.ContainsKey(key))
        {
            skillsKeyDic[key] = skill;
        }

    }
    /// <summary>
    /// 技能释放控制
    /// </summary>
    void SkillRelease()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            skillsKeyDic[0].Release();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            skillsKeyDic[1].Release();
        }
    }

}
