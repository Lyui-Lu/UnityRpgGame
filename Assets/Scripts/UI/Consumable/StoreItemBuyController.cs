using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// 消耗品商店购买按钮类
/// </summary>
public class StoreItemBuyController : MonoBehaviour, IController, IPointerEnterHandler,IPointerExitHandler
{
    public GameObject storeItemBtn;

    StoreItemModel model;
    /// <summary>
    /// 是否显示出来了
    /// </summary>
    bool isShow;
    /// <summary>
    /// 是否在当前UI上
    /// </summary>
    bool isOnUI;
    /// <summary>
    /// 金币是否足够
    /// </summary>
    bool isCoinEnough;
    void Awake()
    {
        this.RegisterEvent<ShowHideStoreItemBtnEvent>(ShowBtn);
        this.RegisterEvent<ConsumableCoinEnoughEvent>(isCoinEnoughMethod);
    }
    void Start()
    {
        model = this.GetModel<StoreItemModel>();
    }
    void Update()
    {
        if ((Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0)) && isShow && !isOnUI)
        {
            HideBtn();
        }
    }
    /// <summary>
    /// 按下购买Btn
    /// </summary>
    public void BuyButtonDown()
    {
        this.SendCommand(new JudgeCoinEnough(model.buyCoin,0));
        AudioManager.instance.SetAudioToSpecial(SpecialAudio.ClickButton);
    }
    /// <summary>
    /// Event事件(金币是否足够)
    /// </summary>
    /// <param name="e"></param>
    void isCoinEnoughMethod(ConsumableCoinEnoughEvent e)
    {
        isCoinEnough = e.isEnough;
        HideBtn();
        if (isCoinEnough)//如果金币够
        {
          this.SendCommand(new AddBackGroundItemCommand(model.itemType,1));
            this.SendCommand(new UpdatePlayerDataCommand(new PlayerData(0, 0, 0, 0, -model.buyCoin, 0)));
        }
        else//如果不够
        {
            HintManager.instance.ShowCoinScantry();
        }
    }
    /// <summary>
    /// 显示Btn
    /// </summary>
    /// <param name="e"></param>
    void ShowBtn(ShowHideStoreItemBtnEvent e)
    {
        storeItemBtn.SetActive(true);
        storeItemBtn.transform.position = e.point;
        isShow = true;
    }
    void HideBtn()
    {
        storeItemBtn.SetActive(false);
        isShow = false;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        isOnUI = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isOnUI = true;
    }
    public IArchitecture GetArchitecture()
    {
        return GameFramework.Interface;
    }

}
/// <summary>
/// 设置消耗品商店即将右键购买的物品信息
/// </summary>
public class SetCurrentBuyConsumableData : AbstractCommand
{
    ItemType itemtype;
    int buyCoin;
    Vector3 point;
    public SetCurrentBuyConsumableData(ItemType itemtype, int buycoin, Vector3 point)
    {
        this.itemtype = itemtype;
        this.buyCoin = buycoin;
        this.point = point;
    }
    protected override void OnExecute()
    {
        this.GetModel<StoreItemModel>().SetCurrentData(itemtype, buyCoin, point);
    }
}
/// <summary>
/// 购买消耗品Model
/// </summary>
public class StoreItemModel : AbstractModel
{
    /// <summary>
    /// 物品类型
    /// </summary>
    public ItemType itemType;
    /// <summary>
    /// 购买金币
    /// </summary>
    public int buyCoin;
    protected override void OnInit()
    {

    }
    /// <summary>
    /// 设置当前要购买的物品的信息
    /// </summary>
    public void SetCurrentData(ItemType itemType, int buyCoin, Vector3 point)
    {
        this.itemType = itemType;
        this.buyCoin = buyCoin;
        this.SendEvent(new ShowHideStoreItemBtnEvent { point = point });
    }
}
public class ShowHideStoreItemBtnEvent
{
    public Vector3 point;
}
