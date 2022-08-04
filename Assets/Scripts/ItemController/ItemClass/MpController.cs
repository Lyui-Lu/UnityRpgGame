using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
public class MpController : IItemController
{
    protected override void Awake()
    {
        base.Awake();
        this.RegisterEvent<MpItemDataEvent>(SetItemAndModel).UnRegisterWhenGameObjectDestroyed(this.gameObject);
        model = this.GetModel<MpModel>();
    }
    protected override void Start()
    {
        base.Start();
        this.GetSystem<MpSystem>().IsItemType(pickItemType);
    }
    public override void SellItem()
    {
        this.SendCommand<ReduceMpItemCommand>();
        this.SendCommand(new UpdatePlayerDataCommand(new PlayerData(0, 0, 0, 0, FieldManager.MpSellCoin, 0)));
    }
    public override void UseItem()
    {
        base.UseItem();
        this.SendCommand<ReduceMpItemCommand>();
    }
    protected override void SetItemAndModel(ItemDataEvent itemData)
    {
        base.SetItemAndModel(itemData);
        this.SendCommand(new SetMpItemCommand(item));
        //this.SendCommand(new AddBackGroundItemCommand(pickItemType,model.Number));
    }
}
public class MpModel : IItmeModel
{

}
public class MpSystem : IItemSystem
{
    protected override void OnInit()
    {
        base.OnInit();
    }
    public override void IsItemType(ItemType pickItemType)
    {
        ItemDataModel pickitem = null;
        pickitem =
            new ItemDataModel(FieldManager.MpName, FieldManager.MpSellCoin, FieldManager.MpDescribe, 0,0,0,0,0,FieldManager.MpAddMp,0);
        if (pickitem != null)
        {
            this.SendEvent(new MpItemDataEvent { item = pickitem });
        }
    }
}
/// <summary>
/// 设置血瓶Item属性Command
/// </summary>
public class SetMpItemCommand : AbstractCommand
{
    ItemDataModel PickItem;
    public SetMpItemCommand(Item pickItem)
    {
        this.PickItem = (ItemDataModel)pickItem;
    }
    protected override void OnExecute()
    {
        this.GetModel<MpModel>().SetMessage(PickItem);
    }
}
/// <summary>
/// 减少血瓶数量Command
/// </summary>
public class ReduceMpItemCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        this.GetModel<MpModel>().ReduceItemNumber();
    }
}
/// <summary>
/// 增加血瓶数量Command
/// </summary>
public class AddMpItemCommand : AbstractCommand
{
    int number;

    public AddMpItemCommand(int number)
    {
        this.number = number;
    }
    protected override void OnExecute()
    {
        this.GetModel<MpModel>().AddItemNumber(number);
    }
}
/// <summary>
/// 初始化血瓶数量Command
/// </summary>
public class InitMpItemCommand : AbstractCommand
{
    int number;

    public InitMpItemCommand(int number)
    {
        this.number = number;
    }
    protected override void OnExecute()
    {
        this.GetModel<MpModel>().SetNumber(number);
    }
}

public class MpItemDataEvent : ItemDataEvent
{
    public Item item { get; set; }
}
