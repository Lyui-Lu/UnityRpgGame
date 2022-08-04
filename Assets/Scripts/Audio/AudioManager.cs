using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 特殊效果音频枚举
/// </summary>
public enum SpecialAudio
{
    /// <summary>
    /// 攻击音效
    /// </summary>
    Attack,
    /// <summary>
    /// 点击UI效果
    /// </summary>
    ClickButton
}
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    /// <summary>
    /// 背景音Source
    /// </summary>
    public AudioSource bgmSource;
    /// <summary>
    /// 特殊音效Source
    /// </summary>
    public AudioSource specialSource;
    /// <summary>
    /// 登录界面Bgm
    /// </summary>
    public AudioClip loginBgm;
    /// <summary>
    /// 游戏界面Bgm
    /// </summary>
    public AudioClip gameBgm;
    /// <summary>
    /// 死亡Bgm
    /// </summary>
    public AudioClip dieBgm;
    /// <summary>
    /// 攻击Audio
    /// </summary>
    public AudioClip attackAudio;
    /// <summary>
    /// 点击UIAudio
    /// </summary>
    public AudioClip clickUIAudio;

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        bgmSource.clip = loginBgm;
        bgmSource.loop = true;
        bgmSource.Play();
        if (PlayerPrefs.HasKey(FieldManager.AudioData))
        {
            SetAudioVolume(PlayerPrefs.GetFloat(FieldManager.AudioData));
        }
    }

    /// <summary>
    /// 设置游戏背景声音为背景音
    /// </summary>
    public void SetAudioToGameBgm()
    {
        bgmSource.clip = gameBgm;
        bgmSource.Play();
    }
    /// <summary>
    /// 设置死亡声音为背景音
    /// </summary>
    public void SetAudioToDieBgm()
    {
        bgmSource.clip = dieBgm;
        bgmSource.Play();
    }
    /// <summary>
    /// 设置攻击音效到特殊音效Source
    /// </summary>
    public void SetAudioToSpecial(SpecialAudio e)
    {
        if (e == SpecialAudio.Attack)
        {
            specialSource.clip = attackAudio;
            specialSource.Play();
        }

        if (e == SpecialAudio.ClickButton)
        {
            specialSource.clip = clickUIAudio;
            specialSource.Play();
        }
    }   
    /// <summary>
    /// 设置音量
    /// </summary>
    /// <param name="value"></param>
    public void SetAudioVolume(float value)
    {
        bgmSource.volume = value;
        specialSource.volume = value;
    }

}