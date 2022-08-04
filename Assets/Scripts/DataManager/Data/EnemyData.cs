using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
/// <summary>
/// 敌人信息类
/// </summary>
public class EnemyData:Data
{
    /// <summary>
    /// Key 敌人ID , Value 敌人属性
    /// </summary>
    public Dictionary<int, EnemyDataModel> enemyList = new Dictionary<int, EnemyDataModel>();
    /// <summary>
    /// 敌人ID列表
    /// </summary>
    public List<int> enemyID = new List<int>();

    public EnemyData()
    {

    }
    public EnemyData( Dictionary<int, EnemyDataModel> enemyList,List<int> enemyID)
    {
        this.enemyList = enemyList;
        this.enemyID = enemyID;
    }
}

[Serializable]
/// <summary>
/// 敌人信息模型
/// </summary>
public class EnemyDataModel
{
    public float Hp;
    public float Attack;
    public int Exp;
    public int coin;
    
    public EnemyDataModel()
    {

    }
    public EnemyDataModel(float hp,float attack,int exp,int coin)
    {
        this.Hp = hp;
        this.Attack = attack;
        this.Exp = exp;
        this.coin = coin;
    }
}
