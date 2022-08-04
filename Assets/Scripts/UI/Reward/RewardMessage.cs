using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 等级奖励数据信息类
/// </summary>
public class RewardMessage : MonoBehaviour,IController
{
    public Button rewardBtn;

    public Text levelText;

    public Text coinText;

    public int level;

    public int coin;

    public bool isGet;
    public void Init(RewardDataModel model)
    {
        coin = model.coin;
        level = model.level;
        isGet = model.isGet;
        if (!isGet)
        {
            ShowBtn();
        }
        levelText.text = level.ToString();
        coinText.text = coin.ToString();
    }
    /// <summary>
    /// 打开Btn
    /// </summary>
    public void ShowBtn()
    {
        isGet = false;
        rewardBtn.interactable = true;
        this.SendCommand(new UpdateRewardMessageCommand(level, this));
    }
    /// <summary>
    /// 按下领取奖励Btn
    /// </summary>
    public void GetButtonDown()
    {
        AudioManager.instance.SetAudioToSpecial(SpecialAudio.ClickButton);
        this.SendCommand(new UpdatePlayerDataCommand(new PlayerData(0, 0, 0, 0, coin, 0)));
        isGet = true;
        rewardBtn.interactable = false;
        this.SendCommand(new UpdateRewardMessageCommand(level, this));
    }
    public IArchitecture GetArchitecture()
    {
        return GameFramework.Interface;
    }
}
