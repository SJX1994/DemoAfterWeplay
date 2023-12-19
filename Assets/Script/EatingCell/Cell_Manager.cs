using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Cell_Manager : MonoBehaviour
{
    private Cell_Mouse cell_Mouse;
    public Cell_Mouse Cell_Mouse
    {
        get
        {
            if(cell_Mouse == null)cell_Mouse = FindObjectOfType<Cell_Mouse>();
            return cell_Mouse;
        }
        set
        {
            cell_Mouse = value;
        }
    }
    private Camera_Follower cell_Camera;
    public Camera_Follower Cell_Camera
    {
        get
        {
            if(cell_Camera == null)cell_Camera = FindObjectOfType<Camera_Follower>();
            return cell_Camera;
        }
        set
        {
            cell_Camera = value;
        }
    }
    private Cell_Buildings cell_Buildings;
    public Cell_Buildings Cell_Buildings
    {
        get
        {
            if(cell_Buildings == null)cell_Buildings = FindObjectOfType<Cell_Buildings>();
            return cell_Buildings;
        }
        set
        {
            cell_Buildings = value;
        }
    }
    private Cells cells;
    public Cells Cells
    {
        get
        {
            if(cells == null)cells = FindObjectOfType<Cells>();
            return cells;
        }
        set
        {
            cells = value;
        }
    }
    bool wined = false;
    // Start is called before the first frame update
    void Start()
    {
        List<Cell_Building> cellBuildingTemp = Cell_Buildings.Cell_Buildings_Set;
        Vector3 cellPos = cellBuildingTemp[Random.Range(0,cellBuildingTemp.Count)].transform.position;
        Cells.CreatBadCells(cellPos);
        Cells.event_whoWillWin += Event_whoWillWin;
    }
    void Event_whoWillWin(Cell cell)
    {
        Cell_Mouse.enabled = false;
        Cell_Camera.target = cell.transform;
    }
    // Update is called once per frame
    void Update()
    {
        if(wined)return;
        if(Cells.cell_Good_Set.Count == 1 && Cells.cell_Bad_Set.Count == 0)
        {
            wined = true;
            FindObjectOfType<Cell>().Win();
            Cell_Camera.distance = 16f;
            FindObjectsOfType<LineRenderer>().ToList().ForEach(x => x.enabled = false);
        }
    }
}
