using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

/// <summary>
/// 物品类型
/// </summary>
public enum ItemType
{
    #region 可拾取物品类型
    Fire,
    XiangXunCao,
    Magic,
    #endregion

    #region 消耗品物品类型
    HP,
    MP,
    DownAttack
    #endregion
}
/// <summary>
/// 可拾取物品基类
/// </summary>
public abstract class IItemController : ItemController, IController, IPointerDownHandler, IPointerUpHandler
{
    /// <summary>
    /// 数量
    /// </summary>
    public Text Number;
    public IItmeModel model;
    protected Item item;
    protected virtual void Awake()
    {
        this.RegisterEvent<DeleteItemEvent>(IsDeleteThis).UnRegisterWhenGameObjectDestroyed(this.gameObject);
        this.RegisterEvent<UpdateItemEvent>(UpdateItemEventMethod).UnRegisterWhenGameObjectDestroyed(this.gameObject);
    }
    protected virtual void Start()
    {
        UpdateText();
    }
    /// <summary>
    /// Action System判断完成类型之后返回Event调用此方法
    /// </summary>
    /// <param name="itemData"></param>
    protected virtual void SetItemAndModel(ItemDataEvent itemData)
    {
        item = itemData.item;
        //设置到Model;
    }
    ///// <summary>
    ///// Model的Number数量为0时调用返回此事件
    ///// </summary>
    ///// <param name="e"></param>
    protected override void IsDeleteThis(DeleteItemEvent e)
    {
        if (model == e.model)
        {
             this.SendCommand(new RemoveBackGroundItemCommand(pickItemType));
            Destroy(this.gameObject);
        }
    }
    /// <summary>
    /// 更新Model数据的回调
    /// </summary>
    /// <param name="e"></param>
    protected override void UpdateItemEventMethod(UpdateItemEvent e)
    {
        if (model == e.model)
        {
            this.SendCommand(new UpdateBackGroundItemCommand(pickItemType, model.Number));
            UpdateText();
        }
    }
    protected void UpdateText()
    {
        Number.text = model.Number.ToString();
        if (model.Number >= 2)
        {
            Number.gameObject.SetActive(true);
        }
        else
        {
            Number.gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// 使用物品
    /// </summary>
    public override void UseItem()
    {
        if (pickItemType == ItemType.Fire || pickItemType == ItemType.Magic || pickItemType == ItemType.HP || pickItemType == ItemType.MP)
        {
            this.SendCommand(new UpdatePlayerDataCommand(new PlayerData(0, model.attack, model.hp, model.mp, 0, model.exp)));
        }
        if (pickItemType == ItemType.XiangXunCao)
        {
            this.SendCommand(new MoveSpeedCommand(model.speedPercent,model.timeOf));
        }
        if (pickItemType == ItemType.DownAttack)
        {
            this.SendCommand<MinusAttackCommand>();
        }
    }
    /// <summary>
    /// 出售物品
    /// </summary>
    public override void SellItem()
    {

    }
    public void OnPointerDown(PointerEventData eventData)
    {

    }
    public void OnPointerUp(PointerEventData eventData)
    {
        //鼠标抬起的时候 显示使用或销毁UI
        if (Input.GetMouseButtonUp(1))
        {
            UseOrSellController.instance.ShowUI(this.transform.position, this);
            ItemMessageController.instance.SetMessage(model.itemName, model.describe, 
                model.attack.ToString(), model.speedPercent.ToString("P")
                , model.exp.ToString(),model.hp.ToString(),model.mp.ToString(),model.minusAttack.ToString("P"), pickItemType);
        }
        if (Input.GetMouseButtonUp(0))
        {
            ItemMessageController.instance.SetMessage(model.itemName, model.describe,
                model.attack.ToString(), model.speedPercent.ToString("P")
                , model.exp.ToString(),model.hp.ToString(),model.mp.ToString(),model.minusAttack.ToString("P"),pickItemType);
        }
    }
    public IArchitecture GetArchitecture()
    {
        return GameFramework.Interface;
    }

}
/// <summary>
/// 物品Model
/// </summary>
public class IItmeModel : AbstractModel
{
    /// <summary>
    /// 物品名称
    /// </summary>
    public string itemName;
    /// <summary>
    /// 物品描述
    /// </summary>
    public string describe;
    /// <summary>
    /// 出售价
    /// </summary>
    public int sellPrice;
    /// <summary>
    /// 经验值
    /// </summary>
    public int exp;
    /// <summary>
    /// 攻击力
    /// </summary>
    public int attack;
    /// <summary>
    /// 增加血量值
    /// </summary>
    public int hp;
    /// <summary>
    /// 增加法力值
    /// </summary>
    public int mp;
    /// <summary>
    /// 增加伤害减免
    /// </summary>
    public float minusAttack;
    /// <summary>
    /// 移动速度百分比
    /// </summary>
    public float speedPercent;
    /// <summary>
    /// 效果持续时间
    /// </summary>
    public int timeOf;
    /// <summary>
    /// 当前物品数量
    /// </summary>
    public int Number;
    protected override void OnInit()
    {
        Number = 1;
    }
    /// <summary>
    /// 设置属性
    /// </summary>
    public void SetMessage(ItemDataModel item)
    {
        this.itemName = item.Itemname;
        this.describe = item.describe;
        this.sellPrice = item.sellPrice;
        this.exp = item.exp;
        this.attack = item.attack;
        this.speedPercent = item.speedPercent;
        this.timeOf = item.timeOf;
        this.hp = item.hp;
        this.mp = item.mp;
        this.minusAttack = item.munusAttack;
    }
    public void SetNumber(int number = 1)
    {
        Number = number;
    }
    /// <summary>
    /// 增加数量
    /// </summary>
    /// <param name="number"></param>
    public void AddItemNumber(int number = 1)
    {
        Number += number;
        this.SendEvent(new UpdateItemEvent { model=this});
    }
    /// <summary>
    /// 减少数量
    /// </summary>
    /// <param name="number"></param>
    public void ReduceItemNumber(int number = 1)
    { 
        Number -= number;
        if (Number <= 0)
        {
            this.SendEvent(new DeleteItemEvent { model = this });
            ItemMessageController.instance.EmptyMessage();
        }
        this.SendEvent(new UpdateItemEvent { model=this});
    }
}
/// <summary>
/// 物品属性System(协助Controller)
/// </summary>
public abstract  class IItemSystem : AbstractSystem
{

    protected override void OnInit(){ }
    /// <summary>
    /// 判断物品属性，返回物品数值和Model
    /// </summary>
    /// <param name="pickItemType"></param>
    public abstract void IsItemType(ItemType pickItemType);

}
/// <summary>
/// 删除Item事件
/// </summary>
public struct DeleteItemEvent
{
    public IItmeModel model;
}
/// <summary>
/// 更新当前Item数量
/// </summary>
public struct UpdateItemEvent
{
    public IItmeModel model;
}
/// <summary>
/// System返回Item数据事件接口
/// </summary>
public interface ItemDataEvent
{
    public Item item { get; set; }
}