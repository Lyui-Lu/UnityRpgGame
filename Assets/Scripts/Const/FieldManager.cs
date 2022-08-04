using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldManager : MonoBehaviour
{

    #region Player动画字段
    public const string IsMove = "isMove";
     public const string IsAttack = "isAttack";
    public const string IsIdle = "isIdle";
    public const string IsDie = "isDie";
    public const string IsSkill = "isSkill";
    #endregion

    #region Data数据字段
    /// <summary>
    /// 玩家名称类字段
    /// </summary>
    public const string Playername = "Name";
    /// <summary>
    /// 玩家信息类字段
    /// </summary>
    public const string PlayerData = "PlayerData";
    /// <summary>
    /// 玩家最大信息类字段
    /// </summary>
    public const string MaxPlayerData = "MaxPlayerData";
    /// <summary>
    /// 背包信息字段
    /// </summary>
    public const string BackGroundData = "BackGroundData";
    /// <summary>
    /// 敌人信息字段
    /// </summary>
    public const string EnemyData = "EnemyData";
    /// <summary>
    /// 等级奖励信息字段
    /// </summary>
    public const string RewardData = "RewardData";
    /// <summary>
    /// 声音信息字段
    /// </summary>
    public const string AudioData = "AudioData";
    /// <summary>
    /// 任务信息字段
    /// </summary>
    public const string TaskData = "TaskData";
    #endregion

    #region Item数据
    #region 火力蘑
    public const string FireName = "火力蘑";
    public const int FireAttack = 20;
    public const int FireTimeOf = 30;
     public const int FireSellCoin = 15;
    public const string FireDescribe = "提高20点攻击力30秒";
    #endregion
    #region 香薰草
    public const string XiangName = "香薰草";
    public const float XiangSpeedPercent =0.2f;
    public const int XiangTimeOf = 20;
    public const int XiangSellCoin = 10;
    public const string XiangDescribe = "提高20%移速20秒";
    #endregion
    #region 魔力蘑
    public const string MagicName = "魔力蘑";
    public const int MagicExp = 20;
    public const int MagicSellCoin = 10;
    public const string MagicDescribe = "增加经验值";
    #endregion
    #region 消耗品-血瓶
    public const string HpName = "血瓶";
    public const int HpAddHp = 45;
    public const string HpDescribe = "购买之后在背包中使用，立刻回复45点生命值";
    public const int HpBuyCoin = 80;
    public const int HpSellCoin = 40;
    #endregion
    #region 消耗品-蓝瓶
    public const string MpName = "蓝瓶";
    public const int MpAddMp = 30;
    public const string MpDescribe = "购买之后在背包中使用，立刻回复30点法力值";
    public const int MpBuyCoin = 80;
    public const int MpSellCoin = 45;
    #endregion
    #region 消耗品-大力肉
    public const string MeatName = "大力肉";
    public const float MeatMinusAttack = 0.5f;
    public const int MeatTimeOf = 30;
    public const string MeatDescribe = "购买之后在背包中使用，在30秒内免疫百分之50伤害";
    public const int MeatBuyCoin = 200;
    public const int MeatSellCoin = 80;
    #endregion
    #endregion

    #region Scene字段
    public const string LoginScene = "01-Login";
    public const string MainScene = "02-Main";
    #endregion

    #region 游戏内数据字段
    public const string PlayerName = "RPGHero";
    #endregion

    #region ResourcesLoad路径
    public const string FireItem = "Item/火力蘑Item";
    public const string XiangXunCaoItem = "Item/香薰草Item";
    public const string MagicItem = "Item/魔力蘑Item";
    public const string HpItem = "Item/血瓶Item";
    public const string MpItem = "Item/蓝瓶Item";
    public const string MeatItem = "Item/大力丸Item";
    public const string RewardUI = "UI/RewardBG";
    public const string TaskUI = "UI/TaskBG";
    public const string RemoteAttack = "Effects/MagicFire";
    #endregion

    #region EnemyAnimator
    public const string EnemyIdle = "IsIdle";
    public const string EnemyRun = "IsRun";
    public const string EnemyAttack = "IsAttack";
    public const string EnemyDie = "IsDie";
    public const string EnemyVictory = "IsVictory";

    #endregion

    #region Tag
    public const string EnemyTag = "Enemy";
    public const string LandTag = "Land";
    public const string NPCTag = "NPC";
    public const string PickUpItemTag = "PickUpItem";
    #endregion

    #region 等级奖励初始数据
    public const int RewardNumber = 5;

    public static readonly  List<int> RewardCoin = new List<int>(){30,50,100,200,500};
    public static readonly List<int> RewardLevel = new List<int>() { 2, 5, 8, 15, 30};

    #endregion


    #region 任务信息数据
    public static readonly List<int> taskLevel=new List<int> () { 2,5,15,30};
    public static readonly List<int> taskNearEnemy=new List<int> () { 2,5,8,10};
    public static readonly List<int> taskRemoteEnemy=new List<int> () { 2,5,8,10};
    public static readonly List<int> taskMoliMo=new List<int> () { 2,5,8,10};
    public static readonly List<int> taskHuoliMo=new List<int> () { 2,5,8,10};
    public static readonly List<int> taskXiangXunCao=new List<int> () { 2,5,8,10};
    public static readonly List<int> taskAward=new List<int> () { 11,45,141,919};
    public static readonly List<string> taksDescribe = new List<string>() { "低级任务：丶", "初级任务:冫", "中级任务:冫丨", "高级任务:冲" };
    #endregion
}
