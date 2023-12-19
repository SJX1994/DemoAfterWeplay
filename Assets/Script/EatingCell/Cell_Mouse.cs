using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell_Mouse : MonoBehaviour
{
    public Cell_Box cell_Box_temp;
    public LayerMask groundTargetMask;
    private Cell_Box currentCell_Box;
    void Update()
    {
        MouseButtonDown();
        MouseButtonUp();
    }
    void MouseButtonDown()
    {
       bool mouseButtonDown = Input.GetMouseButtonDown(0);
       if (!mouseButtonDown)return;
       Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
       RaycastHit hit;
       bool hitUnit = Physics.Raycast(ray, out hit, Mathf.Infinity, groundTargetMask);
       if(!hitUnit)return;
       CreatBox(hit.point);
       StartCoroutine(BoxFollowMouse());
    }
    void MouseButtonUp()
    {
        bool mouseButtonUp = Input.GetMouseButtonUp(0);
        if (!mouseButtonUp)return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hitUnit = Physics.Raycast(ray, out hit, Mathf.Infinity, groundTargetMask);
        if(!hitUnit){if(currentCell_Box)Destroy(currentCell_Box.gameObject);return;}
        if(!currentCell_Box){return;}
        currentCell_Box.Emission();
        currentCell_Box = null;
    }
    void CreatBox(Vector3 pos)
    {
        pos.y += 5.5f;
        Cell_Box cell_Box = Instantiate(cell_Box_temp,pos,Quaternion.identity).GetComponent<Cell_Box>();
        currentCell_Box = cell_Box;
    }
    IEnumerator BoxFollowMouse()
    {
        while (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool hitUnit = Physics.Raycast(ray, out hit, Mathf.Infinity, groundTargetMask);
            if(hitUnit)
            {
                Vector3 pos = hit.point;
                pos.y += 5.5f;
                currentCell_Box.transform.position = pos;
            }
            yield return new WaitForFixedUpdate();
        }
    }
}
