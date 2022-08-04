using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 等级奖励管理类
/// </summary>
public class RewardManager : MonoBehaviour,IController
{
    /// <summary>
    /// PanelF
    /// </summary>
    public GameObject rewardF;
    /// <summary>
    /// 等级奖励UIF
    /// </summary>
    public GameObject rewardUIF;

    public Button exitBtn;

    public Button enterBtn;
   

    RewardManagerModel model;
    void Awake()
    {
        //接收升级事件
        this.RegisterEvent<LevelUpEvent>(UpdateLevelReward).UnRegisterWhenGameObjectDestroyed(this.gameObject);
        //接收初始化UI事件
        this.RegisterEvent<InitCreateRewardUIEvent>(CreateRewardUI).UnRegisterWhenGameObjectDestroyed(this.gameObject);

    }
    void Start()
    {
        model = this.GetModel<RewardManagerModel>();
        exitBtn.onClick.AddListener(ExitBtnDown);
        enterBtn.onClick.AddListener(EnterBtnDown);
    }
    void ExitBtnDown()
    {
         AudioManager.instance.SetAudioToSpecial(SpecialAudio.ClickButton);
        Time.timeScale = 1f;
        rewardF.SetActive(false);
    }
    void EnterBtnDown()
    {
        AudioManager.instance.SetAudioToSpecial(SpecialAudio.ClickButton);
        Time.timeScale = 0f;
        rewardF.SetActive(true);
    }
    /// <summary>
    /// 初始化生成等级奖励UI
    /// </summary>
    /// <param name="e"></param>
    void CreateRewardUI(InitCreateRewardUIEvent e)
    {
        for (int i = 0; i < model.rewardDataModels.Count; i++)
        {
            GameObject go = Instantiate(Resources.Load<GameObject>(FieldManager.RewardUI), rewardUIF.transform);
            go.GetComponent<RewardMessage>().Init(model.rewardDataModels[i]);
            this.SendCommand(new InitRewardMessageCommand(model.rewardDataModels[i].level, go.GetComponent<RewardMessage>()));
        }
    }
    /// <summary>
    /// 更新领取奖励按钮
    /// </summary>
    /// <param name="c"></param>
    void UpdateLevelReward(LevelUpEvent e)
    {
        if (model.rewardMessageDic.ContainsKey(e.level))
        {
            model.rewardMessageDic[e.level].ShowBtn();
        }
    }
    private void OnApplicationQuit()
    {
        List<RewardDataModel> tempRewardDataModels = new List<RewardDataModel>();
        foreach (KeyValuePair<int,RewardMessage> item in model.rewardMessageDic)
        {
            tempRewardDataModels.Add(new RewardDataModel (item.Value.level,item.Value.coin,item.Value.isGet));
        }
        RewardData rewardData = new RewardData(tempRewardDataModels);
        PlayDataManager.instance.SaveData(rewardData, FieldManager.RewardData);
    }
    public IArchitecture GetArchitecture()
    {
        return GameFramework.Interface;
    }
}
public class InitRewardDataModelsCommand : AbstractCommand
{
   List<RewardDataModel> rewardDataModels = new List<RewardDataModel>();
    public InitRewardDataModelsCommand(RewardData rewardData)
    {
        this.rewardDataModels = rewardData.rewardDataModels;
    }
    protected override void OnExecute()
    {
        this.GetModel<RewardManagerModel>().InitRewardDataModels(rewardDataModels);
    }
}
/// <summary>
/// 初始化等级奖励UI类信息字典Command
/// </summary>
public class InitRewardMessageCommand : AbstractCommand
{
    int level;
    RewardMessage rewardMessage;
    public InitRewardMessageCommand(int level,RewardMessage rewardMessage)
    {
        this.level = level;
        this.rewardMessage = rewardMessage;
    }
    protected override void OnExecute()
    {
        this.GetModel<RewardManagerModel>().InitRewardMessageDic(level,rewardMessage);
    }
}
///更新等级奖励UI类信息字典Command
public class UpdateRewardMessageCommand : AbstractCommand
{
    int level;
    RewardMessage rewardMessage;
    public UpdateRewardMessageCommand(int level, RewardMessage rewardMessage)
    {
        this.level = level;
        this.rewardMessage = rewardMessage;
    }
    protected override void OnExecute()
    {
        this.GetModel<RewardManagerModel>().UpdateRewardData(level, rewardMessage);
    }
}
public class RewardManagerModel : AbstractModel
{
    /// <summary>
    /// 流中读取的数据(仅做初始化)
    /// </summary>
    public List<RewardDataModel> rewardDataModels = new List<RewardDataModel>();
    /// <summary>
    /// 所有Reward的Button数据   key:level
    /// </summary>
    public Dictionary<int, RewardMessage> rewardMessageDic = new Dictionary<int, RewardMessage>();
    protected override void OnInit()
    {

    }
    /// <summary>
    /// 初始化等级奖励数据
    /// </summary>
    /// <param name="rewardDataModels"></param>
    public void InitRewardDataModels(List<RewardDataModel> rewardDataModels)
    {
        this.rewardDataModels = rewardDataModels;
        this.SendEvent(new InitCreateRewardUIEvent ());
    }
    /// <summary>
    /// 初始化等级奖励UI类信息字典
    /// </summary>
    /// <param name="level"></param>
    /// <param name="reward"></param>
    public void InitRewardMessageDic(int level,RewardMessage reward)
    {
        rewardMessageDic.Add(level, reward);
    }
    /// <summary>
    /// 更新等级奖励信息;
    /// </summary>
    /// <param name="level"></param>
    /// <param name="reward"></param>
    public void UpdateRewardData(int level,RewardMessage reward)
    {
        if (rewardMessageDic.ContainsKey(level))
        {
            rewardMessageDic[level] = reward;
        }
    }
}
/// <summary>
/// 初始化生成等级奖励UIEvent
/// </summary>
public class InitCreateRewardUIEvent
{

}