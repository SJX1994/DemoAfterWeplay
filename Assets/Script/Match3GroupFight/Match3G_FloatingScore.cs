using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using Match3G_PlayerData;
using UnityEngine.Events;
public class Match3G_FloatingScore : MonoBehaviour
{
    public UnityAction OnMoveComplete;
    TextMeshPro textMesh;
    TextMeshPro TextMesh 
    { 
        get 
        { 
            if (textMesh == null) 
                textMesh = GetComponent<TextMeshPro>();
            return textMesh; 
        } 
    }
    float age = 1f;
    Tween tween1;
    Tween tween2;
    Tween tween3;
    TrailRenderer trailRenderer;
    TrailRenderer TrailRenderer 
    { 
        get 
        { 
            if (trailRenderer == null) 
                trailRenderer = GetComponent<TrailRenderer>();
            return trailRenderer; 
        } 
    }
    public void Show (Vector3 positionFrom,Vector3 positionTo, int value,Match3G_Group_Numerical who,Color color)
	{
        TrailRenderer.startColor = Color.white;
        TrailRenderer.endColor = color;
        positionFrom.z -= 3f;
        positionTo.z -= 3f;
        transform.position = positionFrom;
        TextMesh.text = "能量+" + value.ToString();
        TextMesh.color = Color.blue + Color.white*0.6f;
        tween1 = transform.DOScale(1.5f,age/2).SetEase(Ease.OutBounce);
        tween2 = null;
        tween3 = null;
        tween1.OnComplete(() => {
            tween2 = transform.DOMove(positionTo,age/4).SetEase(Ease.InSine);
            tween3 = transform.DOScale(0.1f,age/4).SetEase(Ease.InSine);
            who.CurrentMP += value;
            who.Group.Shake.ShakeObjectScale();
        });
        Destroy(gameObject,age);
	}
    public void Show (Vector3 positionFrom,Vector3 positionTo, int value)
	{
        positionFrom.z -= 1.5f;
        positionTo.z -= 1.5f;
        transform.position = positionFrom;
        TextMesh.text = "能量+" + value.ToString();
        TextMesh.color = Color.blue + Color.white*0.3f;
        tween1 = transform.DOScale(1.5f,age/2).SetEase(Ease.OutBounce);
        tween2 = null;
        tween3 = null;
        tween1.OnComplete(() => {
            tween2 = transform.DOMove(positionTo,age*2).SetEase(Ease.InSine);
            tween3 = transform.DOScale(0.1f,age/4).SetEase(Ease.InSine);
        });
        Destroy(gameObject,age*4);
	}
    public Match3G_FloatingScore Show (Vector3 positionFrom,Vector3 positionTo, string describe,int value,Color color)
	{
        TrailRenderer.startColor = Color.white;
        TrailRenderer.endColor = color;
        positionFrom.z -= 3f;
        positionTo.z -= 3f;
        transform.position = positionFrom;
        TextMesh.text = describe + value.ToString();
        TextMesh.color = color;
        tween1 = transform.DOScale(1.5f,age/2).SetEase(Ease.OutBounce);
        tween2 = transform.DOMoveY(positionFrom.y + 1f,age/2).SetEase(Ease.OutSine);
        tween3 = null;
        tween1.OnComplete(() => {
            tween2 = transform.DOMove(positionTo,age/4).SetEase(Ease.InSine);
            tween3 = transform.DOScale(0f,age/4).SetEase(Ease.InSine);
            OnMoveComplete?.Invoke();
        });
        Destroy(gameObject,age*4);
        return this;
	}
    void OnCollisionEnter(Collision other)
    {
        // EnergyCollecting(other);
    }
    // 待启用
    void EnergyCollecting(Collision other)
    {
        if(other.gameObject.TryGetComponent<Match3G_Egg>(out Match3G_Egg egg))
        {
            tween1?.Kill();
            tween2?.Kill();
            tween3?.Kill();
            if(egg.groupType == Match3G_GroupInfo.GroupType.GroupA)
            {
                Match3G_GroupInfo.Game.GroupA.Numerical.CurrentMP += int.Parse(TextMesh.text);
                Match3G_GroupInfo.Game.GroupA.Shake.ShakeObjectScale();
            }else if (egg.groupType == Match3G_GroupInfo.GroupType.GroupB)
            {
                Match3G_GroupInfo.Game.GroupB.Numerical.CurrentMP += int.Parse(TextMesh.text);
                Match3G_GroupInfo.Game.GroupA.Shake.ShakeObjectScale();
            }
            Destroy(gameObject);
        }
    }
}
