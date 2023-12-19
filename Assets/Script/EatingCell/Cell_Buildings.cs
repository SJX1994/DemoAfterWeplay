using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cell_Buildings : MonoBehaviour
{
    public List<Cell_Building> cell_Buildings_Set = new();
    public List<Cell_Building> Cell_Buildings_Set 
    { 
        get
        {
            if(cell_Buildings_Set.Count == 0)GetAllBuildings();
            return cell_Buildings_Set;
        }
        
    }
    void GetAllBuildings()
    {
        cell_Buildings_Set = FindObjectsOfType<Cell_Building>().ToList();
    }
}
