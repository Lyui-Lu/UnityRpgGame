using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using UnityEngine.SceneManagement;
/// <summary>
/// 登录管理类
/// </summary>
public class LoginManager : MonoBehaviour, IController
{
    public InputField nameInputField;

    public Button startGameEnterBtn;

    public Button nameEnterBtn;
    
    void Start()
    {
        startGameEnterBtn.onClick.AddListener(StartGameBtnDown);
        nameEnterBtn.onClick.AddListener(NameEnterBtnDown);
    }
    /// <summary>
    /// 按下AccountBtn时
    /// </summary>
    void StartGameBtnDown()
    {
        //如果数据库中存储的名字的话
        if (PlayDataManager.instance.LoadData(FieldManager.Playername) != null)
        {
            this.SendCommand<LoadSceneCommandMain>();
            AudioManager.instance.SetAudioToGameBgm();
            return;
        }

        nameInputField.gameObject.SetActive(true);
        startGameEnterBtn.gameObject.SetActive(false);
    }
    void NameEnterBtnDown()
    {
        //输入不等于空
        if (nameInputField.text != null)
        {
            LoginData loginData = new LoginData(); //存储
            loginData.name = nameInputField.text;
            PlayDataManager.instance.SaveData(loginData, FieldManager.Playername);
            AudioManager.instance.SetAudioToGameBgm();
            this.SendCommand<LoadSceneCommandMain>();
        }
    }
    public IArchitecture GetArchitecture()
    {
        return GameFramework.Interface;
    }

}
public class LoadSceneCommandMain : AbstractCommand
{
    protected override void OnExecute()
    {
        this.GetModel<LoadSceneModel>().LoadScene(FieldManager.MainScene);
    }
}
public class LoadSceneModel : AbstractModel
{
    float targetValue;
    CoroutineUtility coroutineUtility;
    protected override void OnInit()
    {
        coroutineUtility = this.GetUtility<CoroutineUtility>();
    }
    public void LoadScene(string sceneConst)
    {
        
        coroutineUtility.Startcoroutine(AsyncLoadScene(sceneConst));
    }
    IEnumerator AsyncLoadScene(string sceneConst)
    {
        //异步加载场景
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneConst, LoadSceneMode.Single);
        //阻止加载完成时候自动切换
        async.allowSceneActivation = false;
        while (!async.isDone || targetValue != 1)
        {
            if (async.progress < 1)
            {
                targetValue = async.progress;
            }
            else
            {
                targetValue = 1;
            }
            if (targetValue >= 0.9f)
            {
                async.allowSceneActivation = true;
            }
             yield return null;
        }
       
    }
}
public class CoroutineUtility : MonoBehaviour, IUtility
{
    public Coroutine Startcoroutine(IEnumerator enumerator)
    {
        return StartCoroutine(enumerator);
    }

    public void Stopcoroutine(Coroutine coroutine)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        else
        {
            Debug.LogError("coroutine=null");
        }

    }

}
