using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SetPanelManager : MonoBehaviour
{   
    /// <summary>
    /// 确定退出游戏面板
    /// </summary>
    public GameObject quitGameBG;
    /// <summary>
    /// 设置面板
    /// </summary>
    public GameObject setPanelF;
    /// <summary>
    /// 开发者面板
    /// </summary>
    public GameObject mePanel;
    /// <summary>
    /// 设置面板开启按钮
    /// </summary>
    public Button setPanelBtn;
    /// <summary>
    /// 音量滑动条
    /// </summary>
    public Slider audioSlider;
    /// <summary>
    /// 音量Text
    /// </summary>
    public Text audioNumberText;
    /// <summary>
    /// 当前音量
    /// </summary>
    float currentNumber;

    private void Awake()
    {
        audioSlider.onValueChanged.AddListener(SliderEvent);
    }
    private void Start()
    {
        setPanelBtn.onClick.AddListener(OpenSetPanel);
        if (PlayerPrefs.HasKey(FieldManager.AudioData))
        { 
            audioSlider.value=currentNumber = PlayerPrefs.GetFloat(FieldManager.AudioData);
        }
        else
        {
             audioSlider.value=currentNumber = 1;
        }
    }

    void Update()
    {
          
    }
    /// <summary>
    /// 打开设置面板
    /// </summary>
    void OpenSetPanel()
    {
        AudioManager.instance.SetAudioToSpecial(SpecialAudio.ClickButton);
        setPanelF.SetActive(true);
        Time.timeScale = 0F;
    }
    public void CloseSetPanel()
    {
        AudioManager.instance.SetAudioToSpecial(SpecialAudio.ClickButton);
        setPanelF.SetActive(false);
        Time.timeScale = 1F;
    }
    /// <summary>
    /// 滑动条事件
    /// </summary>
    void SliderEvent(float a)
    {
       currentNumber=Mathf.Ceil(audioSlider.value*10);
        audioNumberText.text = ((int)currentNumber).ToString();
        currentNumber = currentNumber / 10;
        AudioManager.instance.SetAudioVolume(currentNumber);
    }
    /// <summary>
    /// 关闭退出界面，返回设置面板
    /// </summary>
    public void ReturnSetPanel()
    {
        AudioManager.instance.SetAudioToSpecial(SpecialAudio.ClickButton);
        quitGameBG.SetActive(false);
    }
    /// <summary>
    /// 按下退出游戏一级按钮
    /// </summary>
    public void QuitGameBtn()
    {
        AudioManager.instance.SetAudioToSpecial(SpecialAudio.ClickButton);
        quitGameBG.SetActive(true);
    }
    /// <summary>
    /// 彻底退出游戏
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
    /// <summary>
    /// 开发者面板开启
    /// </summary>
    public void OpenMePanel()
    {
        mePanel.SetActive(true);
    }
    /// <summary>
    /// 开发者面板关闭
    /// </summary>
    public void CloseMePanel()
    {
        mePanel.SetActive(false);
    }
    void OnApplicationQuit()
    {
         PlayerPrefs.SetFloat(FieldManager.AudioData, currentNumber);
    }
}