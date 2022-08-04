using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// 消耗品商店里的Item（未购买）
/// </summary>
public class ConsumableStoreItem : MonoBehaviour, IController, IPointerDownHandler, IPointerUpHandler
{

    public ItemType itemType;

    string itemName;


    int sellCoin;

    string describe;
    /// <summary>
    /// 初始化Item的数据
    /// </summary>
    void Start()
    {
        if (itemType == ItemType.DownAttack)
        {
            itemName = FieldManager.MeatName;
            sellCoin = FieldManager.MeatBuyCoin;
            describe = FieldManager.MeatDescribe;
        }
        if (itemType == ItemType.HP)
        {

            itemName = FieldManager.HpName;
            sellCoin = FieldManager.HpBuyCoin;
            describe = FieldManager.HpDescribe;
        }
        if (itemType == ItemType.MP)
        {

            itemName = FieldManager.MpName;
            sellCoin = FieldManager.MpBuyCoin;
            describe = FieldManager.MpDescribe;
        }
    }
    void Update()
    {

    }
    public IArchitecture GetArchitecture()
    {
        return GameFramework.Interface;
    }
    public void OnPointerDown(PointerEventData eventData)
    {

    }
    public void OnPointerUp(PointerEventData eventData)
    {
        //鼠标抬起的时候 显示使用或销毁UI
        if (Input.GetMouseButtonUp(1))
        {
            ConsumableStoreManager.instance.SetMessage(itemName, describe, sellCoin);
            this.SendCommand(new SetCurrentBuyConsumableData(itemType, sellCoin, this.transform.position + new Vector3(45, -40)));
        }
        if (Input.GetMouseButtonUp(0))
        {
            ConsumableStoreManager.instance.SetMessage(itemName, describe, sellCoin);
        }
    }
}