using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour, IController
{
    public ParticleSystem mouseButtonDownEffect;
    /// <summary>
    /// 自动寻路组件
    /// </summary>
    NavMeshAgent agent;
    /// <summary>
    /// 鼠标右键点击射线返回
    /// </summary>
    RaycastHit raycastHit;
    /// <summary>
    /// 主角动画状态机 
    /// </summary>
    Animator animator;
    /// <summary>
    /// 是否正在移动
    /// </summary>
    bool isMove = false;
    /// <summary>
    /// 是否正在去采摘
    /// </summary>
    bool isPickUpTo = false;
    /// <summary>
    /// 是否正在去打开商店
    /// </summary>
    bool isTask = false;
    /// <summary>
    /// 是否要去砍敌人
    /// </summary>
    bool IsAttackEnemy=false;
    /// <summary>
    /// 玩家是否死亡
    /// </summary>
    bool isDie = false;

    bool isSkill = false;
    /// <summary>
    /// 采摘范围;
    /// </summary>
    float PickScope = 2f;
    /// <summary>
    /// NPC交互范围;
    /// </summary>
    float npcInteractiveScope = 2f;
    /// <summary>
    /// 攻击范围
    /// </summary>
    float attackScope = 2f;
    /// <summary>
    /// 攻击夹角
    /// </summary>
    float attackAngle = 60;
    /// <summary>
    /// 要去采摘的物品
    /// </summary>
    GameObject pickUpItem;
    /// <summary>
    /// 任务NPC
    /// </summary>
    GameObject Npc;
    /// <summary>
    /// model
    /// </summary>
    PlayerDataModel playerModel;
    MoveSpeedModel moveModel;

    public IArchitecture GetArchitecture()
    {
        return GameFramework.Interface;
    }
    void Awake()
    {
        this.RegisterEvent<PlayerDieEvent>(e =>
      {
          PlayerDieMethod();
      });
        this.RegisterEvent<EnemyDieEvent>(EnemyDieMethod);
        this.RegisterEvent<ReleaseDoubleAtkEvent>(ReleaseDoubleAtk);
    }

    private void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        animator = this.GetComponent<Animator>();
        playerModel=this.GetModel<PlayerDataModel>();
        moveModel = this.GetModel<MoveSpeedModel>();
        Npc = GameObject.Find("Npc");
        this.RegisterEvent<MoveSpeedEvent>(e =>
        {
            agent.speed = moveModel.MoveSpeed;
        });
    }

    private void Update()
    {
        MouseDownMove();
        if (isMove&&!isDie)
        {
            //到达移动位置，停下
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                agent.ResetPath();
                animator.SetBool(FieldManager.IsMove, false);
                animator.SetBool(FieldManager.IsIdle, true);
                isMove = false;
            }
            if (isTask) //前往交互任务面板
            {
                if (Vector3.Distance(this.transform.position, Npc.transform.position) < npcInteractiveScope)
                {
                    TaskManager.instance.OpenTaskPanel();
                    agent.ResetPath();
                    animator.SetBool(FieldManager.IsMove, false);
                    animator.SetBool(FieldManager.IsIdle, true);
                    isTask = false;
                    isMove = false;
                }
            }
            if (isPickUpTo)
            {
                //与要采摘物品的距离小于采摘距离
                if (Vector3.Distance(this.transform.position, pickUpItem.transform.position) < PickScope)
                {
                    pickUpItem.GetComponent<PickItemGameObkjectController>().PickMethod();
                    agent.ResetPath();
                    animator.SetBool(FieldManager.IsMove, false);
                    animator.SetBool(FieldManager.IsIdle, true);
                    isPickUpTo = false;
                    isMove = false;
                    pickUpItem = null;
                }
            }
            //处于追着砍人状态并且上次鼠标按下检测到的物品为Object
            if (IsAttackEnemy && raycastHit.transform.tag == FieldManager.EnemyTag)
            {
                //如果与敌人距离大于攻击距离
                if (Vector3.Distance(this.transform.position, raycastHit.transform.position) >= attackScope)
                {
                    agent.SetDestination(raycastHit.transform.position);
                    animator.SetBool(FieldManager.IsMove, true);
                    animator.SetBool(FieldManager.IsAttack, false);
                }
                else
                {
                    agent.ResetPath();
                    this.transform.LookAt(raycastHit.transform);
                    animator.SetBool(FieldManager.IsAttack, true);
                    animator.SetBool(FieldManager.IsMove, false);
                }
            }
        }
        if (isSkill)
        {
          if(animator.GetCurrentAnimatorStateInfo(0).IsName("Skill") && (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f))
            {
                isSkill = false;
                DoubleAttackAnimFinallyEvent();
            }
        }
    }
    /// <summary>
    /// 按下鼠标移动
    /// </summary>
    void MouseDownMove()
    {
        if (!isSkill&&Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject() && !isDie)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out raycastHit, 10000, ~(1 << 6 | 1 << 7)))
            {
                isPickUpTo = false;
                isTask = false;
                IsAttackEnemy = false;
                isMove = true;
                animator.SetBool(FieldManager.IsAttack, false);
                if (raycastHit.transform.tag == FieldManager.LandTag)
                {
                    mouseButtonDownEffect.transform.position = raycastHit.point;
                    mouseButtonDownEffect.time = 0;
                    mouseButtonDownEffect.Play();
                    agent.SetDestination(raycastHit.point);
                    animator.SetBool(FieldManager.IsMove, true);
                    animator.SetBool(FieldManager.IsIdle, false);
                }
                if (raycastHit.transform.tag == FieldManager.NPCTag)
                {
                    agent.SetDestination(raycastHit.point);
                    isTask = true;
                    animator.SetBool(FieldManager.IsMove, true);
                    animator.SetBool(FieldManager.IsIdle, false);
                }
                if (raycastHit.transform.tag == FieldManager.PickUpItemTag)
                {
                    agent.SetDestination(raycastHit.point);
                    isPickUpTo = true;
                    pickUpItem = raycastHit.transform.gameObject;
                    animator.SetBool(FieldManager.IsMove, true);
                    animator.SetBool(FieldManager.IsIdle, false);
                }
                //是否选择攻击AI
                if (raycastHit.transform.tag == FieldManager.EnemyTag)
                {
                    agent.SetDestination(raycastHit.transform.position);
                    animator.SetBool(FieldManager.IsMove, true);
                    animator.SetBool(FieldManager.IsIdle, false);
                    IsAttackEnemy = true;
                }
            }
        }
    }
    /// <summary>
    /// 播放攻击音效Event
    /// </summary>
    public void AttackAudioEvent()
    {
        AudioManager.instance.SetAudioToSpecial(SpecialAudio.Attack);

    }

    /// <summary>
    /// 敌人死亡回调方法
    /// </summary>
    /// <param name="e"></param>
    void EnemyDieMethod(EnemyDieEvent e)
    {
        if (e.gameObject== raycastHit.transform.gameObject)
        {
            isMove = false;
            IsAttackEnemy = false;
            animator.SetBool(FieldManager.IsAttack, false);
            animator.SetBool(FieldManager.IsIdle, true);
            return;
        }
        else
        {
            return;
        }
    }
    /// <summary>
    /// 玩家死亡方法
    /// </summary>
    void PlayerDieMethod()
    {
        agent.ResetPath();
        isDie = true;
        animator.SetBool(FieldManager.IsDie, true);
        AudioManager.instance.SetAudioToDieBgm();
    }
    /// <summary>
    /// 释放双连击
    /// </summary>
    void ReleaseDoubleAtk(ReleaseDoubleAtkEvent e)
    {
        isMove=false;
        isSkill = true;
        animator.SetBool(FieldManager.IsSkill, true);
    }
    /// <summary>
    /// 玩家攻击方法
    /// </summary>
    public void AttackMethod()
    {
        float distance = Vector3.Distance(transform.position, raycastHit.transform.position);//距离
        Vector3 norVec = transform.rotation * Vector3.forward;
        Vector3 temVec = raycastHit.transform.position - transform.position;
        float jiajiao = Mathf.Acos(Vector3.Dot(norVec.normalized, temVec.normalized)) * Mathf.Rad2Deg;//计算两个向量间的夹角
        if (distance < attackScope)
        {
            if (jiajiao <= attackAngle * 0.5f)
            {
                raycastHit.transform.GetComponent<IEnemy>().GetAttackMethod(playerModel.attack + playerModel.itemAttack);
            }
        }
    }
    /// <summary>
    /// 双连击动画攻击帧
    /// </summary>
    public void DoubleAttackAnimEvent()
    {
        for (int i = 0; i < EnemyDataManager.instance.enemyList.Count; i++)
        {
            float distance = Vector3.Distance(transform.position, EnemyDataManager.instance.enemyList[i].transform.position);//距离
            Vector3 norVec = transform.rotation * Vector3.forward;
            Vector3 temVec = EnemyDataManager.instance.enemyList[i].transform.position - transform.position;
            float jiajiao = Mathf.Acos(Vector3.Dot(norVec.normalized, temVec.normalized)) * Mathf.Rad2Deg;//计算两个向量间的夹角
            if (distance < attackScope*1.2f)
            {
                if (jiajiao <= attackAngle )
                {
                    EnemyDataManager.instance.enemyList[i].transform.
                        GetComponent<IEnemy>().GetAttackMethod(playerModel.attack+(playerModel.level*3)+ playerModel.itemAttack);
                }
            }
        }
    }
   /// <summary>
   /// 动画播放最后帧
   /// </summary>
    public void DoubleAttackAnimFinallyEvent()
    {
        animator.SetBool(FieldManager.IsSkill, false);
    }

}
/// <summary>
/// 移速Command
/// </summary>
public class MoveSpeedCommand : AbstractCommand
{
    float speedPercent;
    /// <summary>
    /// 持续时间
    /// </summary>
    float time;
    public MoveSpeedCommand(float speedPercent,float time)
    {
        this.speedPercent = speedPercent;
        this.time = time;
    }
    protected override void OnExecute()
    {
        this.GetModel<MoveSpeedModel>().QuickMoveSpeed(speedPercent,time);
    }
}
public class ReleaseDoubleAtkCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        this.GetModel<MoveSpeedModel>().ReleaseDoubleAtk();
    }
}

