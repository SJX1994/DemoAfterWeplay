using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GhostBox_PlayerData;
using System.Linq;
public class GhostBox_box : MonoBehaviour
{
    GhostBox_map_block blockPair;
    public GhostBox_map_block BlockPair
    {
        get
        {
            return blockPair;
        }
        set
        {
            blockPair = value;
            blockPair.OccupiedBox = this;
        }
    }
    public Vector2 posId;
    public HodingObjectData.HodingObjectType objectType;
    bool showMoveDir = false;
    bool ShowMoveDir 
    {
        get 
        { 
            return showMoveDir; 
        } 
        set 
        { 
            showMoveDir = value; 
            if(showMoveDir)
            {
                LineRenderer.positionCount = 2;
            }else
            {
                LineRenderer.positionCount = 0;
            }
        }
    }
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
    LineRenderer lineRenderer;
    public LineRenderer LineRenderer
    {
        get
        {
            if(lineRenderer == null)
            {
                lineRenderer = GetComponent<LineRenderer>();
            }
            return lineRenderer;
        }
        set
        {
            lineRenderer = value;
        }
    }
    new Rigidbody rigidbody;
    Rigidbody Rigidbody
    {
        get
        {
            if(!rigidbody)
            {
                gameObject.GetComponent<BoxCollider>().enabled = true;
                rigidbody = gameObject.AddComponent<Rigidbody>();
            }
            return rigidbody;
        }
    }
    float lineLength = 0.3f; 
    public float force;
    public Vector3 direction;
    public void Emission()
    {
        Rigidbody.mass = 0.5f;
        Rigidbody.AddForce(direction * force);
        // Rigidbody.AddRelativeForce(direction* force,ForceMode.Impulse);
        
        ShowMoveDir = true;
    }
    void Update()
    {
        ShowMoveDirLine();
        FindPos();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(!ShowMoveDir)return;
        if(rigidbody.IsSleeping())return;
        if(collision.gameObject.GetComponent<Rigidbody>() != null)
        {
            Camera.main.GetComponent<Shake>().ShakeObjectPosition();
            string Sound_Hit = "Sound_Hit_" + Random.Range(1, 7).ToString();
            Sound.Instance.PlaySoundTemp(Sound_Hit);
        }
        GhostBox_NPC GhostBox_NPC = collision.gameObject.GetComponent<GhostBox_NPC>();
        if(GhostBox_NPC)
        {
            GhostBox_NPC.BeenKilled();
        }
    }
    void ShowMoveDirLine()
    {
        if(!ShowMoveDir)return;
        Vector3 velocityDirection = Rigidbody.velocity.normalized;
        float magnitude = Rigidbody.velocity.magnitude;
        Vector3 lineStart = transform.position;
        Vector3 lineEnd = lineStart + velocityDirection * lineLength * magnitude;
        lineRenderer.SetPositions(new Vector3[] { lineStart, lineEnd });
        if (Rigidbody.IsSleeping())ShowMoveDir = false;
    }
    public void SetLine(Vector3[] positions)
    {
        LineRenderer.positionCount = positions.Length;
        LineRenderer.SetPositions(positions);
    }
    void FindPos()
    {
        if(!rigidbody)return;
        if(rigidbody.IsSleeping())FindPosId();
    }
    void FindPosId()
    {
        posId = new Vector2(Mathf.RoundToInt(transform.position.x),Mathf.RoundToInt(transform.position.z));
        transform.position = new Vector3(posId.x, transform.position.y, posId.y);
        transform.rotation = Quaternion.Euler(Vector3.zero);
        GhostBox_map_block block = Manager.Map.blocks.Where(x => x.posId == posId).FirstOrDefault();
        if(!block)
        {
            Destroy(gameObject);
            return;
        }
        transform.localScale = Vector3.one;
        BlockPair = block;
    }
}
