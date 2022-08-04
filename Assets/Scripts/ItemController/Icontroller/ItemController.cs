using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemController : MonoBehaviour
{
    public ItemType pickItemType;

    /// <summary>
    /// 是否删除此物品
    /// </summary>
    /// <param name="e"></param>
    protected abstract void IsDeleteThis(DeleteItemEvent e);
    /// <summary>
    /// 更新Model层数据的回调函数
    /// </summary>
    /// <param name="e"></param>
    protected abstract void UpdateItemEventMethod(UpdateItemEvent e);
    /// <summary>
    /// 使用Item
    /// </summary>
    public abstract void UseItem ();
    /// <summary>
    /// 出售Item
    /// </summary>
    public abstract void SellItem();
}