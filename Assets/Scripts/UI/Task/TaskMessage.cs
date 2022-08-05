using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskMessage : MonoBehaviour,IController
{
    public Text taskName;

    public Text coinNum;

    public Text taskLevel;
    /// <summary>
    /// 接收任务Btn
    /// </summary>
    public Button taskBtn;
    /// <summary>
    /// 正在进行任务Text;
    /// </summary>
    public Text taskProceed;

    TaskModel model=new TaskModel ();


    /// <summary>
    /// 是否正在有任务进行
    /// </summary>
    public bool isTaskProceed;
    /// <summary>
    /// 等级是否到这了
    /// </summary>
    public bool isLevelTo;

    private void Awake()
    {
        this.RegisterEvent<IsProceedTask>(SetTaskState).UnRegisterWhenGameObjectDestroyed(this.gameObject);
    }
    private void Start()
    {
        taskBtn.onClick.AddListener(ReceiveTaskBtn);
        if (isLevelTo && !isTaskProceed)
        {
            taskBtn.interactable = true;
        }
    }
    /// <summary>
    /// 初始化设置Task信息
    /// </summary>
    /// <param name="model"></param>
    /// <param name="streamData"></param>
    public void SetMessage(TaskModel model,TaskMesData streamData)
    {
        this.model = model;
        taskName.text = model.Describe;
        coinNum.text = model.Award.ToString();
        taskLevel.text = model.unlockLevel.ToString();
        isLevelTo = streamData.isLevelTo;
        if (model.isTask)//如果正在进行任务
        {
            isTaskProceed = true;
            taskProceed.gameObject.SetActive(true);
        }
        if (model.isComplete)//如果完成了此任务
        {
            Destroy(this.gameObject);
        }
    }
    /// <summary> 
    /// 设置Task的状态 All
    /// </summary>
    /// <param name="e"></param>
    void SetTaskState(IsProceedTask e)
    {
        isTaskProceed = e.isTaskProceed;
        Debug.Log(222);

        if (isTaskProceed)
        {
            taskBtn.interactable = false;
        }
        else
        {
            if (isLevelTo&&!model.isComplete)
            {
                taskBtn.interactable = true;
            }
        }
    }
    
    /// <summary>
    /// 接收任务
    /// </summary>
    void ReceiveTaskBtn()
    {
        if (!isTaskProceed)
        {
            AudioManager.instance.SetAudioToSpecial(SpecialAudio.ClickButton);
            taskProceed.gameObject.SetActive(true);
            this.SendCommand(new ReceiveTaskCommand(model));
        }
    }
    /// <summary>
    /// 任务结束
    /// </summary>
    public void FinishTask()
    {
        Destroy(this.gameObject);
    }
    /// <summary>
    /// 角色等级达到此任务需要等级调用方法
    /// </summary>
    public void LevelToMethod()
    {
        isLevelTo = true;
        if (isTaskProceed)
        {
            taskBtn.interactable = false;
        }
        else
        {
            taskBtn.interactable = true;
        }
    }
    public IArchitecture GetArchitecture()
    {
        return GameFramework.Interface;
    }
}