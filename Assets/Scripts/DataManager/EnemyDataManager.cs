using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 敌人数据管理类（懒得用MVC了）
/// </summary>

public class EnemyDataManager : MonoBehaviour
{
    public static EnemyDataManager instance;
    /// <summary>
    /// 敌人信息字典
    /// </summary>
    public Dictionary<int, EnemyDataModel> enemyDataList = new Dictionary<int, EnemyDataModel>();
    /// <summary>
    /// 敌人ID列表
    /// </summary>
    public List<int> enemyIDList = new List<int>();
    /// <summary>
    /// 敌人引用管理List
    /// </summary>
    public List<IEnemy> enemyList = new List<IEnemy>();

    void Awake()
    {
        instance = this;
        enemyList = new List<IEnemy>(FindObjectsOfType<IEnemy>());
    }
    /// <summary>
    /// 添加敌人引用
    /// </summary>
    public void AddEnemy(IEnemy enemy)
    {
        if(!enemyList.Contains(enemy))
        enemyList.Add(enemy);
    }
    
    /// <summary>
    /// 初始化敌人的数据
    /// </summary>
    /// <param name="enemyList"></param>
    /// <param name="enemyIdList"></param>
    public void InitEnemyData(EnemyData enemydata)
    {
        this.enemyDataList = enemydata.enemyList;
        this.enemyIDList = enemydata.enemyID;
        SetDataToEnemy();
    }
    /// <summary>
    /// 设置数据到Enemy()
    /// </summary>
    void SetDataToEnemy()
    {//遍历所有的敌人引用
        for (int i = 0; i < enemyList.Count; i++)
        { //如果敌人ID列表内有此敌人的ID数据
            if (enemyIDList.Contains(enemyList[i].id))
            {
                enemyList[i].SetEnemyData(enemyDataList[enemyList[i].id]);
            }
            else //不存在就把数据设置到里面
            {
                enemyIDList.Add(enemyList[i].id);
                enemyDataList.Add(enemyList[i].id, new EnemyDataModel(enemyList[i].maxHp, enemyList[i].attack, enemyList[i].exp, enemyList[i].coin));
            }
        }
    }
    
    /// <summary>
    /// 更新敌人的数据
    /// </summary>
    /// <param name="id"></param>
    /// <param name="enemy"></param>
    public void UpdateEnemyData(int id,EnemyDataModel enemyModel)
    {
        if (enemyIDList.Contains(id))
        {
            enemyDataList[id] = enemyModel;
        }
        else
        {
            enemyIDList.Add(id);
            enemyDataList.Add(id, enemyModel);
        }
    }
    void OnApplicationQuit()
    {
        PlayDataManager.instance.SaveData(new EnemyData(enemyDataList, enemyIDList), FieldManager.EnemyData);
    }
}