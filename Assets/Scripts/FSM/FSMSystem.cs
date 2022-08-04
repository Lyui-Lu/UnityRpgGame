using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMSystem
{
    private Dictionary<StateID, FSMState> stateDic = new Dictionary<StateID, FSMState>();
    private StateID currentStateID;
    private FSMState currentState;

    /// <summary>
    /// 更新npc的动作
    /// </summary>
    public void Update(GameObject npc)
    {
        currentState.Act(npc);
        currentState.Reason(npc);
    }
    /// <summary>
    /// 添加新状态
    /// </summary>
    public void AddState(FSMState state)
    {
        if (state == null)
        {
            Debug.LogError("FSMState不能为空");
            return;
        }
        if (currentState == null)
        {
            currentState = state;
            currentStateID = state.StateID;
        }
        if (stateDic.ContainsKey(state.StateID))
        {
            Debug.LogError("状态" + state.StateID + "已经存在，无法重复添加");
            return;
        }
        stateDic.Add(state.StateID, state);
    }

    /// <summary>
    /// 删除状态
    /// </summary>
    public void DeleteState(StateID stateID)
    {
        if (stateID == StateID.NullState)
        {
            Debug.LogError("无法删除空状态");
            return;
        }
        if (!stateDic.ContainsKey(stateID))
        {
            Debug.LogError("无法删除不存在的状态");
            return;
        }
        stateDic.Remove(stateID);
    }
    /// <summary>
    /// 执行过渡条件满足时对应状态该做的事
    /// </summary>
    public void PerformTransition(Transition transition)
    {
        if (transition == Transition.NullTransition)
        {
            Debug.LogError("无法执行空的转换条件");
            return;
        }
        StateID id = currentState.GetOutputState(transition);
        if (id == StateID.NullState)
        {
            Debug.LogWarning("当前状态" + currentStateID + "无法根据转换条件" + transition + "发生转换");
            return;
        }
        if (!stateDic.ContainsKey(id))
        {
            Debug.LogError("在状态机里面不存在状态" + id + ",无法进行状态转换");
            return;
        }
        FSMState state = stateDic[id];
        currentState.DoAfterLeave();
        currentState = state;
        currentStateID = state.StateID;
        currentState.DoBeforeEnter();
    }
}

