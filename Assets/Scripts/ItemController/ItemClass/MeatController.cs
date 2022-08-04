using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
public class MeatController : IItemController
{
    protected override void Awake()
    {
        base.Awake();
        this.RegisterEvent<MeatItemDataEvent>(SetItemAndModel).UnRegisterWhenGameObjectDestroyed(this.gameObject);
        model = this.GetModel<MeatModel>();
    }
    protected override void Start()
    {
        base.Start();
        this.GetSystem<MeatSystem>().IsItemType(pickItemType);
    }
    public override void SellItem()
    {
        this.SendCommand<ReduceMeatItemCommand>();
        this.SendCommand(new UpdatePlayerDataCommand(new PlayerData(0, 0, 0, 0, FieldManager.MeatSellCoin, 0)));
    }
    public override void UseItem()
    {
        base.UseItem();
        this.SendCommand<ReduceMeatItemCommand>();
    }
    protected override void SetItemAndModel(ItemDataEvent itemData)
    {
        base.SetItemAndModel(itemData);
        this.SendCommand(new SetMeatItemCommand(item));
        //this.SendCommand(new AddBackGroundItemCommand(pickItemType,model.Number));
    }
}
public class MeatModel : IItmeModel
{

}
public class MeatSystem : IItemSystem
{
    protected override void OnInit()
    {
        base.OnInit();
    }
    public override void IsItemType(ItemType pickItemType)
    {
        ItemDataModel pickitem = null;
        pickitem =
            new ItemDataModel(FieldManager.MeatName, FieldManager.MeatSellCoin, FieldManager.MeatDescribe, 0, 0, 0, FieldManager.MeatTimeOf, 0, 0, FieldManager.MeatMinusAttack);
        if (pickitem != null)
        {
            this.SendEvent(new MeatItemDataEvent { item = pickitem });
        }
    }
}
/// <summary>
/// 设置血瓶Item属性Command
/// </summary>
public class SetMeatItemCommand : AbstractCommand
{
    ItemDataModel PickItem;
    public SetMeatItemCommand(Item pickItem)
    {
        this.PickItem = (ItemDataModel)pickItem;
    }
    protected override void OnExecute()
    {
        this.GetModel<MeatModel>().SetMessage(PickItem);
    }
}
/// <summary>
/// 减少血瓶数量Command
/// </summary>
public class ReduceMeatItemCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        this.GetModel<MeatModel>().ReduceItemNumber();
    }
}
/// <summary>
/// 增加血瓶数量Command
/// </summary>
public class AddMeatItemCommand : AbstractCommand
{
    int number;

    public AddMeatItemCommand(int number)
    {
        this.number = number;
    }
    protected override void OnExecute()
    {
        this.GetModel<MeatModel>().AddItemNumber(number);
    }
}
/// <summary>
/// 初始化血瓶数量Command
/// </summary>
public class InitMeatItemCommand : AbstractCommand
{
    int number;

    public InitMeatItemCommand(int number)
    {
        this.number = number;
    }
    protected override void OnExecute()
    {
        this.GetModel<MeatModel>().SetNumber(number);
    }
}

public class MeatItemDataEvent : ItemDataEvent
{
    public Item item { get; set; }
}
