using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
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
    void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Unit unit))
        {
            Manager.Blocks.CheckMerge();
            if(unit)Destroy(unit.gameObject);
        }
    }
}
