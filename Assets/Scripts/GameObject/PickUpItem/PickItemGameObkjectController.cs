using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
public class PickItemGameObkjectController : MonoBehaviour,IController
{
    public ItemType itemType;
    /// <summary>
    /// 被采摘掉
    /// </summary>
    public void PickMethod()
    {
        this.SendCommand(new AddBackGroundItemCommand(itemType,1));
        StartCoroutine(InvokeActive());
    }
    /// <summary>
    /// 120秒后重新开启自身
    /// </summary>
    /// <returns></returns>
    IEnumerator InvokeActive()
    {
        this.GetComponent<MeshRenderer>().enabled=false;
         this.GetComponent<BoxCollider>().enabled=false;
        yield return new WaitForSeconds(180);
        this.GetComponent<MeshRenderer>().enabled = true;
        this.GetComponent<BoxCollider>().enabled = true;
    }
    public IArchitecture GetArchitecture()
    {
        return GameFramework.Interface;
    }
}
public class AddBackGroundItemEvent
{
    public GameObject item;
}