using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
public class XiangXunCaoController : IItemController
{
    protected override void Awake()
    {
        base.Awake();
        this.RegisterEvent<XiangXunCaoItemDataEvent>(SetItemAndModel).UnRegisterWhenGameObjectDestroyed(this.gameObject);
        model = this.GetModel<XiangXunCaoModel>();
    }
    protected override void Start()
    {
       base.Start();
        this.GetSystem<XiangXunCaoSystem>().IsItemType(pickItemType);
    }
    public override void SellItem()
    {
        this.SendCommand<ReduceXiangXunCaoItemCommand>();
        this.SendCommand(new UpdatePlayerDataCommand(new PlayerData(0, 0, 0, 0, FieldManager.XiangSellCoin, 0)));
        base.SellItem();
    }
    public override void UseItem()
    {
        base.UseItem();
        this.SendCommand<ReduceXiangXunCaoItemCommand>();  
    }
    protected override void SetItemAndModel(ItemDataEvent itemData)
    {
        base.SetItemAndModel(itemData);
        this.SendCommand(new SetXiangXunCaoItemCommand(item));
         //this.SendCommand(new AddBackGroundItemCommand(pickItemType,model.Number));
    }
}
public class XiangXunCaoModel : IItmeModel
{

}
public class XiangXunCaoSystem : IItemSystem
{
    protected override void OnInit()
    {
        base.OnInit();
    }
    public override void IsItemType(ItemType pickItemType)
    {
        ItemDataModel pickitem = null;
        pickitem =
            new ItemDataModel(FieldManager.XiangName, FieldManager.XiangSellCoin, FieldManager.XiangDescribe, 0, 0, FieldManager.XiangSpeedPercent, FieldManager.XiangTimeOf,0,0,0);
        if (pickitem != null)
            this.SendEvent(new XiangXunCaoItemDataEvent { item = pickitem });
        
    }
}
/// <summary>
/// 设置香薰草Item属性Command
/// </summary>
public class SetXiangXunCaoItemCommand : AbstractCommand
{
    ItemDataModel PickItem;
    public SetXiangXunCaoItemCommand(Item pickItem)
    {
        this.PickItem = (ItemDataModel)pickItem;
    }
    protected override void OnExecute()
    {
        this.GetModel<XiangXunCaoModel>().SetMessage(PickItem);
    }
}
/// <summary>
/// 减少香薰草数量Item
/// </summary>
public class ReduceXiangXunCaoItemCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        this.GetModel<XiangXunCaoModel>().ReduceItemNumber();
    }
}
/// <summary>
/// 增加香薰草数量Item
/// </summary>
public class AddXiangXunCaoItemCommand : AbstractCommand
{
    int number;

    public AddXiangXunCaoItemCommand(int number)
    {
        this.number = number;
    }
    protected override void OnExecute()
    {
        this.GetModel<XiangXunCaoModel>().AddItemNumber(number);
    }
}
/// <summary>
/// 初始化香薰草数量Command
/// </summary>
public class InitXiangXunCaoItemCommand : AbstractCommand
{
    int number;
    public InitXiangXunCaoItemCommand(int number)
    {
        this.number = number;
    }
    protected override void OnExecute()
    {
        this.GetModel<XiangXunCaoModel>().SetNumber(number);
    }
}
/// <summary>
/// 返回香薰草数据事件
/// </summary>
public class XiangXunCaoItemDataEvent : ItemDataEvent
{
    public Item item { get; set; }
}
