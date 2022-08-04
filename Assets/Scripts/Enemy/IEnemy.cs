using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EnemyType
{
    Warrior,
    Master
}
/// <summary>
/// 敌人属性基类
/// </summary>
public abstract class  IEnemy :MonoBehaviour
{
    /// <summary>
    /// 怪物当前血量值
    /// </summary>
    protected float Hp;
    /// <summary>
    /// 怪物血量值
    /// </summary>
    public float maxHp;
    /// <summary>
    /// 攻击力
    /// </summary>
    public float attack;
    /// <summary>
    /// 攻击范围
    /// </summary>
    public int attackScope;
    /// <summary>
    /// 玩家
    /// </summary>
    public GameObject player;
    /// <summary>
    /// 复活点
    /// </summary>
    public GameObject pointGo;
    /// <summary>
    /// 敌人类型
    /// </summary>
    public EnemyType enemyType;
    /// <summary>
    /// 敌人唯一ID
    /// </summary>
    public int id;
    /// <summary>
    /// 自身Mesh
    /// </summary>
    public GameObject mesh;
    /// <summary>
    /// 重生时间
    /// </summary>
    public float reShowTime;
    /// <summary>
    /// 血条
    /// </summary>
    public Slider hpSlider;

    public Animator animator;

    public EnemyStateController enemyStateController;

    public GameObject hpCanvas;

    /// <summary>
    /// 击杀敌人获得的经验值
    /// </summary>
    public int exp;
    /// <summary>
    /// 击杀敌人获得的金币
    /// </summary>
    public int coin;

    protected virtual void Awake()
    {
        player = GameObject.FindWithTag("Player");
        hpSlider.transform.LookAt(Camera.main.transform);
        Hp = maxHp;
    }
    protected virtual void Update()
    {
        hpCanvas.transform.position = mesh.transform.position;
        LookAtCam();
    }
    /// <summary>
    /// 血量条看向摄像机
    /// </summary>
    void LookAtCam()
    {
        hpSlider.transform.rotation = Camera.main.transform.rotation;
    }
    /// <summary>
    /// 设置敌人属性
    /// </summary>
    /// <param name="enemydate"></param>
    public  void SetEnemyData(EnemyDataModel enemydate)
    {
        maxHp = enemydate.Hp;
        attack = enemydate.Attack;
        exp = enemydate.Exp;
        coin = enemydate.coin;
        Hp = maxHp;
    }
    /// <summary>
    /// 攻击方法
    /// </summary>
    public abstract void AttackMethod();
    /// <summary>
    /// 收到攻击方法
    /// </summary>
    public abstract void GetAttackMethod(int attack);

    protected virtual void Die()
    {
        hpSlider.gameObject.SetActive(false);
        EnemyDataManager.instance.UpdateEnemyData(id, new EnemyDataModel(maxHp, attack, exp, coin));
    }
}
public class EnemyDieCommand:AbstractCommand
{
    GameObject go;

    public EnemyDieCommand(GameObject go)
    {
        this.go = go;
    }

    protected override void OnExecute()
    {
        this.SendEvent(new EnemyDieEvent { gameObject = go });
    }
}
public class EnemyDieEvent
{
    public GameObject gameObject;
}

