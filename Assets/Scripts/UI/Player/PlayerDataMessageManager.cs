using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
/// <summary>
///用户属性管理类
/// </summary>
public class PlayerDataMessageManager : MonoBehaviour, IController
{
    public Text ID;
    public Text Level;
    public Text Attack;
    public Text Hp;
    public Text Mp;
    public Text Coin;
    public Text CurrentExp;
    public Text MaxExp;
    /// <summary>
    /// 用户数据模型
    /// </summary>
    PlayerDataModel model;

    void Start()
    {
        model = this.GetModel<PlayerDataModel>();
        this.RegisterEvent<InitPlayerData>(e =>
        {
            InitData();
        });
        this.RegisterEvent<UpdatePlayerData>(e =>
        {
            UpdateData();
        });
    }
    /// <summary>
    /// 初始化数据
    /// </summary>
    void InitData()
    {
        ID.text = model.id;
        UpdateData();
    }
    /// <summary>
    /// 更新用户数据
    /// </summary>
    public void UpdateData()
    {
        Level.text = model.level.ToString();
        Attack.text = (model.itemAttack + model.attack).ToString();
        Hp.text = model.hp.ToString();
        Mp.text = model.mp.ToString();
        Coin.text = model.coin.ToString();
        CurrentExp.text = model.exp.ToString();
        MaxExp.text = model.LevelUpexp.ToString();
    }
    private void OnApplicationQuit()
    {
        PlayDataManager.instance.SaveData
             (new PlayerData(model.level, model.attack, model.hp, model.mp, model.coin, model.exp), FieldManager.PlayerData);
        PlayDataManager.instance.SaveData(new MaxPlayerData
           (model.Maxattack, model.Maxhp, model.Maxmp, model.LevelUpexp), FieldManager.MaxPlayerData);
    }

    public IArchitecture GetArchitecture()
    {
        return GameFramework.Interface;
    }
}
/// <summary>
/// 初始化数据Command
/// </summary>
public class InitPlayerDataCommand : AbstractCommand
{
    public PlayerData playerData;
    public MaxPlayerData maxPlayerData;
    public LoginData loginData;
    public InitPlayerDataCommand(PlayerData playerData, MaxPlayerData maxPlayerData, LoginData loginData)
    {
        this.playerData = playerData;
        this.maxPlayerData = maxPlayerData;
        this.loginData = loginData;
    }
    protected override void OnExecute()
    {
        this.GetModel<PlayerDataModel>().SetPlayerData(playerData, maxPlayerData, loginData);
    }
}
/// <summary>
/// 更新用户数据Command
/// </summary>
public class UpdatePlayerDataCommand : AbstractCommand
{
    public PlayerData playerData;
    public UpdatePlayerDataCommand(PlayerData playerData)
    {
        this.playerData = playerData;
    }
    protected override void OnExecute()
    {
        this.GetModel<PlayerDataModel>().UpdataPlayerData(playerData);
    }
}
/// <summary>
/// 判断金币是否足够购买Command
/// </summary>
public class JudgeCoinEnough : AbstractCommand
{
    int coin;

    int id;
    public JudgeCoinEnough(int coin, int id)
    {
        this.coin = coin;
        this.id = id;
    }
    protected override void OnExecute()
    {
        this.GetModel<PlayerDataModel>().isCoinEnough(coin, id);
    }
}/// <summary>
/// 判断蓝量是否足够释放技能Command
/// </summary>
public class JudgeMpEnough : AbstractCommand
{
    int MP;
    public JudgeMpEnough(int Mp)
    {
        this.MP = Mp;
    }
    protected override void OnExecute()
    {
        this.GetModel<PlayerDataModel>().isMpEnough(MP);
    }
}
/// <summary>
/// 减伤Command
/// </summary>
public class MinusAttackCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        this.GetModel<PlayerDataModel>().MinusAttackMethod();

    }
}

/// <summary>
/// 玩家数据模型
/// </summary>
public class PlayerDataModel : AbstractModel
{
    public string id;
    #region 实时用户信息数据
    public int level;
    public int attack;
    public int hp;
    public int mp;
    public int coin;
    public int exp;
    #endregion
    #region 最大用户信息数据
    public int Maxattack;
    public int Maxhp;
    public int Maxmp;
    public int LevelUpexp;
    #endregion

