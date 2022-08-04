using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskDetails : MonoBehaviour, IController
{
    /// <summary>
    /// 任务面板的爹
    /// </summary>
    public GameObject taskF;

    public GameObject messageF;

    public Button showBtn;

    public Button hideBtn;

    public Button awardBtn;
    public Text taskName;
    public Text nearNumber;
    public Text remoteNumber;
    public Text molimoNumber;
    public Text huolimoNumber;
    public Text xiangxuncaoNumber;

    TaskDetailsModel model;

    void Awake()
    {
        model = this.GetModel<TaskDetailsModel>();
        awardBtn.onClick.AddListener(AwardBtnDown);
        showBtn.onClick.AddListener(ShowBtn);
        hideBtn.onClick.AddListener(HideBtn);
        this.RegisterEvent<SetTaskToDatails>(SetTaskToDatails);
        this.RegisterEvent<UpdateTaskToDatails>(UpdateTaskToDatails);
        this.RegisterEvent<OpenAwardBtnEvent>(ShowAwardBtn);
        this.RegisterEvent<ShowTaskMesEvent>(ShowTaskMesBtn);
        this.RegisterEvent<InitShowTextEvent>(InitShowText);
        this.RegisterEvent<UpdateShowTextEvent>(UpdateShowText);
    }
    void ShowBtn()
    {
        Time.timeScale = 0f;
        taskF.SetActive(true);
        AudioManager.instance.SetAudioToSpecial(SpecialAudio.ClickButton);

    }
    void HideBtn()
    {
        Time.timeScale = 1f;
        taskF.SetActive(false);
        AudioManager.instance.SetAudioToSpecial(SpecialAudio.ClickButton);

    }
    /// <summary>
    /// 按下爆金币按钮
    /// </summary>
    void AwardBtnDown()
    {
        AudioManager.instance.SetAudioToSpecial(SpecialAudio.ClickButton);
        messageF.SetActive(false);
        taskName.text = "当前无任务";
        awardBtn.interactable=false;
        this.SendCommand(new UpdatePlayerDataCommand(new PlayerData(0, 0, 0, 0, model.currentTaskModel.Award,0)));
        this.SendCommand(new FinishTaskCommand(model.currentTaskModel));
    }
    /// <summary>
    /// 显示爆金币按钮
    /// </summary>
    /// <param name="e"></param>
    void ShowAwardBtn(OpenAwardBtnEvent e)
    {
        awardBtn.interactable=true;
    } /// <summary>
    /// 显示信息界面
    /// </summary>
    /// <param name="e"></param>
    void ShowTaskMesBtn(ShowTaskMesEvent e)
    {
        messageF.SetActive(true);
    }
    /// <summary>
    /// 重定向后初始化设置信息显示
    /// </summary>
    /// <param name="e"></param>
    void InitShowText(InitShowTextEvent e)
    {
        taskName.text = model.taskName;
        nearNumber.text = "还需要击杀"+model.nearEnemyNum.ToString()+"个近战敌人";
        remoteNumber.text = "还需要击杀"+model.RemoteEnemyNum.ToString()+"个远程敌人";
        molimoNumber.text = "还需要采摘"+model.moLimoNum.ToString()+"个魔力蘑";
        huolimoNumber.text = "还需要采摘"+model.huoLiMoNum.ToString()+"个火力蘑";
        xiangxuncaoNumber.text = "还需要采摘"+model.xiangXunCaoNum.ToString()+"个香薰草";
    }
    /// <summary>
    /// 更新任务信息显示
    /// </summary>
    /// <param name="e"></param>
    void UpdateShowText(UpdateShowTextEvent e)
    {
        nearNumber.text = "还需要击杀" + model.nearEnemyNum.ToString() + "个近战敌人";
        remoteNumber.text = "还需要击杀" + model.RemoteEnemyNum.ToString() + "个远程敌人";
        molimoNumber.text = "还需要采摘" + model.moLimoNum.ToString() + "个魔力蘑";
        huolimoNumber.text = "还需要采摘" + model.huoLiMoNum.ToString() + "个火力蘑";
        xiangxuncaoNumber.text = "还需要采摘" + model.xiangXunCaoNum.ToString() + "个香薰草";
    }
    /// <summary>
    /// 设置任务信息到Model
    /// </summary>
    /// <param name="e"></param>
    void SetTaskToDatails(SetTaskToDatails e)
    {
        this.SendCommand(new SetTaskDetailsCommand(e.model));
    }/// <summary>
    /// 更新任务信息到Model
    /// </summary>
    /// <param name="e"></param>
    void UpdateTaskToDatails(UpdateTaskToDatails e)
    {
        this.SendCommand(new UpdateTaskDetailsCommand(e.nearnum,e.remotenum,e.molimonum,e.huolimonum,e.xiangxuncaonum));
    }
    public IArchitecture GetArchitecture()
    {
        return GameFramework.Interface;
    }
}
/// <summary>
/// 重定向初始化设置任务信息
/// </summary>
public class SetTaskDetailsCommand : AbstractCommand
{
    TaskModel currentTaskModel;
    public SetTaskDetailsCommand(TaskModel currentTaskModel)
    {
        this.currentTaskModel = currentTaskModel;
    }
    protected override void OnExecute()
    {
        this.GetModel<TaskDetailsModel>().SetTaskDetailsMes(currentTaskModel);
    }
}
/// <summary>
/// 更新任务信息Command
/// </summary>
public class UpdateTaskDetailsCommand : AbstractCommand
{
    
