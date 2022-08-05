using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class CursorManager : MonoBehaviour
{
    public Texture2D normalCursor;
    public Texture2D attackCursor;
    public Texture2D pickUpCursor;

    public void Awake()
    {
        SetNormal();
    }
    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;
        if (Physics.Raycast(ray, out raycastHit, 10000,~(1 << 6|1 << 7)))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                SetNormal();
                return;
            }
            if (raycastHit.transform.tag == FieldManager.EnemyTag)
            {
                SetAttack();
            }
            else if (raycastHit.transform.tag == "PickUpItem"||raycastHit.transform.tag == "NPC")
            {
                SetPickUp();
            }
            else
            {
                SetNormal();
            }
        }
    }
    public void SetNormal()
    {
        Cursor.SetCursor(normalCursor, new Vector2(10, 5), CursorMode.Auto);
    }

    public void SetAttack()
    {
        Cursor.SetCursor(attackCursor, new Vector2(5, 2), CursorMode.Auto);
    }
    public void SetPickUp()
    {
        Cursor.SetCursor(pickUpCursor, new Vector2(5, 2), CursorMode.Auto);
    }
}
