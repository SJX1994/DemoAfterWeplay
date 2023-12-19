using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBox_mouse : MonoBehaviour
{
    private GhostBox_manager manager;
    public GhostBox_manager Manager
    {
        get
        {
            if(manager == null)manager = FindObjectOfType<GhostBox_manager>();
            return manager;
        }
        set
        {
            manager = value;
        }
    }
    public LayerMask groundTargetMask;
    void Update()
    {
        MouseButtonDown();
        MouseButtonUp();
    }
    void MouseButtonDown()
    {
        if(!Manager.Player)return;
        bool mouseButtonDown = Input.GetMouseButtonDown(0);
        if (!mouseButtonDown)return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hitUnit = Physics.Raycast(ray, out hit, Mathf.Infinity, groundTargetMask);
        if(!hitUnit)return;
        StartCoroutine(FollowMouse());
    }
    void MouseButtonUp()
    {
        if(!Manager.Player)return;
        bool mouseButtonUp = Input.GetMouseButtonUp(0);
        if(!mouseButtonUp)return;
        LineRenderer lr =  Manager.Player.LineRenderer;
        if(lr.positionCount < 2)return;
        Vector3 pointA = lr.GetPosition(0);
        Vector3 pointB = lr.GetPosition(1);
        float distance = Vector3.Distance(pointA,pointB);
        Vector3 direction = (pointB - pointA).normalized;
        Manager.Player.PlaceBox(distance*60,direction,pointA,pointB);
        Manager.Player.CreatBox();
        lr.positionCount = 0;
        string Sound_DrawTheBow = "Sound_DrawTheBow";
        Sound.Instance.PlaySoundTemp(Sound_DrawTheBow,1,0.2f);
    }   
    IEnumerator FollowMouse()
    {
        while (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool hitUnit = Physics.Raycast(ray, out hit, Mathf.Infinity, groundTargetMask);
            if(hitUnit)
            {
                Vector3 pos = hit.point;
                pos.y += Manager.Player.transform.position.y;
                Manager.Player.Point2Pos = pos;
            }
            yield return new WaitForFixedUpdate();
        }
    }
}