    /// <summary>
    /// 剩余需要击杀近战敌人的数量
    /// </summary>
    public int nearEnemyNum;
    /// <summary>
    /// 剩余需要远程敌人的数量
    /// </summary>
    public int RemoteEnemyNum;    //这是View层的  懒得分了
    /// <summary>
    /// 剩余需要采摘的魔力蘑数量
    /// </summary>
    public int moLimoNum;
    /// <summary>
    /// 剩余需要采摘的火力蘑数量
    /// </summary>
    public int huoLiMoNum;
    /// <summary>
    ///剩余需要采摘的香薰草的数量
    /// </summary>
    public int xiangXunCaoNum;
    public UpdateTaskDetailsCommand(int nearEnemyNum,int RemoteEnemyNum,int moLimoNum,int huoLiMoNum,int xiangXunCaoNum)
    {
        
        this.nearEnemyNum = nearEnemyNum;
        this.RemoteEnemyNum = RemoteEnemyNum;
        this.moLimoNum = moLimoNum;
        this.huoLiMoNum = huoLiMoNum;
        this.xiangXunCaoNum = xiangXunCaoNum;
        if (nearEnemyNum < 0)
        {
            this.nearEnemyNum = 0;
        } if (RemoteEnemyNum < 0)
        {
            this.RemoteEnemyNum = 0;
        } if (moLimoNum < 0)
        {
            this.moLimoNum = 0;
        } if (huoLiMoNum < 0)
        {
            this.huoLiMoNum = 0;
        } if (xiangXunCaoNum < 0)
        {
            this.xiangXunCaoNum = 0;
        }
    }
    protected override void OnExecute()
    {
        this.GetModel<TaskDetailsModel>().UpdateNumber(nearEnemyNum,RemoteEnemyNum,moLimoNum,huoLiMoNum,xiangXunCaoNum);
    }
}
public class TaskDetailsModel : AbstractModel
{    /// <summary>
     /// 当前是否有任务
     /// </summary>
    bool isTask;
    /// <summary>
    /// 剩余需要击杀近战敌人的数量
    /// </summary>
    public int nearEnemyNum;
    /// <summary>
    /// 剩余需要远程敌人的数量
    /// </summary>
    public int RemoteEnemyNum;    //这是View层的  懒得分了
    /// <summary>
    /// 剩余需要采摘的魔力蘑数量
    /// </summary>
    public int moLimoNum;
    /// <summary>
    /// 剩余需要采摘的火力蘑数量
    /// </summary>
    public int huoLiMoNum;
    /// <summary>
    ///剩余需要采摘的香薰草的数量
    /// </summary>
    public int xiangXunCaoNum;
    public string taskName;
    /// <summary>
    /// 当前的任务Model
    /// </summary>
    public TaskModel currentTaskModel;
    protected override void OnInit()
    {

    }
    /// <summary>
    /// 重定向正在执行的任务
    /// </summary>
    public void SetTaskDetailsMes(TaskModel currentTaskModel)
    {
        isTask = true;
        this.currentTaskModel = currentTaskModel;
        Debug.Log(currentTaskModel);
        taskName = currentTaskModel.Describe;
        nearEnemyNum = currentTaskModel.nearEnemyNum;
        RemoteEnemyNum = currentTaskModel.RemoteEnemyNum;
        moLimoNum = currentTaskModel.moLimoNum;
        huoLiMoNum = currentTaskModel.huoLiMoNum;
        xiangXunCaoNum = currentTaskModel.xiangXunCaoNum;
        if (nearEnemyNum < 0)
        {
            this.nearEnemyNum = 0;
        }
        if (RemoteEnemyNum < 0)
        {
            this.RemoteEnemyNum = 0;
        }
        if (moLimoNum < 0)
        {
            this.moLimoNum = 0;
        }
        if (huoLiMoNum < 0)
        {
            this.huoLiMoNum = 0;
        }
        if (xiangXunCaoNum < 0)
        {
            this.xiangXunCaoNum = 0;
        }
        this.SendEvent<InitShowTextEvent>();
        this.SendEvent<ShowTaskMesEvent>();

    }
    /// <summary>
    /// 更新剩余数量
    /// </summary>
    public void UpdateNumber(int nearEnemyNum, int RemoteEnemyNum, int moLimoNum, int huoLiMoNum, int xiangXunCaoNum)
    {
        this.nearEnemyNum = nearEnemyNum;
        this.RemoteEnemyNum = RemoteEnemyNum;
        this.moLimoNum = moLimoNum;
        this.huoLiMoNum = huoLiMoNum;
        this.xiangXunCaoNum = xiangXunCaoNum;
        this.SendEvent<UpdateShowTextEvent>();
    }
}

/// <summary>
/// 打开领取奖励按钮Event
/// </summary>
public struct OpenAwardBtnEvent
{

}
/// <summary>
/// 显示任务面板信息Event
/// </summary>
public struct ShowTaskMesEvent
{

}
/// <summary>
/// 重定向之后初次设置UI显示数量设置
/// </summary>
public struct InitShowTextEvent
{

}
    /// <summary>
///更新设置UI显示数量设置
/// </summary>
public struct UpdateShowTextEvent
{

}