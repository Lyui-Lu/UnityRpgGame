using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 远程子弹
/// </summary>
public class RemoteEffect : MonoBehaviour,IController
{
    public float attack;
    int destoryTime = 12;
    Coroutine coroutine;
    private void OnEnable()
    {
        coroutine=StartCoroutine(DestoryCoroutine());
    }
    void Update()
    {
        this.transform.Translate(Vector3.forward * 8.2f * Time.deltaTime);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == FieldManager.EnemyTag||other.tag == FieldManager.LandTag)
        {
            return;
        }
        if (other.tag == "Player")
        {
            StopCoroutine(coroutine);
             this.SendCommand(new UpdatePlayerDataCommand(new PlayerData(0, 0, (int)-attack, 0, 0, 0)));
            DestoryThis();

        }
        else
        {
            StopCoroutine(coroutine);
            DestoryThis();
        }
    }
    IEnumerator DestoryCoroutine()
    {
        yield return new WaitForSeconds(destoryTime);
        DestoryThis();
    }
    void DestoryThis()
    {
        ObjectPool.instance.SetPool(FieldManager.RemoteAttack, this.gameObject);
    }

    public IArchitecture GetArchitecture()
    {
        return GameFramework.Interface;
    }
}
