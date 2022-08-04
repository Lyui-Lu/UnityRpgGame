using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UnityEngine.UI;
/// <summary>
/// 物品信息面板管理类
/// </summary>
public class ItemMessageController : MonoBehaviour
{
    public static ItemMessageController instance;
    public GameObject messageF;
    public GameObject pickF;
    public GameObject consumableF;
    public Text itemName;
    public Text Describe;

    public Text exp;
    public Text attack;
    public Text speed;

    public Text hp;
    public Text mp;
    public Text minusAttack;
    private void Awake()
    {
        instance = this;
    }
    /// <summary>
    /// 设置Item的Message
    /// </summary>
    /// <param name="name"></param>
    /// <param name="describe"></param>
    /// <param name="attack"></param>
    /// <param name="speed"></param>
    public void SetMessage(string name,string describe,string attack,string speed,string exp,string hp,string mp,string minusAttack,ItemType type)
    {
        messageF.SetActive(true);
        if (type == ItemType.Fire || type == ItemType.Magic || type == ItemType.XiangXunCao)
        {
            pickF.SetActive(true);
            consumableF.SetActive(false);
            this.attack.text = attack;
            this.speed.text = speed;
            this.exp.text = exp;
        }
        if(type == ItemType.HP||type==ItemType.MP||type==ItemType.DownAttack)
        {
            pickF.SetActive(false);
            consumableF.SetActive(true);
            this.minusAttack.text = minusAttack;
            this.mp.text = mp;
            this.hp.text = hp;
        }
        itemName.text = name;
        Describe.text = describe;
    }
    /// <summary>
    /// 关闭Message面板
    /// </summary>
    public void EmptyMessage()
    {
        messageF.SetActive(false);
    }

}
