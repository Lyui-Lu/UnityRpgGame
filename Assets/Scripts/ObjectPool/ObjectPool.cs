using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;

    public GameObject PoolF;

    public Dictionary<string, Queue<GameObject>> objectPool = new Dictionary<string,  Queue<GameObject>>();
    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// 出池
    /// </summary>
    /// <param name="Key"></param>
    /// <returns></returns>
    public GameObject GetPool(string Key)
    {
        if (!objectPool.ContainsKey(Key))
        {
            Queue<GameObject> tempList = new Queue<GameObject>();
            for (int i = 0; i < 5; i++)
            {
                  GameObject Tempgo = Instantiate(Resources.Load<GameObject>(Key),PoolF.transform);
                if (Tempgo == null)
                {
                    Debug.LogError("无法生成此物品"+Key);
                }
                Tempgo.SetActive(false);
                tempList.Enqueue(Tempgo);
            }
            objectPool.Add(Key, tempList);
        }
        if (objectPool[Key].Count == 0)
        {
            for (int i = 0; i < 5; i++)
            {
                GameObject Tempgo = Instantiate(Resources.Load<GameObject>(Key), PoolF.transform);
                if (Tempgo == null)
                {
                    Debug.LogError("无法生成此物品" + Key);
                }
                Tempgo.SetActive(false);
                 objectPool[Key].Enqueue (Tempgo);
            }
        }
        GameObject go = objectPool[Key].Dequeue();
        go.SetActive(true);
        return go;
        
    }
    /// <summary>
    /// 入池
    /// </summary>
    /// <param name="Key"></param>
    /// <param name="go"></param>
    public void SetPool(string Key,GameObject go)
    {
        if (!objectPool.ContainsKey(Key))
        {
            Queue<GameObject> tempList = new Queue<GameObject>();
            go.SetActive(false);
            tempList.Enqueue(go);
            objectPool.Add(Key, tempList);
        }
        else
        {
            go.SetActive(false);
            objectPool[Key].Enqueue(go);
        }
    }
}
