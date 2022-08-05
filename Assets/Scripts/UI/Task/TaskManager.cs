using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour, IController
{
    public static TaskManager instance;
    /// <summary>
    /// PanelF
    /// </summary>
    public GameObject taskPanel;
    /// <summary>
    /// 任务模块的爹
    /// </summary>
    public GameObject taskBGF;

    TaskManagerModel model;


    private void Awake()
    {
        instance = this;
        //接收升级事件
        this.RegisterEvent<LevelUpEvent>(UpdateLevelTask).UnRegisterWhenGameObjectDestroyed(this.gameObject);
        this.RegisterEvent<InitTaskUI>(e => InitTask());
        model = this.GetModel<TaskManagerModel>();
    }
    /// <summary>
    /// 打开Task面板
    /// </summary>
    public void OpenTaskPanel()
    {
        Time.timeScale = 0f;
        taskPanel.SetActive(true);
    }
    /// <summary>
    /// 关闭Task面板
    /// </summary>
    public void CloseTaskPanel()
    {
        Time.timeScale = 1f;
        taskPanel.SetActive(false);
    }
    /// <summary>
    /// 初始化生成任务模块
    /// </summary>
    void InitTask()
    {
        for (int i = 0; i < FieldManager.taskLevel.Count; i++)
        {
            if (!model.taskStoreData[FieldManager.taskLevel[i]].isComplete)
            {
                GameObject go = Instantiate(Resources.Load<GameObject>(FieldManager.TaskUI));
                go.transform.SetParent(taskBGF.transform);
                go.GetComponent<TaskMessage>().SetMessage(model.taskDic[FieldManager.taskLevel[i]], model.taskStoreData[FieldManager.taskLevel[i]]);
                model.taskModuleDic.Add(FieldManager.taskLevel[i], go.GetComponent<TaskMessage>());
                if (model.isTask)//如果正在有任务进行
                {
                    go.GetComponent<TaskMessage>().isTaskProceed = true;
                }
            }
        }
    }
    /// <summary>
    /// 升级事件
    /// </summary>
    /// <param name="e"></param>
    void UpdateLevelTask(LevelUpEvent e)
    {
        this.SendCommand(new LevelUpTaskCommand(e.level));
    }
    public IArchitecture GetArchitecture()
    {
        return GameFramework.Interface;
    }
    void OnApplicationQuit()
    {
        PlayDataManager.instance.SaveData(new TaskData(model.taskStoreData), FieldManager.TaskData);
    }
}
/// <summary>
/// 初始化已进行任务数据Command
/// </summary>
public class InitTaskManagerCommand : AbstractCommand
{
    TaskData taskData = new TaskData();
    public InitTaskManagerCommand(TaskData taskData)
    {
        this.taskData = taskData;
    }
    protected override void OnExecute()
    {
        this.GetModel<TaskManagerModel>().InitTaskManagerModel(taskData.taskLevelGatherNum);
    }
}
/// <summary>
/// 接收任务Command
/// </summary>
public class ReceiveTaskCommand : AbstractCommand
{
    TaskModel taskModel = new TaskModel();
    public ReceiveTaskCommand(TaskModel taskData)
    {
        this.taskModel = taskData;
    }
    protected override void OnExecute()
    {
        this.GetModel<TaskManagerModel>().ReceiveTask(taskModel);
    }
}/// <summary>
/// 接收任务Command
/// </summary>
public class FinishTaskCommand : AbstractCommand
{
    TaskModel taskModel = new TaskModel();
    public FinishTaskCommand(TaskModel taskData)
    {
        this.taskModel = taskData;
    }
    protected override void OnExecute()
    {
        this.GetModel<TaskManagerModel>().FinishTask(taskModel);
    }
}
/// <summary>
/// 接收任务Command
/// </summary>
public class LevelUpTaskCommand : AbstractCommand
{
    int level ;
    public LevelUpTaskCommand(int level)
    {
        this.level = level;
    }
    protected override void OnExecute()
    {
        this.GetModel<TaskManagerModel>().LevelUp(level);
    }
}
/// <summary>
/// 监听任务指标Command
/// </summary>
public class RegisterTasksNumCommand : AbstractCommand
{
    public int nearEnemyNum;
    public int RemoteEnemyNum;
    public int moLimoNum;
    public int huoLiMoNum;
    public int xiangXunCaoNum;
    public RegisterTasksNumCommand(int nearEnemyNum, int RemoteEnemyNum, int moLimoNum, int huoLiMoNum, int xiangXunCaoNum)
    {
        this.nearEnemyNum = nearEnemyNum;
        this.RemoteEnemyNum = RemoteEnemyNum;
        this.moLimoNum = moLimoNum;
        this.huoLiMoNum = huoLiMoNum;
        this.xiangXunCaoNum = xiangXunCaoNum;
    }
    protected override void OnExecute()
    {
        this.GetModel<TaskManagerModel>().UpdateTaskData(nearEnemyNum, RemoteEnemyNum, moLimoNum, huoLiMoNum, xiangXunCaoNum);
    }
}
/// <summary>
/// 任务总管理类Model
/// </summary>
public class TaskManagerModel : AbstractModel
{
    /// <summary>
    /// 数据流中的数据，仅初始化时调用和更改正在进行的任务数据时候调用
    /// </summary>
    public Dictionary<int, TaskMesData> taskStoreData = new Dictionary<int, TaskMesData>();
    /// <summary>
    /// 任务字典
    /// </summary>
    public Dictionary<int, TaskModel> taskDic = new Dictionary<int, TaskModel>();
    /// <summary>
    /// 所有生成出来的任务模块
    /// </summary>
    public Dictionary<int, TaskMessage> taskModuleDic = new Dictionary<int, TaskMessage>();

   
    /// <summary>
    /// 当前正在进行的任务
    /// </summary>
    TaskModel taskmodel;
    /// <summary>
    /// 当前是否有任务正在执行
    /// </summary>
    public  bool isTask = false;
    /// <summary>
    /// 任务需要的等级
    /// </summary>
    public int unlockLevel;
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
    protected override void OnInit()
    {

    }
    /// <summary>
    /// 初始化任务数据
    /// </summary>
    /// <param name="taskStoreData"></param>
    public void InitTaskManagerModel(Dictionary<int, TaskMesData> taskStoreData)
    {
        this.taskStoreData = taskStoreData;

        for (int i = 0; i < FieldManager.taskLevel.Count; i++)
        {
            taskDic.Add(FieldManager.taskLevel[i], new TaskModel(FieldManager.taskLevel[i], taskStoreData[FieldManager.taskLevel[i]].isTask,taskStoreData[FieldManager.taskLevel[i]].isLevelTo, FieldManager.taskNearEnemy[i],
                FieldManager.taskRemoteEnemy[i], FieldManager.taskMoliMo[i], FieldManager.taskHuoliMo[i], FieldManager.taskXiangXunCao[i],
                FieldManager.taksDescribe[i], FieldManager.taskAward[i], taskStoreData[FieldManager.taskLevel[i]].isComplete));
            if (taskStoreData[FieldManager.taskLevel[i]].isTask)//如果此任务正在进行
            {
                isTask = true;//设置为任务正在进行状态
                taskmodel = taskDic[FieldManager.taskLevel[i]];
                initProceedMessage(taskStoreData[FieldManager.taskLevel[i]]);
                this.SendEvent(new SetTaskToDatails
                {
                    model = new TaskModel(taskDic[FieldManager.taskLevel[i]].unlockLevel, true,taskStoreData[FieldManager.taskLevel[i]].isLevelTo,
                    taskDic[FieldManager.taskLevel[i]].nearEnemyNum - taskStoreData[FieldManager.taskLevel[i]].nearEnemyNum,
                     taskDic[FieldManager.taskLevel[i]].RemoteEnemyNum - taskStoreData[FieldManager.taskLevel[i]].RemoteEnemyNum,
                      taskDic[FieldManager.taskLevel[i]].moLimoNum - taskStoreData[FieldManager.taskLevel[i]].moLimoNum,
                       taskDic[FieldManager.taskLevel[i]].huoLiMoNum - taskStoreData[FieldManager.taskLevel[i]].huoLiMoNum,
                        taskDic[FieldManager.taskLevel[i]].xiangXunCaoNum - taskStoreData[FieldManager.taskLevel[i]].xiangXunCaoNum,
                         taskDic[FieldManager.taskLevel[i]].Describe, taskDic[FieldManager.taskLevel[i]].Award, false)
                });
            }
        }
        this.SendEvent<InitTaskUI>();
    }
    /// <summary>
    /// 初始化同步正在进行的此任务的数据
    /// </summary>
    void initProceedMessage(TaskMesData streamData)
    {
        unlockLevel = streamData.unlockLevel;
        isTask = streamData.isTask;
        nearEnemyNum = streamData.nearEnemyNum;
        RemoteEnemyNum = streamData.RemoteEnemyNum;
        moLimoNum = streamData.moLimoNum;
        huoLiMoNum = streamData.huoLiMoNum;
        xiangXunCaoNum = streamData.xiangXunCaoNum;
        if (taskmodel.nearEnemyNum <= nearEnemyNum &&
          taskmodel.RemoteEnemyNum <= RemoteEnemyNum && //如果达成任务条件
          taskmodel.moLimoNum <= moLimoNum &&
          taskmodel.huoLiMoNum <= huoLiMoNum &&
          taskmodel.xiangXunCaoNum <= xiangXunCaoNum)
        {
            this.SendEvent<OpenAwardBtnEvent>();
        }
    }
    /// <summary>
    /// 重置当前执行的任务信息(接取任务时)
    /// </summary>s
    void ResetMessage()
    {
        nearEnemyNum = 0;
        RemoteEnemyNum = 0;
        moLimoNum = 0;
        huoLiMoNum = 0;
        xiangXunCaoNum = 0;
    }
    /// <summary>
    /// 接受任务
    /// </summary>
    /// <param name="taskModel"></param>
    public void ReceiveTask(TaskModel taskModel)
    {
        isTask = true;//设置为任务正在进行状态
        unlockLevel = taskModel.unlockLevel;
        ResetMessage();
        this.SendEvent(new IsProceedTask { isTaskProceed = isTask });
        taskmodel = taskModel;
        this.SendEvent(new SetTaskToDatails { model = taskmodel });
        taskStoreData[taskModel.unlockLevel] = new TaskMesData (unlockLevel,true,false,true,0,0,0,0,0);
    }
    /// <summary>
    /// 更新当前任务的进度数据
    /// </summary>
    public void UpdateTaskData(int nearEnemuNum, int remoteEnemyNum, int molimoNum, int huolimoNum, int xiangxuncaoNum)
    {
        if (isTask)
        {
            nearEnemyNum += nearEnemuNum;
            RemoteEnemyNum += remoteEnemyNum;
            moLimoNum += molimoNum;
            huoLiMoNum += huolimoNum;
            xiangXunCaoNum += xiangxuncaoNum;
            this.SendEvent(new UpdateTaskToDatails
            {
                nearnum = taskmodel.nearEnemyNum - nearEnemyNum,
                remotenum = taskmodel.RemoteEnemyNum - RemoteEnemyNum
            ,
                molimonum = taskmodel.moLimoNum - moLimoNum,
                huolimonum = taskmodel.huoLiMoNum - huoLiMoNum,
                xiangxuncaonum = taskmodel.xiangXunCaoNum - xiangXunCaoNum
            });
            taskStoreData[unlockLevel] = new TaskMesData(unlockLevel, true,false,true,nearEnemyNum,RemoteEnemyNum,moLimoNum,huoLiMoNum,xiangXunCaoNum);
            
            if (taskmodel.nearEnemyNum <= nearEnemyNum &&
           taskmodel.RemoteEnemyNum <= RemoteEnemyNum && //如果达成任务条件
           taskmodel.moLimoNum <= moLimoNum &&
           taskmodel.huoLiMoNum <= huoLiMoNum &&
           taskmodel.xiangXunCaoNum <= xiangXunCaoNum)
            {
                this.SendEvent<OpenAwardBtnEvent>();
            }
        }
    }
    /// <summary>
    /// 结束任务
    /// </summary>
    /// <param name="model"></param>
    public void FinishTask(TaskModel model)
    {
        TaskMesData temp = new TaskMesData(model.unlockLevel, false,true,true, model.nearEnemyNum, 
            model.RemoteEnemyNum, model.moLimoNum, model.huoLiMoNum, model.xiangXunCaoNum);
        taskStoreData[model.unlockLevel] = temp;
        isTask = false;
        taskModuleDic[model.unlockLevel].FinishTask();
        taskModuleDic.Remove(model.unlockLevel);
        this.SendEvent(new IsProceedTask { isTaskProceed = isTask });
    }
    public void LevelUp(int level)
    {
        if (taskModuleDic.ContainsKey(level))
        {
            taskModuleDic[level].LevelToMethod();
        }
        if (taskStoreData.ContainsKey(level))
        {
            taskStoreData[level] = new TaskMesData(taskStoreData[level].unlockLevel, false,false, true, 0, 0, 0, 0, 0);
        }
    }
}
/// <summary>
/// 初始化任务UIEvent
/// </summary>
public struct InitTaskUI
{
}

/// <summary>
/// 是否接收任务Event
/// </summary>
public struct IsProceedTask
{
    public bool isTaskProceed;
}
/// <summary>
/// 设置任务到已接任务面板Event
/// </summary>
public struct SetTaskToDatails
{
    public TaskModel model;
}/// <summary>
/// 更新任务数据到已接任务面板Event
/// </summary>
public struct UpdateTaskToDatails
{
    public int nearnum;
    public int remotenum;
    public int molimonum;
    public int huolimonum;
    public int xiangxuncaonum;
}