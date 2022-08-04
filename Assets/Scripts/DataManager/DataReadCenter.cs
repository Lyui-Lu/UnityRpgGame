using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
/// <summary>
/// 数据加载中心
/// </summary>
public class DataReadCenter : MonoBehaviour, IController
{
    public IArchitecture GetArchitecture()
    {
        return GameFramework.Interface;
    }

    private void Start()
    {
        InitPlayerData();
        InitBackGroundData();
        InitEnemyData();
        InitRewardData();
        InitTaskData();
    }
    /// <summary>
    /// 初始化 用户信息数据
    /// </summary>
    void InitPlayerData()
    {
        if (PlayDataManager.instance.LoadData(FieldManager.PlayerData) == null)
        {
            PlayerData data = new PlayerData();
            PlayDataManager.instance.SaveData(data, FieldManager.PlayerData);
        }
        if (PlayDataManager.instance.LoadData(FieldManager.MaxPlayerData) == null)
        {
            MaxPlayerData data = new MaxPlayerData();
            PlayDataManager.instance.SaveData(data, FieldManager.MaxPlayerData);
        }
        //初始化用户信息数据
        this.SendCommand(new InitPlayerDataCommand(
           (PlayerData)PlayDataManager.instance.LoadData(FieldManager.PlayerData),
           (MaxPlayerData)PlayDataManager.instance.LoadData(FieldManager.MaxPlayerData),
           (LoginData)PlayDataManager.instance.LoadData(FieldManager.Playername)
           ));
    }
    /// <summary>
    /// 初始化背包数据
    /// </summary>
    void InitBackGroundData()
    {
        if (PlayDataManager.instance.LoadData(FieldManager.BackGroundData) == null)
        {
            BackGroundDataModel backGroundDataModel = new BackGroundDataModel();
            PlayDataManager.instance.SaveData(backGroundDataModel, FieldManager.BackGroundData);
        }
        this.SendCommand(new InitBackGroundCommand((BackGroundDataModel)PlayDataManager.instance.LoadData(FieldManager.BackGroundData)));
    }
    /// <summary>
    /// 初始化敌人数据
    /// </summary>
    void InitEnemyData()
    {
        if (PlayDataManager.instance.LoadData(FieldManager.EnemyData) == null)
        {
            EnemyData enemyData = new EnemyData();
            PlayDataManager.instance.SaveData(enemyData, FieldManager.EnemyData);
        }
        EnemyDataManager.instance.InitEnemyData((EnemyData)PlayDataManager.instance.LoadData(FieldManager.EnemyData));
    }
    /// <summary>
    /// 初始化敌人数据
    /// </summary>
    void InitRewardData()
    {
        if (PlayDataManager.instance.LoadData(FieldManager.RewardData) == null)
        {
            RewardData rewardData = new RewardData();
            PlayDataManager.instance.SaveData(rewardData, FieldManager.RewardData);
        }
        this.SendCommand(new InitRewardDataModelsCommand
            ((RewardData)PlayDataManager.instance.LoadData(FieldManager.RewardData)));
    }

    void InitTaskData()
    {
        if (PlayDataManager.instance.LoadData(FieldManager.TaskData) == null)
        {
            TaskData taskdata = new TaskData();
            PlayDataManager.instance.SaveData(taskdata, FieldManager.TaskData);
        }
        this.SendCommand(new InitTaskManagerCommand
           ((TaskData)PlayDataManager.instance.LoadData(FieldManager.TaskData)));
    }
}