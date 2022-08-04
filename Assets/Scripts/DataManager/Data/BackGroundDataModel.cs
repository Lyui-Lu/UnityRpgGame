using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class BackGroundDataModel : Data
{
    /// <summary>
    /// 背包中Item种类
    /// </summary>
    public List<ItemType> itemTypes=new List<ItemType>();
    /// <summary>
    /// 此种类Item有多少个
    /// </summary>
    public Dictionary<ItemType,int> itemsNumber=new Dictionary<ItemType, int>();
    /// <summary>
    /// 背包内存储Itme最大数量
    /// </summary>
    public int maxLimit=24 ;
    /// <summary>
    /// 当前背包内存储个数
    /// </summary>
    public int currentNumber = 0;

    public BackGroundDataModel(List<ItemType> itmeModels,Dictionary<ItemType,int> itemsNumber,int maxlimit,int currentnumber)
    {
        this.itemTypes = itmeModels;
        this.itemsNumber = itemsNumber;
        this.maxLimit = maxlimit;
        this.currentNumber = currentnumber;
    }
    public BackGroundDataModel()
    {
    }
}
