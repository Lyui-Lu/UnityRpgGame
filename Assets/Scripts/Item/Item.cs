using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 物品数据模型基类
/// </summary>
public  class Item
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Itemname;
    /// <summary>
    /// 出售价格
    /// </summary>
    public int sellPrice;
    /// <summary>
    /// 描述
    /// </summary>
    public string describe;
    public Item(string itemname, int sellprice, string describe)
    {
        this.Itemname = itemname;
        this.sellPrice = sellprice;
        this.describe = describe;
    }
}
