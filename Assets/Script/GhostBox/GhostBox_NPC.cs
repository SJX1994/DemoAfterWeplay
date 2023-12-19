using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GhostBox_PlayerData;
using DG.Tweening;
public class GhostBox_NPC : MonoBehaviour
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
    public NPC_Data.NPC_Type npcType = NPC_Data.NPC_Type.notReady;
    public GhostBox_map_block blockPaired;
    new Rigidbody rigidbody;
    Rigidbody Rigidbody
    {
        get
        {
            rigidbody = GetComponent<Rigidbody>();
            if(!rigidbody)
            {
                gameObject.GetComponent<BoxCollider>().enabled = true;
                rigidbody = gameObject.AddComponent<Rigidbody>();
            }
            return rigidbody;
        }
    }
    private LineRenderer lineRenderer;
    public LineRenderer LineRenderer
    {
        get
        {
            if(lineRenderer == null)lineRenderer = GetComponent<LineRenderer>();
            return lineRenderer;
        }
        set
        {
            lineRenderer = value;
        }
    }
    public void BeenKilled()
    {
        if(npcType == NPC_Data.NPC_Type.good)
        {
            Manager.Restart();
        }
        string Particle_Hit = "GhostBox_FX_BloodSplatter";
        ParticleLoader.Instance.PlayParticleTemp(Particle_Hit,transform.position, new Vector3(0f,0f,90f));
        if(TryGetComponent(out Rigidbody rigidbody))
        {
            rigidbody.isKinematic = true;
        }
        if(TryGetComponent(out Collider collider))
        {
            collider.enabled = false;
        }
        transform.DORotate(new Vector3(0f,360f,0f),1f).SetUpdate(true).OnComplete(()=>{
            blockPaired.OccupiedNpc = null;
            Destroy(gameObject);
        });
        
    }
    public void Win()
    {
        transform.DOLocalJump(transform.position,1f,1,1f);
    }
    Vector3 randomForce;
    float range = 1f;
    float moveForce = 3f;
    bool isMoving = true;
    public bool IsMoving
    {
        get
        {
            return isMoving;
        }
        set
        {
            isMoving = value;
        }
    } 
    bool isMovingPreview = false;
    void Update()
    {
        if(isMovingPreview)return;
        if(Rigidbody.IsSleeping() && isMoving)MovePreview();  
       
    }
    public void MovePreview()
    {
        if(!isMoving)return;
        randomForce = Vector3.zero;
        randomForce = GetRandomForce();
        LineRenderer.positionCount = 2;
        LineRenderer.SetPosition(0, transform.position);
        LineRenderer.SetPosition(1, transform.position + randomForce);
        isMoving = false;
        isMovingPreview = true;
    }
    public void Action()
    {
        LineRenderer.positionCount = 0;
        Rigidbody.AddForce(randomForce*25f);
        isMovingPreview = false;
    }
    private Vector3 GetRandomForce()
    {
        
        // 生成随机的移动方向
        Vector3 randomDirection = Random.insideUnitSphere * range;

        // 计算移动向量
        Vector3 moveVector = randomDirection.normalized * moveForce;

        return moveVector;
    }

    
}
