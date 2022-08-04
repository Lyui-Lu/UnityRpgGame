using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UnityEngine.UI;
using UnityEngine.EventSystems;
/// <summary>
/// Item售卖Or使用UI
/// </summary>
public class UseOrSellController : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler
{
    public static UseOrSellController instance;
    public GameObject useSellF;

    ItemController controller;
    /// <summary>
    /// 是否显示UseSellF;
    /// </summary>
    bool isShow = false;
    /// <summary>
    /// 鼠标是否在当前UI上
    /// </summary>
    bool isUION = false;
    public void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        useSellF.transform.Find("Use").GetComponent<Button>().onClick.AddListener(UseBtnDown);
        useSellF.transform.Find("Sell").GetComponent<Button>().onClick.AddListener(SellBtnDown);
    }
    void Update()
    {
        if ((Input.GetMouseButtonDown(1)||Input.GetMouseButtonDown(0)) && isShow&&!isUION)
        {
            HideUI();
        }
    }
    /// <summary>
    /// 显示物品右键UI
    /// </summary>
    /// <param name="point"></param>
    public void ShowUI(Vector3 point,ItemController controller)
    {
        this.controller = controller;
        useSellF.transform.position = point+new Vector3(18,-18);
        useSellF.SetActive(true);
        isShow = true;
    }
    /// <summary>
    /// 关闭UI
    /// </summary>
    public void HideUI()
    {
        useSellF.SetActive(false);
        isShow = false;
    }
    void UseBtnDown()
    {
        AudioManager.instance.SetAudioToSpecial(SpecialAudio.ClickButton);
        controller.UseItem();
        HideUI();
    }
    void SellBtnDown()
    {
        AudioManager.instance.SetAudioToSpecial(SpecialAudio.ClickButton);
        controller.SellItem();
        HideUI();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isUION = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isUION = true;
    }

}
