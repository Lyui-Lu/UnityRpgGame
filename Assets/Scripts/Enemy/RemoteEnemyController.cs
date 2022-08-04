using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// 远程敌人管理类
/// </summary>
public class RemoteEnemyController : IEnemy, IController
{
    public GameObject attackEffect;


    public GameObject effectPoint;
    protected override void Awake()
    {
        base.Awake();
    }
    /// <summary>
    /// 攻击方法 //在攻击动画后调用
    /// </summary>
    public override void AttackMethod()
    {
     GameObject tempGo= ObjectPool.instance.GetPool(FieldManager.RemoteAttack);
        tempGo.GetComponent<RemoteEffect>().attack = attack;
        tempGo.transform.position = effectPoint.transform.position;
        tempGo.transform.rotation = effectPoint.transform.rotation;
    }
    /// <summary>
    /// 受到伤害
    /// </summary>
    /// <param name="attack"></param>
    public override void GetAttackMethod(int attack)
    {
        if (Hp > 0)
        {
            Hp -= attack;
            hpSlider.value = Hp / maxHp;
            if (Hp <= 0)
            {
                this.SendCommand(new UpdatePlayerDataCommand(new PlayerData(0, 0, 0, 0, coin, exp)));
                animator.SetBool(FieldManager.EnemyDie, true);
                Die();
            }
        }
    }
    /// <summary>
    /// 死亡方法
    /// </summary>
    protected override void Die()
    {
        //自身属性根据死亡次数来递增
        maxHp = maxHp * 1.25f;
        attack = attack * 1.1f;
        exp = exp + 5;
        coin = coin + 4;
        Hp = maxHp;
        base.Die();
        this.SendCommand(new RegisterTasksNumCommand(0, 1, 0, 0, 0));
        this.SendCommand(new EnemyDieCommand(this.gameObject));
        enemyStateController.enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponent<CapsuleCollider>().enabled = false;
        Invoke("Hide", 3f);
    }
    /// <summary>
    /// 隐藏Enemy
    /// </summary>
    void Hide()
    {
        mesh.SetActive(false);
        Invoke("Show", reShowTime);
    }
    /// <summary>
    /// 复活
    /// </summary>
    void Show()
    {
        hpSlider.gameObject.SetActive(true);
        hpSlider.value =1;
        this.transform.position = pointGo.transform.position;
        animator.SetBool(FieldManager.EnemyDie, false);
        animator.SetBool(FieldManager.EnemyIdle, true);
        mesh.SetActive(true);
        enemyStateController.SetNormalState();
        GetComponent<NavMeshAgent>().enabled = true;
        GetComponent<CapsuleCollider>().enabled = true;
    }
    /// <summary>
    /// 看向玩家（动画帧调用）
    /// </summary>
    public void LookAtPlayer()
    {
        this.transform.LookAt(player.transform);
    }
    public IArchitecture GetArchitecture()
    {
        return GameFramework.Interface;
    }
}
