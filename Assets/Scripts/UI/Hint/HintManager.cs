using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintManager : MonoBehaviour
{
    public static HintManager instance;
    /// <summary>
    /// 蓝量不足
    /// </summary>
    public GameObject mpScantry;
    /// <summary>
    /// 金币不足
    /// </summary>
    public GameObject coinScantry;
    private void Awake()
    {
        instance = this;
    }

    public void ShowMpScantry()
    {
        mpScantry.SetActive(true);
        Invoke("HideMpScantry", 2f);
    }
    void HideMpScantry()
    {
        mpScantry.SetActive(false);
    }
    public void ShowCoinScantry()
    {
        coinScantry.SetActive(true);
        Invoke("HideCoinScantry", 2f);
    }
    void HideCoinScantry()
    {
        coinScantry.SetActive(false);
    }
}
