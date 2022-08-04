using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
//等级奖励数据
public class RewardData : Data
{
    /// <summary>
    /// 所有的等级奖励数据模型（从流中读取）
    /// </summary>
    public List<RewardDataModel> rewardDataModels = new List<RewardDataModel>();



    public RewardData()
    {
        for (int i = 1; i <= FieldManager.RewardNumber; i++)
        {
            rewardDataModels.Add(new RewardDataModel(FieldManager.RewardLevel[i-1], 
                FieldManager.RewardCoin[i-1], true));
        }
    }
    public RewardData(List<RewardDataModel> rewardDataModels)
    {
        this.rewardDataModels = rewardDataModels;
    }
}
[Serializable]
/// <summary>
/// 等级奖励数据模型
/// </summary>
public class RewardDataModel
{
    /// <summary>
    /// 可领取等级奖励的等级
    /// </summary>
    public int level;
    /// <summary>
    /// 等级奖励金币
    /// </summary>
    public int coin;
    /// <summary>
    /// 是否已经领取
    /// </summary>
    public bool isGet;

    public RewardDataModel()
    {

    }
    public RewardDataModel (int level,int coin,bool isGet)
    {
        this.level = level;
        this.coin = coin;
        this.isGet = isGet;
    }
}
