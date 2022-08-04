using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
public class MoLiMoController : IItemController
{
    protected override void Awake()
    {
        base.Awake();
        this.RegisterEvent<MoLiItemDataEvent>(SetItemAndModel).UnRegisterWhenGameObjectDestroyed(this.gameObject);
        model = this.GetModel<MoLiMoModel>();
    }
    protected override void Start()
    {
        base.Start();
         this.GetSystem<MoLiMoSystem>().IsItemType(pickItemType);
    }
    public override void SellItem()
    {
        this.SendCommand<ReduceMoLiItemCommand>();
         this.SendCommand(new UpdatePlayerDataCommand(new PlayerData(0, 0, 0, 0, FieldManager.MagicSellCoin, 0)));
    }
    public override void UseItem()
    {
        base.UseItem();
        this.SendCommand<ReduceMoLiItemCommand>();
    }
    protected override void SetItemAndModel(ItemDataEvent itemData)
    {
        base.SetItemAndModel(itemData);
        this.SendCommand(new SetMoLiItemCommand(item));
        //this.SendCommand(new AddBackGroundItemCommand(pickItemType,model.Number));
    }
}
public class MoLiMoModel : IItmeModel
{

}
public class MoLiMoSystem : IItemSystem
{
    protected override void OnInit()
    {
        base.OnInit();
    }
    public override void IsItemType(ItemType pickItemType)
    {
        ItemDataModel pickitem = null;
        pickitem =
            new ItemDataModel(FieldManager.MagicName, FieldManager.MagicSellCoin, FieldManager.MagicDescribe, FieldManager.MagicExp, 0, 0, 0,0,0,0);
        if (pickitem != null)
        {
            this.SendEvent(new MoLiItemDataEvent { item = pickitem });
        }
    }
}
/// <summary>
/// 设置魔力蘑Item属性Command
/// </summary>
public class SetMoLiItemCommand : AbstractCommand
{
    ItemDataModel PickItem;
    public SetMoLiItemCommand(Item pickItem)
    {
        this.PickItem = (ItemDataModel)pickItem;
    }
    protected override void OnExecute()
    {
        this.GetModel<MoLiMoModel>().SetMessage(PickItem);
    }
}
/// <summary>
/// 减少魔力蘑数量Command
/// </summary>
public class ReduceMoLiItemCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        this.GetModel<MoLiMoModel>().ReduceItemNumber();
    }
}
/// <summary>
/// 增加魔力蘑数量Command
/// </summary>
public class AddMoLiItemCommand : AbstractCommand
{
    int number;

    public AddMoLiItemCommand(int number)
    {
        this.number = number;
    }
    protected override void OnExecute()
    {
        this.GetModel<MoLiMoModel>().AddItemNumber(number);
    }
}
/// <summary>
/// 初始化魔力蘑数量Command
/// </summary>
public class InitMoLiItemCommand : AbstractCommand
{
    int number;

    public InitMoLiItemCommand(int number)
    {
        this.number = number;
    }
    protected override void OnExecute()
    {
        this.GetModel<MoLiMoModel>().SetNumber(number);
    }
}

public class MoLiItemDataEvent : ItemDataEvent
{
    public Item item { get; set; }
}
