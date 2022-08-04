using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
public class BackGroundManager : MonoBehaviour, IController
{
    public static BackGroundManager instance;
    public Button backGroundBtn;
    public Button exitBackGroundBtn;
    /// <summary>
    /// 背包背景(开关背包用这个玩意)
    /// </summary>
    public GameObject backGround;
    /// <summary>
    /// 背包物品的父物体
    /// </summary>
    public GameObject backGroundItemArea;
    /// <summary>
    /// 当前背包内Item数量Text
    /// </summary>
    public Text currentNumberText;
    /// <summary>
    /// 当前背包内Item最大数量Text
    /// </summary>
    public Text maxLimitText;
    BackGroundManagerModel model;
    void Awake()
    {
        instance = this;
        model = this.GetModel<BackGroundManagerModel>();
        this.RegisterEvent<InitBackGroundEvent>(e =>
      {
          CreateBackGroundItem();
      });
        this.RegisterEvent<UpdateNumberTextEvent>(e =>
        {
            UpdateNumberText();
        });
        this.RegisterEvent<AddItemEvent>(AddItemCreate);
        this.RegisterEvent<AddItemNumberEvent>(AddItemsCount);
    }
    void Start()
    {
        exitBackGroundBtn.onClick.AddListener(ExitBackGround);
        backGroundBtn.onClick.AddListener(ShowBackGround);
    }
    /// <summary>
    /// 显示背包按钮
    /// </summary>
    void ShowBackGround()
    {
         AudioManager.instance.SetAudioToSpecial(SpecialAudio.ClickButton);
        Time.timeScale = 0F;
        backGround.SetActive(true);
    }
    /// <summary>
    /// 退出背包按钮
    /// </summary>
    void ExitBackGround()
    {
         AudioManager.instance.SetAudioToSpecial(SpecialAudio.ClickButton);
        Time.timeScale = 1F;
        backGround.SetActive(false);
    }
    /// <summary>
    /// 背包内Item数量更新时
    /// </summary>
    /// <param name="e"></param>
    void UpdateNumberText()
    {
        currentNumberText.text = model.currentNumber.ToString();
        maxLimitText.text = model.maxLimit.ToString();
    }
    /// <summary>
    /// 初始化生成背包内存储的Item
    /// </summary>
    void CreateBackGroundItem()
    {

        for (int i = 0; i < model.BackGroundmodelList.Count; i++)
        {
            CreateUtility(model.BackGroundmodelList[i]);
            SetItemsNumberUtility(model.BackGroundmodelList[i], model.typeItemsNumber[model.BackGroundmodelList[i]]);
        }
    }
    /// <summary>
    /// 增加Item自身的数量
    /// </summary>
    void AddItemsCount(AddItemNumberEvent e)
    {
        AddItemsNumberUtility(e.type, e.number);
    }
    /// <summary>
    /// 增加背包中Item
    /// </summary>
    /// <param name="e"></param>
    void AddItemCreate(AddItemEvent e)
    {
        CreateUtility(e.type);
         SetItemsNumberUtility(e.type,1);
    }
    /// <summary>
    /// 设置Item自身的数量
    /// </summary>
    /// <param name="type"></param>
    /// <param name="number"></param>
    void SetItemsNumberUtility(ItemType type, int number)
    {
        if (type == ItemType.Fire)
        {
            this.SendCommand(new RegisterTasksNumCommand(0, 0, 0, 1, 0));
            this.SendCommand(new InitHuoLiItemCommand(number));
        }
        if (type == ItemType.Magic)
        {
            this.SendCommand(new RegisterTasksNumCommand(0, 0, 1, 0, 0));

            this.SendCommand(new InitMoLiItemCommand(number));
        }
        if (type == ItemType.XiangXunCao)
        {
            this.SendCommand(new RegisterTasksNumCommand(0, 0, 0, 0, 1));
            this.SendCommand(new InitXiangXunCaoItemCommand(number));
        }
        if (type == ItemType.HP)
        {
            this.SendCommand(new InitHpItemCommand(number));
        }
        if (type == ItemType.MP)
        {
            this.SendCommand(new InitMpItemCommand(number));
        }
        if (type == ItemType.DownAttack)
        {
            this.SendCommand(new InitMeatItemCommand(number));
        }
    }
    /// <summary>
    /// 增加Item数量
    /// </summary>
    /// <param name="type"></param>
    /// <param name="number"></param>
    void AddItemsNumberUtility(ItemType type, int number)
    {
        if (type == ItemType.Fire)
        {
            this.SendCommand(new RegisterTasksNumCommand(0, 0, 0, 1, 0));

            this.SendCommand(new AddHuoLiItemCommand(number));
        }
        if (type == ItemType.Magic)
        {
            this.SendCommand(new RegisterTasksNumCommand(0, 0, 1, 0, 0));

            this.SendCommand(new AddMoLiItemCommand(number));
        }
        if (type == ItemType.XiangXunCao)
        {
            this.SendCommand(new RegisterTasksNumCommand(0, 0, 0, 0, 1));

            this.SendCommand(new AddXiangXunCaoItemCommand(number));
        }
        if (type == ItemType.HP)
        {
            this.SendCommand(new AddHpItemCommand(number));
        }
        if (type == ItemType.MP)
        {
            this.SendCommand(new AddMpItemCommand(number));
        }
        if (type == ItemType.DownAttack)
        {
            this.SendCommand(new AddMeatItemCommand(number));
        }
    }
    /// <summary>
    /// 生成Item工具
    /// </summary>
    /// <param name="type"></param>
    void CreateUtility(ItemType type)
    {
        if (type == ItemType.Fire)
        {
            Instantiate(Resources.Load<GameObject>(FieldManager.FireItem), backGroundItemArea.transform);
        }
        if (type == ItemType.Magic)
        {
            Instantiate( Resources.Load<GameObject>(FieldManager.MagicItem), backGroundItemArea.transform);
        }
        if (type == ItemType.XiangXunCao)
        {
            Instantiate(Resources.Load<GameObject>(FieldManager.XiangXunCaoItem), backGroundItemArea.transform);
        }
        if (type == ItemType.HP)
        {
            Instantiate(Resources.Load<GameObject>(FieldManager.HpItem), backGroundItemArea.transform);
        }
        if (type == ItemType.MP)
        {
            Instantiate(Resources.Load<GameObject>(FieldManager.MpItem), backGroundItemArea.transform);
        }
        if (type == ItemType.DownAttack)
        {
            Instantiate(Resources.Load<GameObject>(FieldManager.MeatItem), backGroundItemArea.transform);
        }
    }
    private void OnApplicationQuit()
    {
        PlayDataManager.instance.SaveData
            (new BackGroundDataModel(model.BackGroundmodelList, 
            model.typeItemsNumber, model.maxLimit, model.currentNumber), FieldManager.BackGroundData);
    }
    public IArchitecture GetArchitecture()
    {
        return GameFramework.Interface;
    }
}
/// <summary>
/// 初始化背包Command
/// 
/// </summary>
public class InitBackGroundCommand : AbstractCommand
{
    BackGroundDataModel model;
    public InitBackGroundCommand(BackGroundDataModel model)
    {
        this.model = model;
    }
    protected override void OnExecute()
    {
        this.GetModel<BackGroundManagerModel>().Init(model);
    }
}
/// <summary>
/// 增加背包中Item
/// </summary>
public class AddBackGroundItemCommand : AbstractCommand
{
    ItemType model;
    int number;

