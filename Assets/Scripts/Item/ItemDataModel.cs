using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 可拾取物数据模型
/// </summary>
public class ItemDataModel :Item
{
    /// <summary>
    /// 经验值
    /// </summary>
    public int exp;
    /// <summary>
    /// 攻击力
    /// </summary>
    public int attack;
    /// <summary>
    /// 增加移动速度比率
    /// </summary>
    public float speedPercent;
    /// <summary>
    /// 效果持续时间
    /// </summary>
    public int timeOf;
    /// <summary>
    /// 增加生命值
    /// </summary>
    public int hp;
    /// <summary>
    /// 增加法力值
    /// </summary>
    public int mp;
    /// <summary>
    /// 减少伤害%
    /// </summary>
    public float munusAttack;
    public ItemDataModel(string itemname, int sellprice, string describe,int exp,int attack,float speedPercent,int timeof,int hp,int mp,float minusAttack) : base(itemname, sellprice, describe)
    {
        this.exp = exp;
        this.attack = attack;
        this.speedPercent = speedPercent;
        this.timeOf = timeof;
        this.hp = hp;
        this.mp = mp;
        this.munusAttack = minusAttack;
    }
}