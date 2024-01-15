using UnityEngine;
using DG.Tweening;
using Match3G_PlayerData;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;

public class Match3G_Base : MonoBehaviour
{
    public Match3G_GroupInfo.GroupType groupType;
    public Vector2 posID;
    public Match3G_Unit pairedUnit;
    Tween tweenTurn;
    public Vector3 midPos;
    Match3G_Base tempBase;
    public Match3G_Base TempBase
    {
        get
        {
            return tempBase;
        }
        set
        {
            if(tempBase)Destroy(tempBase);
            tempBase = Instantiate(value,transform.position,Quaternion.Euler(Vector3.zero));
            tempBase.transform.localScale = new Vector3(1f, 1f, 0.25f);
        }
    }
    MeshRenderer meshRenderer;
    public MeshRenderer MeshRenderer
    {
        get
        {
            if(!meshRenderer)
            {
                meshRenderer = GetComponent<MeshRenderer>();
            }
            return meshRenderer;
        }
    }
    Tween tweenScale;
    GameObject spriteRenderers_basesCheckerboard;
    public GameObject SpriteRenderers_basesCheckerboard
    {
        get
        {
            if(!spriteRenderers_basesCheckerboard)
            {
                spriteRenderers_basesCheckerboard = transform.Find("BasesCheckerboard").gameObject;
            }
            return spriteRenderers_basesCheckerboard;
        }
    }
    GameObject outLine;
    public GameObject OutLine
    {
        get
        {
            if(!outLine)
            {
                outLine = transform.Find("OutLine").gameObject;
            }
            return outLine;
        }
    }
    public void SetCollider(bool value)
    {
       transform.GetComponent<BoxCollider>().enabled = value;
    }
    public TileState FindGrid(int2 posID)
    {
        if(groupType == Match3G_GroupInfo.GroupType.GroupA)
        {
            return Match3G_GroupInfo.Game.GroupA.Grid[posID.x,posID.y];
        }else if(groupType == Match3G_GroupInfo.GroupType.GroupB)
        {
            return Match3G_GroupInfo.Game.GroupB.Grid[posID.x,posID.y];
        }
        return TileState.None;
    }
    public void Display_BasesCheckerboard()
    {
        GameObject Base1 = SpriteRenderers_basesCheckerboard.transform.Find("Base1").gameObject;
        GameObject Base2 = SpriteRenderers_basesCheckerboard.transform.Find("Base2").gameObject;
        if((posID.x + posID.y) % 2 == 0)
        {
            Base1.SetActive(true);
            Base2.SetActive(false);
        }else
        {
            Base1.SetActive(false);
            Base2.gameObject.SetActive(true);
        }
       
    }
    public void MeshRendererSet(bool value)
    {
        tweenScale?.Kill();
        
        if(value)
        {
            MeshRenderer.enabled = value;
            transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
            tweenScale = transform.DOScale(new Vector3(1f, 1f, 0.25f), 0.25f).OnComplete(()=>{
                SpriteRenderers_basesCheckerboard.SetActive(true);
                OutLine.SetActive(true);
                
            });
        }else
        {
            transform.localScale = new Vector3(1f, 1f, 0.25f);
            tweenScale = transform.DOScale(new Vector3(0.0f, 0.0f, 0.0f), 0.25f).OnComplete(()=>{
                MeshRenderer.enabled = value;

            });
        }
    }
    public void Turn()
    {
        // gameObject.SetActive(true);
        int rangeMax = 0;
        int rangeMin = 0;
        if(groupType == Match3G_GroupInfo.GroupType.GroupA)
        {
            rangeMax = 11;
            rangeMin = 6;
            
        }else if(groupType == Match3G_GroupInfo.GroupType.GroupB)
        {
            rangeMax = 5;
            rangeMin = 0;
        }
        if(posID.y >= rangeMin && posID.y <= rangeMax)
        {
            MeshRendererSet(true);
            // SetCollider(true);
        }else
        {
            MeshRenderer.enabled = false;
            SpriteRenderers_basesCheckerboard.SetActive(false);
            OutLine.SetActive(false);
            // SetCollider(false);
        }
        Vector3 midPosTemp = midPos;
        midPosTemp.z -= 0.5f;
        transform.position = midPosTemp;
        transform.localScale = new Vector3(1, 1, 0.25f);
    }
    public void TurnBack()
    {
        int rangeMax = 0;
        int rangeMin = 0;
        if(groupType == Match3G_GroupInfo.GroupType.GroupA)
        {
            rangeMax = 11;
            rangeMin = 6;
            
        }else if(groupType == Match3G_GroupInfo.GroupType.GroupB)
        {
            rangeMax = 5;
            rangeMin = 0;
        }
        if(posID.y >= rangeMin && posID.y <= rangeMax)
        {
            MeshRenderer.enabled = true;
            SpriteRenderers_basesCheckerboard.SetActive(true);
            OutLine.SetActive(true);
            // SetCollider(true);
        }else
        {
            MeshRenderer.enabled = false;
            SpriteRenderers_basesCheckerboard.SetActive(false);
            OutLine.SetActive(false);
            // SetCollider(false);
        }
        // gameObject.SetActive(true);
        // MeshRenderer.enabled = true;
        transform.position = midPos;
        transform.localScale = new Vector3(1f, 1f, 0.25f);
    }
    public void BeenOccupied()
    {
        // gameObject.SetActive(false);
        // MeshRenderer.enabled = false;
        MeshRendererSet(false);
    }
    public void SwitchTurnClear()
    {
        if(TempBase)Destroy(TempBase.gameObject);
    }
}