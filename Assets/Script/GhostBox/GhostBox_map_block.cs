using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBox_map_block : MonoBehaviour
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
    public Vector2 posId;
    public GhostBox_box occupiedBox = null;
    public GhostBox_box OccupiedBox
    {
        get=>occupiedBox;
        set
        {
            if(occupiedBox == value)return;
            occupiedBox = value;
            CheckIsOccupied();
        }
    }
    public GhostBox_NPC occupiedNpc = null;
    public GhostBox_NPC OccupiedNpc
    {
        get=>occupiedNpc;
        set
        {
            if(occupiedNpc == value)return;
            occupiedNpc = value;
            CheckIsOccupied();
        }
    }
    private bool isOccupied = false;
    public bool IsOccupied
    {
        get
        {
            CheckIsOccupied();
            return isOccupied;
        }
        set
        {
            CheckIsOccupied();
            if(isOccupied == value)return;
            isOccupied = value;
        }
    }
    public void PlayEffect(string name)
    {
        GameObject effect = Resources.Load<GameObject>(name);
        Instantiate(effect, transform);
        effect.transform.localPosition = Vector3.zero;
        effect.transform.localScale = Vector3.one + Vector3.one*0.3f;
    }
    void CheckIsOccupied()
    {
        if(occupiedBox || occupiedNpc)
        {
            isOccupied = true;
        }else
        {
            isOccupied = false;
        }
    }
}
