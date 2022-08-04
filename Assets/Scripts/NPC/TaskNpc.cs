using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskNpc : MonoBehaviour
{
    public Text describe;
    void Start()
    {
        describe.transform.LookAt(Camera.main.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        describe.transform.rotation = Camera.main.transform.rotation;
    }
}
