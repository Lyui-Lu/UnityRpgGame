using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// 近战敌人管理类
/// </summary>
public class NearEnemyController : IEnemy,IController
{
    /// <summary>
    /// 攻击夹角
    /// </summary>
    public float attackAngle;
   
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Update()
    {
        base.Update();
    }
    public override void AttackMethod()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);//距离
        Vector3 norVec = transform.rotation * Vector3.forward ;//此处*5只是为了画线更清楚,可以不要
        Vector3 temVec = player.transform.position - transform.position;
        float jiajiao = Mathf.Acos(Vector3.Dot(norVec.normalized, temVec.normalized)) * Mathf.Rad2Deg;//计算两个向量间的夹角
        if (distance < attackScope)
        {
            if (jiajiao <= attackAngle * 0.5f)
            {
                this.SendCommand(new UpdatePlayerDataCommand(new PlayerData(0, 0, (int)-attack, 0, 0, 0)));
            }
        }
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
        this.SendCommand(new RegisterTasksNumCommand(1, 0, 0, 0, 0));
        this.SendCommand(new EnemyDieCommand(this.gameObject));
        enemyStateController.enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponent<CapsuleCollider>().enabled = false;     
        Invoke( "Hide",3f);
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
        hpSlider.value = 1;

        this.transform.position = pointGo.transform.position;
        enemyStateController.SetNormalState();
        mesh.SetActive(true);

        animator.SetBool(FieldManager.EnemyDie, false);
        animator.SetBool(FieldManager.EnemyIdle, true);
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
