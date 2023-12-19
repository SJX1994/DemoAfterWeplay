using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private Manager manager;
    public Manager Manager
    {
        get
        {
            if(manager == null)manager = FindObjectOfType<Manager>();
            return manager;
        }
        set
        {
            manager = value;
        }
    }
    public Vector2 posId;
    public Unit occupiedUnit;
    public Unit OccupiedUnit
    {
        get=>occupiedUnit;
        set
        {
            if(occupiedUnit == value)return;
            occupiedUnit = value;   
            Manager.Blocks.CheckMerge();
        }
    }
    public void PlayEffect(string name)
    {
        GameObject effect = Resources.Load<GameObject>(name);
        Instantiate(effect, transform);
        effect.transform.localPosition = Vector3.zero;
        effect.transform.localScale = Vector3.one + Vector3.one*0.3f;
    }
}
