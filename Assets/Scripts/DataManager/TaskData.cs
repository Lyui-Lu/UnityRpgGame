using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
/// <summary>
/// 任务数据模型(要存储到流中的)
/// </summary>
public class TaskData :Data
{
    /// <summary>
    /// 对应等级目前的数据
    /// </summary>
    public Dictionary<int, TaskMesData> taskLevelGatherNum = new Dictionary<int, TaskMesData>();


    public TaskData()
    {
        for (int i = 0; i < FieldManager.taskLevel.Count; i++)
        {
            taskLevelGatherNum.Add(FieldManager.taskLevel[i], new TaskMesData());
        }
    }
    public TaskData(Dictionary<int, TaskMesData> taskLevelGatherNum)
    {
        this.taskLevelGatherNum = taskLevelGatherNum;
    }
}
