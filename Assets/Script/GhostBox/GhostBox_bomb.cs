using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GhostBox_PlayerData;
public class GhostBox_bomb : MonoBehaviour
{
    public HodingObjectData.HodingObjectType objectType;
    public float force;
    public Vector3 direction;
    new Rigidbody rigidbody;
    Rigidbody Rigidbody
    {
        get
        {
            if(!rigidbody)rigidbody = gameObject.AddComponent<Rigidbody>();
            return rigidbody;
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
    float lineLength = 0.3f; 
    void Update()
    {
        ShowMoveDirLine();
        SelfDestruct();    
    }
    public void Emission()
    {
        Rigidbody.mass = 0.5f;
        Rigidbody.AddForce(direction * force);
        ShowMoveDir = true;
    }
    public void SetLine(Vector3[] positions)
    {
        LineRenderer.positionCount = positions.Length;
        LineRenderer.SetPositions(positions);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(!ShowMoveDir)return;
        bool haveRigidbody = collision.gameObject.GetComponent<Rigidbody>();
        bool isBox = collision.gameObject.GetComponent<GhostBox_box>();
        if(haveRigidbody && isBox)
        {
            Camera.main.GetComponent<Shake>().ShakeObjectPosition();
            string Sound_Hit = "Sound_Hit_" + Random.Range(1, 7).ToString();
            Sound.Instance.PlaySoundTemp(Sound_Hit);
            string Particle_Hit = "GhostBox_FX_Explosion";
            ParticleLoader.Instance.PlayParticleTemp(Particle_Hit,collision.contacts[0].point, new Vector3(-90,0,0));
            Destroy(collision.gameObject);
            Destroy(gameObject);
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
    void SelfDestruct()
    {
        if(!rigidbody)return;
        if(rigidbody.IsSleeping())
        {
            string Particle_Hit = "GhostBox_FX_SelfExplosion";
            ParticleLoader.Instance.PlayParticleTemp(Particle_Hit,transform.position,new Vector3(-90,0,0));
            Destroy(gameObject);
        }
    }
}
