using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraLookPlayer : MonoBehaviour
{
    /// <summary>
    /// 旋转速度
    /// </summary>
    public float rotateSpeed;
    /// <summary>
    /// 插值速度
    /// </summary>
    public float smoothTime;
    /// <summary>
    /// 摄像机根节点
    /// </summary>
    public GameObject cameraRoot;
    public Transform delectTrs;

    /// <summary>
    /// 主角
    /// </summary>
    GameObject rpgHeroPlayer;

    bool isRound = false;

    bool isOnUIDown = false;

    void Awake()
    {
        //获取主角
        rpgHeroPlayer = GameObject.Find(FieldManager.PlayerName);
        cameraRoot.transform.position = rpgHeroPlayer.transform.position;
        transform.LookAt(rpgHeroPlayer.transform.position);
        delectTrs.position = transform.position;
        delectTrs.rotation = transform.rotation;
        //获取偏移量
    }

    // Update is called once per frame
    void Update()
    {
        cameraRoot.transform.position = rpgHeroPlayer.transform.position;/*Vector3.SmoothDamp(cameraRoot.transform.position, rpgHeroPlayer.transform.position, ref currentVelocity, smoothTime);*/
        transform.LookAt(rpgHeroPlayer.transform.position);
        RotateAxis();

    }
    public void RotateAxis()
    {
        if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject())
        {
            isOnUIDown = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isOnUIDown = false;
        }
        if (Input.GetMouseButton(0) && Input.GetAxis("Mouse X") != 0&&!isOnUIDown)
        {
            delectTrs.RotateAround(rpgHeroPlayer.transform.position, Vector3.up, Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime);
            isRound = true;
        }
        if (isRound)
        {
            transform.position = Vector3.Lerp(transform.position, delectTrs.position, 0.15f);
            transform.rotation = Quaternion.Lerp(transform.rotation, delectTrs.rotation, 0.15f);
            if (Quaternion.Angle(transform.rotation, delectTrs.rotation) <= 0.1F && Vector3.Distance(transform.position, delectTrs.position) <= 2)
            {
                isRound = false;
            }
        }
    }
}
