using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
public class HpController : IItemController
{
    protected override void Awake()
    {
        base.Awake();
        this.RegisterEvent<HpItemDataEvent>(SetItemAndModel).UnRegisterWhenGameObjectDestroyed(this.gameObject);
        model = this.GetModel<HpModel>();
    }
    protected override void Start()
    {
        base.Start();
        this.GetSystem<HpSystem>().IsItemType(pickItemType);
    }
    public override void SellItem()
    {
        this.SendCommand<ReduceHpItemCommand>();
        this.SendCommand(new UpdatePlayerDataCommand(new PlayerData(0, 0, 0, 0, FieldManager.HpSellCoin, 0)));
        base.SellItem();
    }
    public override void UseItem()
    {
        base.UseItem();
        this.SendCommand<ReduceHpItemCommand>();
    }
    protected override void SetItemAndModel(ItemDataEvent itemData)
    {
        base.SetItemAndModel(itemData);
        this.SendCommand(new SetHpItemCommand(item));
        //this.SendCommand(new AddBackGroundItemCommand(pickItemType,model.Number));
    }
}
public class HpModel : IItmeModel
{

}
public class HpSystem : IItemSystem
{
    protected override void OnInit()
    {
        base.OnInit();
    }
    public override void IsItemType(ItemType pickItemType)
    {
        ItemDataModel pickitem = null;
        pickitem =
            new ItemDataModel(FieldManager.HpName, FieldManager.HpSellCoin, FieldManager.HpDescribe,0, 0, 0, 0,FieldManager.HpAddHp,0,0);
        if (pickitem != null)
        {
            this.SendEvent(new HpItemDataEvent { item = pickitem });
        }
    }
}
/// <summary>
/// 设置血瓶Item属性Command
/// </summary>
public class SetHpItemCommand : AbstractCommand
{
    ItemDataModel PickItem;
    public SetHpItemCommand(Item pickItem)
    {
        this.PickItem = (ItemDataModel)pickItem;
    }
    protected override void OnExecute()
    {
        this.GetModel<HpModel>().SetMessage(PickItem);
    }
}
/// <summary>
/// 减少血瓶数量Command
/// </summary>
public class ReduceHpItemCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        this.GetModel<HpModel>().ReduceItemNumber();
    }
}
/// <summary>
/// 增加血瓶数量Command
/// </summary>
public class AddHpItemCommand : AbstractCommand
{
    int number;

    public AddHpItemCommand(int number)
    {
        this.number = number;
    }
    protected override void OnExecute()
    {
        this.GetModel<HpModel>().AddItemNumber(number);
    }
}
/// <summary>
/// 初始化血瓶数量Command
/// </summary>
public class InitHpItemCommand : AbstractCommand
{
    int number;

    public InitHpItemCommand(int number)
    {
        this.number = number;
    }
    protected override void OnExecute()
    {
        this.GetModel<HpModel>().SetNumber(number);
    }
}

public class HpItemDataEvent : ItemDataEvent
{
    public Item item { get; set; }
}
