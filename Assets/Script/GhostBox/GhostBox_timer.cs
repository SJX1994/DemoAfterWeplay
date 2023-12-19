using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GhostBox_PlayerData;
using DG.Tweening;
public class GhostBox_timer : MonoBehaviour
{
    public HodingObjectData.HodingObjectType objectType;
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
    public void Emission()
    {
        Time.timeScale = 0.3f;
        Rigidbody.isKinematic = true;
        transform.DOScale(Vector3.zero,3.0f).SetUpdate(true).OnComplete(()=>{
            Time.timeScale = 1f;
            Destroy(gameObject);
        });
    }
}
