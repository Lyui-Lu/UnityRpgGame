using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
/// <summary>
/// 当前任务完成进度数据
/// </summary>
public class TaskMesData
{
    /// <summary>
    /// 任务需要的等级
    /// </summary>
    public int unlockLevel;
    /// <summary>
    /// 是否接取任务  true是接取
    /// </summary>
    public bool isTask;
    /// <summary>
    /// 已经击杀近战敌人的数量
    /// </summary>
    public int nearEnemyNum;
    /// <summary>
    /// 已经击杀远程敌人的数量
    /// </summary>
    public int RemoteEnemyNum;
    /// <summary>
    /// 已经采摘的魔力蘑数量
    /// </summary>
    public int moLimoNum;
    /// <summary>
    /// 已经采摘的火力蘑数量
    /// </summary>
    public int huoLiMoNum;
    /// <summary>
    /// 已经采摘的香薰草的数量
    /// </summary>
    public int xiangXunCaoNum;
    /// <summary>
    /// 是否完成任务 true是完成
    /// </summary>
    public bool isComplete;
    /// <summary>
    /// 是否达到完成此任务的等级
    /// </summary>
    public bool isLevelTo;
    public TaskMesData()
    {

    }
    public TaskMesData(int unlocklevel,bool isTask,bool isComplete,bool isLevelTo,int nearenemynum,int remoteenemynum,int molimonum,int huolinum,int xiangxuncaonum)
    {
        this.unlockLevel = unlocklevel;
        this.isTask = isTask;
        this.nearEnemyNum = nearenemynum;
        this.RemoteEnemyNum = remoteenemynum;
        this.moLimoNum = molimonum;
        this.huoLiMoNum = huolinum;
        this.xiangXunCaoNum = xiangxuncaonum;
        this.isComplete = isComplete;
        this.isLevelTo = isLevelTo;
    }
}
/// <summary>
/// 任务数据模型(不存储在本地，每次运行时生成)
/// </summary>
public class TaskModel 
{
    /// <summary>
    /// 任务需要的等级
    /// </summary>
    public int unlockLevel;
    /// <summary>
    /// 要击杀近战敌人的数量
    /// </summary>
    public int nearEnemyNum;
    /// <summary>
    /// 是否接取了此任务
    /// </summary>
    public bool isTask;
    /// <summary>
    /// 是否完成了此任务
    /// </summary>
    public bool isComplete;
    /// <summary>
    /// 是否达到完成此任务的等级
    /// </summary>
    public bool isLevelTo;
    /// <summary>
    /// 要击杀远程敌人的数量
    /// </summary>
    public int RemoteEnemyNum;
    /// <summary>
    /// 要采摘的魔力蘑数量
    /// </summary>
    public int moLimoNum;
    /// <summary>
    /// 要采摘的火力蘑数量
    /// </summary>
    public int huoLiMoNum;
    /// <summary>
    /// 要采摘的香薰草的数量
    /// </summary>
    public int xiangXunCaoNum;
    /// <summary>
    /// 描述
    /// </summary>
    public string Describe;
    /// <summary>
    /// 奖励
    /// </summary>
    public int Award;
    
    public TaskModel()
    {

    }
    public TaskModel(int unlocklevel, bool isTask, bool isLevelTo,int nearenemynum, int remoteenemynum, int molimonum, int huolinum, int xiangxuncaonum,string Describe,int Award,bool isComplete)
    {
        this.unlockLevel = unlocklevel;
        this.isTask = isTask;
        this.nearEnemyNum = nearenemynum;
        this.RemoteEnemyNum = remoteenemynum;
        this.moLimoNum = molimonum;
        this.huoLiMoNum = huolinum;
        this.xiangXunCaoNum = xiangxuncaonum;
        this.Describe = Describe;
        this.Award = Award;
        this.isComplete = isComplete;
        this.isLevelTo = isLevelTo;
    }
}