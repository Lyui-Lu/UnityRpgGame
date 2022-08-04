using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ISkill : MonoBehaviour
{  
    /// <summary>
    /// 技能冷却时间
    /// </summary>
    protected int cD=10;

    public Image mask;

    public Text key;
    /// <summary>
    /// 是否可以释放技能
    /// </summary>
    protected bool isRealse=true;

    protected virtual void Update()
    {
        if (!isRealse)
        {
            mask.fillAmount -= Time.deltaTime/cD;

            if (mask.fillAmount <= 0)
            {
                isRealse = true;
            }
        }
    }

    /// <summary>
    /// 释放技能
    /// </summary>
    public abstract void Release();
    /// <summary>
    /// 更改技能顺序(更改按钮)
    /// </summary>
    public abstract void ChangeOrder();
}