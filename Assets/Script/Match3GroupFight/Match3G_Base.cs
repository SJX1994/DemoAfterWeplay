using UnityEngine;
using DG.Tweening;
using Match3G_PlayerData;

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
    public void Turn()
    {
        
        gameObject.SetActive(true);
        transform.position = midPos;
        transform.localScale = new Vector3(1, 1, 0.25f);
    }
    public void TurnBack()
    {
        gameObject.SetActive(true);
        transform.position = midPos;
        transform.localScale = new Vector3(0.8f, 0.8f, 0.25f);
    }
    public void BeenOccupied()
    {
        gameObject.SetActive(false);
    }
    public void SwitchTurnClear()
    {
        if(TempBase)Destroy(TempBase.gameObject);
    }
}