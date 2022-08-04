using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ConsumableStoreManager : MonoBehaviour
{
    public static ConsumableStoreManager instance;
    public GameObject consumableF;
    public GameObject MessageF;
    public Text itemName;
    public Text describe;
    public Text sellPrice;

    private void Awake()
    {
        instance = this;
    }
    /// <summary>
    /// 设置消耗品商店Message信息
    /// </summary>
    /// <param name="itemname"></param>
    /// <param name="describe"></param>
    /// <param name="sellprice"></param>
    public void SetMessage(string itemname,string describe,int sellprice)
    {
        this.itemName.text = itemname;
        this.describe.text = "描述:"+describe;
        this.sellPrice.text = "售价:"+sellprice.ToString();
        MessageF.SetActive(true);
    }
    /// <summary>
    /// 关闭消耗品商店Message信息
    /// </summary>
    public void CloseMessage()
    {
         AudioManager.instance.SetAudioToSpecial(SpecialAudio.ClickButton);
        MessageF.SetActive(false);
    }
    public void Show()
    {
         AudioManager.instance.SetAudioToSpecial(SpecialAudio.ClickButton);
        consumableF.SetActive(true);
        Time.timeScale = 0f;
    }
    public void Hide()
    {
        consumableF.SetActive(false);
        CloseMessage();
        Time.timeScale = 1f;
    }
}