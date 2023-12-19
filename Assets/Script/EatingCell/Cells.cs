using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
public class Cells : MonoBehaviour
{
    public UnityAction<Cell> event_whoWillWin;
    public Cell cell_Bad_temp;
    public Cell cell_Good_temp;
    public List<Cell> cell_Bad_Set = new();
    public List<Cell> cell_Good_Set = new();
    private Cell whoWillWin;
    private Cell WhoWillWin
    {
        get
        {
            return whoWillWin;
        }
        set
        {
            if(whoWillWin == value)return;
            whoWillWin = value;
            Invoke(nameof(ChangeCamera),1.5f);
           
        }
    }
    void Update()
    {
        if(cell_Bad_Set.Count == 0)
        {
            WhoWillWin = cell_Good_Set.OrderBy(x => x.transform.localScale.y).Last();
        }
    }
    public void CereatGoodCell(Vector3 creat_pos,int num)
    {
        for(int i = 0; i < num; i++)
        {
            CreatGoodCell(creat_pos);
        }
    }
    void CreatGoodCell(Vector3 pos)
    {
        Cell cell = Instantiate(cell_Good_temp,pos,Quaternion.identity).GetComponent<Cell>();
        cell.winner = false;
        cell.Active();
        cell_Good_Set.Add(cell);
    }
    public void CreatBadCells(Vector3 creat_pos,int num = 0)
    {
        if(num == 0)num = Random.Range(15, 25);
        for(int i = 0; i < num; i++)
        {
            CreatBadCell(creat_pos);
        }
    }
    void CreatBadCell(Vector3 pos)
    {
        
        Cell cell = Instantiate(cell_Bad_temp,pos,Quaternion.identity).GetComponent<Cell>();
        cell.Active();
        cell_Bad_Set.Add(cell);
    }
    void ChangeCamera()
    {
        event_whoWillWin?.Invoke(whoWillWin);
    }
}