    #region Item效果数据
    public int itemAttack;
    bool isminusAttack = false;
    Coroutine currentMinusAttack;
    #endregion
    protected override void OnInit()
    {

    }
    /// <summary>
    /// 初始化用户数据
    /// </summary>
    /// <param name="playerData"></param>
    /// <param name="loginData"></param>
    public void SetPlayerData(PlayerData playerData, MaxPlayerData maxPlayerData, LoginData loginData)
    {
        id = loginData.name;
        level = playerData.level;
        attack = playerData.attack;
        hp = playerData.hp;
        mp = playerData.mp;
        coin = playerData.Coin;
        exp = playerData.exp;
        Maxattack = maxPlayerData.attack;
        Maxhp = maxPlayerData.hp;
        Maxmp = maxPlayerData.mp;
        LevelUpexp = maxPlayerData.exp;
        this.SendEvent<LevelUpEvent>();
        this.SendEvent<InitPlayerData>();
    }
    /// <summary>
    /// 更新用户数据
    /// </summary>
    /// <param name="playerData"></param>
    public void UpdataPlayerData(PlayerData playerData)
    {

        itemAttack += playerData.attack;
        if (isminusAttack)
        {
            hp += playerData.hp / 2;
        }//如果减伤
        else
        {
            hp += playerData.hp;

        }
        mp += playerData.mp;
        coin += playerData.Coin;
        exp += playerData.exp;
        if (playerData.attack != 0)
        {
            this.GetUtility<CoroutineUtility>().Startcoroutine(CountDownAttack(playerData.attack));
        }//如果增加攻击力了
        if (exp >= LevelUpexp)
        {
            exp -= LevelUpexp;
            LevelUp();
            return;
        }//如果经验值达到升级经验
        if (hp <= 0)
        {
            hp = 0;
            this.SendEvent(new PlayerDieEvent());
        }//如果生命值小于等于0
        if (hp > Maxhp)
        {
            hp = Maxhp;
        }//如果生命值大于最高生命值
        if (mp > Maxmp)
        {
            mp = Maxmp;
        }//如果法力值大于最高法力值
        this.SendEvent<UpdatePlayerData>();
    }
    /// <summary>
    /// 是否可以购买判断方法
    /// </summary>
    /// <param name="buyCoin"></param>
    /// <param name="id"></param>
    public void isCoinEnough(int buyCoin, int id)
    {
        if (id == 0)
        {
            if (coin >= buyCoin)
            {
                this.SendEvent(new ConsumableCoinEnoughEvent { isEnough = true });
            }
            else
            {
                this.SendEvent(new ConsumableCoinEnoughEvent { isEnough = false });
            }
        }
        else
        {
            if (coin >= buyCoin)
            {
                this.SendEvent(new EquipCoinEnoughEvent { isEnough = true });
            }
            else
            {
                this.SendEvent(new EquipCoinEnoughEvent { isEnough = false });
            }
        }
    }/// <summary>
     /// 是否可以释放技能判断方法
     /// </summary>
     /// <param name="buyCoin"></param>
     /// <param name="id"></param>
    public void isMpEnough(int skillMp)
    {
        if (mp >= skillMp)
        {
            this.SendEvent(new EquipMpEnoughEvent { isEnough = true });
        }
        else
        {
            this.SendEvent(new EquipMpEnoughEvent { isEnough = false });
        }
    }
    /// <summary>
    /// 减伤方法
    /// </summary>
    public void MinusAttackMethod()
    {
        if (isminusAttack)
        {
            this.GetUtility<CoroutineUtility>().StopCoroutine(currentMinusAttack);
        }
        currentMinusAttack = this.GetUtility<CoroutineUtility>().StartCoroutine(CountDownMinusAttack());
    }
    /// <summary>
    /// 等级提升
    /// </summary>
    void LevelUp()
    {
        level++;
        attack += 6;
        Maxattack += 6;
        Maxhp += 23;
        Maxmp += 16;
        hp = Maxhp;
        mp = Maxmp;
        LevelUpexp = 300 + (level * 60); //300+等级*60     升到下一级需要的经验值
        this.SendEvent(new LevelUpEvent { level = level });
        this.SendEvent<UpdatePlayerData>();

    }
    /// <summary>
    /// 倒计时减少攻击力协程 
    /// </summary>
    /// <param name="attack"></param>
    /// <returns></returns>
    IEnumerator CountDownAttack(int attack)
    {
        yield return new WaitForSeconds(FieldManager.FireTimeOf);
        itemAttack -= attack;
        this.SendEvent<UpdatePlayerData>();
    }
    /// <summary>
    /// 倒计时减少攻击力协程 
    /// </summary>
    /// <param name="attack"></param>
    /// <returns></returns>
    IEnumerator CountDownMinusAttack(int timeof = FieldManager.MeatTimeOf)
    {
        isminusAttack = true;
        yield return new WaitForSeconds(timeof);
        isminusAttack = false;
        this.SendEvent<UpdatePlayerData>();
    }
}
public class SaveDataUtility : MonoBehaviour, IUtility
{
    /// <summary>
    /// 保存数据
    /// </summary>
    /// <param name="playerData">数据名称</param>
    public void SaveData(Data playerData, string str)
    {
        PlayDataManager.instance.SaveData(playerData, str);
    }
}
/// <summary>
/// 数据初始化事件结构体
/// </summary>
public struct InitPlayerData
{

}
/// <summary>
/// 数据初始化事件结构体
/// </summary>
public struct UpdatePlayerData
{

}
/// <summary>
/// 升级事件
/// </summary>
public struct LevelUpEvent
{
    public int level;
}
/// <summary>
/// 消耗品金币是否足够
/// </summary>
public struct ConsumableCoinEnoughEvent
{
    public bool isEnough;
}
/// <summary>
/// 装备金币是否足够
/// </summary>
public struct EquipCoinEnoughEvent
{
    public bool isEnough;
}
/// <summary>
/// 释放技能蓝量是否足够
/// </summary>
public struct EquipMpEnoughEvent
{
    public bool isEnough;
}