    public AddBackGroundItemCommand(ItemType model, int number)
    {
        this.model = model;
        this.number = number;
    }
    protected override void OnExecute()
    {
        this.GetModel<BackGroundManagerModel>().AddItem(model, number);
    }
}
/// <summary>
/// 更改Item的数量之后回调
/// </summary>
public class UpdateBackGroundItemCommand : AbstractCommand
{
    ItemType model;
    int number;

    public UpdateBackGroundItemCommand(ItemType model, int number)
    {
        this.model = model;
        this.number = number;
    }
    protected override void OnExecute()
    {
        this.GetModel<BackGroundManagerModel>().UpdateItemNumber(model, number);
    }
}
/// <summary>
/// Item数量为0时回调
/// </summary>
public class RemoveBackGroundItemCommand : AbstractCommand
{
    ItemType model;

    public RemoveBackGroundItemCommand(ItemType model)
    {
        this.model = model;
    }
    protected override void OnExecute()
    {
        this.GetModel<BackGroundManagerModel>().RemoveItemNumber(model);
    }
}
public class BackGroundManagerModel : AbstractModel
{
    /// <summary>
    /// 当前背包内ItemList
    /// </summary>
    public List<ItemType> BackGroundmodelList = new List<ItemType>();

    public Dictionary<ItemType, int> typeItemsNumber = new Dictionary<ItemType, int>();
    /// <summary>
    /// 背包存储上限
    /// </summary>
    public int maxLimit;
    /// <summary>
    /// 当前背包存储数量
    /// </summary>
    public int currentNumber;
    protected override void OnInit()
    {

    }
    /// <summary>
    /// 初始化背包
    /// </summary>
    /// <param name="datamodel"></param>
    public void Init(BackGroundDataModel datamodel)
    {
        BackGroundmodelList = datamodel.itemTypes;
        typeItemsNumber = datamodel.itemsNumber;
        maxLimit = datamodel.maxLimit;
        currentNumber = datamodel.currentNumber;
        this.SendEvent<InitBackGroundEvent>();
        this.SendEvent(new UpdateNumberTextEvent());
    }
    /// <summary>
    /// 背包添加Itme到List中
    /// </summary>
    /// <param name="item"></param>s
    public void AddItem(ItemType item, int number)
    {//有此数据
        if(BackGroundmodelList.Contains(item))
        {
            typeItemsNumber[item] = typeItemsNumber[item] + number;
            this.SendEvent(new AddItemNumberEvent { type = item, number = number });
        }
        if (currentNumber < maxLimit)
        { //无此数据
            if (!BackGroundmodelList.Contains(item))
            {
                currentNumber++;
                BackGroundmodelList.Add(item);
                typeItemsNumber.Add(item, number);
                this.SendEvent(new AddItemEvent { type = item });
                this.SendEvent(new UpdateNumberTextEvent());
            }
        }
        else
        {
            //调用UI弹窗，提示无法加入背包(发送一个Event或Command 购买/采集失败)
        }
    }
    /// <summary>
    /// 更新Item的数量(物品使用或者出售时Command此方法)
    /// </summary>
    public void UpdateItemNumber(ItemType item, int number)
    {
        if (BackGroundmodelList.Contains(item) && typeItemsNumber.ContainsKey(item))
        {
            typeItemsNumber[item] = number;
          
        }
    }
    /// <summary>
    /// 删除背包中的Item
    /// </summary>
    public void RemoveItemNumber(ItemType item)
    {
        if (BackGroundmodelList.Contains(item) && typeItemsNumber.ContainsKey(item))
        {
            currentNumber--;
            typeItemsNumber.Remove(item);
            BackGroundmodelList.Remove(item);
            this.SendEvent(new UpdateNumberTextEvent());
           
        }
    }
}
/// <summary>
/// 当背包内存储的Item数量或最大数量改变时调用
/// </summary>
public struct UpdateNumberTextEvent
{
}
/// <summary>
/// 当背包内已经存储了此类Item时调用(在AddItem时)
/// </summary>
public struct AddItemNumberEvent
{
    public ItemType type;
    public int number;
}
/// <summary>
/// 当背包内没有存储了此类Item时调用(在AddItem时)
/// </summary>
public struct AddItemEvent
{
    public ItemType type;
}
/// <summary>
/// 初始化生成
/// </summary>
public struct InitBackGroundEvent
{

}