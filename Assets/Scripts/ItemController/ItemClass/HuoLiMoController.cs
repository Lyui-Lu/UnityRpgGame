using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
public class HuoLiMoController : IItemController
{
    protected override void Awake()
    {
        base.Awake();
        this.RegisterEvent<HuoLiItemDataEvent>(SetItemAndModel).UnRegisterWhenGameObjectDestroyed(this.gameObject);
        model = this.GetModel<HuoLiMoModel>();
    }
    protected override void Start()
    {
        base.Start();
        this.GetSystem<HuoLiMoSystem>().IsItemType(pickItemType);
    }
    public override void SellItem()
    {
        this.SendCommand<ReduceHuoLiItemCommand>();
         this.SendCommand(new UpdatePlayerDataCommand(new PlayerData(0, 0, 0, 0, FieldManager.FireSellCoin, 0)));
    }
    public override void UseItem()
    {
        base.UseItem();
        this.SendCommand<ReduceHuoLiItemCommand>();
    }
    protected override void SetItemAndModel(ItemDataEvent itemData)
    {
        base.SetItemAndModel(itemData);
        this.SendCommand(new SetHuoLiItemCommand(item));
    }
}
public class HuoLiMoModel : IItmeModel
{
    
}
public class HuoLiMoSystem : IItemSystem
{
    protected override void OnInit()
    {
        base.OnInit();
    }
    public override void IsItemType(ItemType pickItemType)
    {
        ItemDataModel pickitem = null;
        pickitem =
            new ItemDataModel(FieldManager.FireName, FieldManager.FireSellCoin, FieldManager.FireDescribe, 0, FieldManager.FireAttack, 0, FieldManager.FireTimeOf,0,0,0);
        if (pickitem != null)
        {
            this.SendEvent(new HuoLiItemDataEvent { item = pickitem });
        }
    }
}
/// <summary>
/// 设置火力蘑Item属性Command
/// </summary>
public class SetHuoLiItemCommand : AbstractCommand
{
    ItemDataModel PickItem;
    public SetHuoLiItemCommand(Item pickItem)
    {
        this.PickItem = (ItemDataModel)pickItem;
    }
    protected override void OnExecute()
    {
        this.GetModel<HuoLiMoModel>().SetMessage(PickItem);
    }
}
/// <summary>
/// 减少火力蘑数量Command
/// </summary>
public class ReduceHuoLiItemCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        this.GetModel<HuoLiMoModel>().ReduceItemNumber();
    }
}
/// <summary>
/// 增加火力蘑数量Command
/// </summary>
public class AddHuoLiItemCommand : AbstractCommand
{
    int number;

    public AddHuoLiItemCommand(int number)
    {
        this.number = number;
    }
    protected override void OnExecute()
    {
        this.GetModel<HuoLiMoModel>().AddItemNumber(number);
    }
}
/// <summary>
/// 初始化火力蘑数量Command
/// </summary>
public class InitHuoLiItemCommand : AbstractCommand
{
    int number;

    public InitHuoLiItemCommand(int number)
    {
        this.number = number;
    }
    protected override void OnExecute()
    {
        this.GetModel<HuoLiMoModel>().SetNumber(number);
    }
}

public class HuoLiItemDataEvent:ItemDataEvent
{
    public Item item { get; set; }
}