/// <summary>
/// 移速Model
/// </summary>
public class MoveSpeedModel : AbstractModel
{
    public float startMoveSpeed;
    public float MoveSpeed;
    Coroutine currentCoroutine;
    protected override void OnInit()
    {
        startMoveSpeed = 8;
        MoveSpeed = startMoveSpeed;
    }
    /// <summary>
    /// 增加移速方法
    /// </summary>
    /// <param name="speedPercent"></param>
    /// <param name="time"></param>
    public void QuickMoveSpeed(float speedPercent,float time)
    {
        if (MoveSpeed == startMoveSpeed)
        {
            MoveSpeed = MoveSpeed + (MoveSpeed * speedPercent);
            currentCoroutine = this.GetUtility<CoroutineUtility>().StartCoroutine(CountDownSpeedPercent(time));
        }
        else
        {
            this.GetUtility<CoroutineUtility>().Stopcoroutine(currentCoroutine);
            currentCoroutine = this.GetUtility<CoroutineUtility>().StartCoroutine(CountDownSpeedPercent(time));
        }
        this.SendEvent(new  MoveSpeedEvent());
    }
    /// <summary>
    /// 恢复移速
    /// </summary>
    public void RecoverMoveSpeed()
    {
        MoveSpeed = startMoveSpeed;
        this.SendEvent(new  MoveSpeedEvent());
    }

    public void ReleaseDoubleAtk()
    {
        this.SendEvent<ReleaseDoubleAtkEvent>();
    }
    IEnumerator CountDownSpeedPercent(float time)
    {
        yield return new WaitForSeconds(time);
        MoveSpeed = startMoveSpeed;
        RecoverMoveSpeed();
    }
   
}
/// <summary>
/// 设置移速事件
/// </summary>
public struct MoveSpeedEvent
{

}
/// <summary>
/// 暂停事件
/// </summary>
public struct StopMoveEvent
{

}
/// <summary>
/// 玩家死亡事件
/// </summary>
public struct PlayerDieEvent
{

}
