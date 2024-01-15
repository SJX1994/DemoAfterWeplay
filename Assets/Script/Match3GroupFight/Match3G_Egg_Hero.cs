using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Match3G_PlayerData;
using TMPro;
using Unity.Mathematics;
public class Match3G_Egg_Hero : MonoBehaviour
{
    public Match3G_Egg egg;
    Vector3 originalPos;
    TextMeshPro textMesh;
    TextMeshPro TextMesh 
    { 
        get 
        { 
            if (textMesh == null) 
                textMesh = GetComponentInChildren<TextMeshPro>();
            return textMesh; 
        } 
    }
    ParticleSystem particleSystem_selected;
    ParticleSystem ParticleSystem_selected
    { 
        get 
        { 
            if (particleSystem_selected == null) 
                particleSystem_selected = GetComponentInChildren<ParticleSystem>();
            return particleSystem_selected; 
        } 
    }
    public bool OnUsing{get;private set;}
    public LayerMask targetMask_hero;
    public LayerMask targetMask_base;
    bool inPlace = false;
    bool scaledDown = false;
    public bool ScaledDown
    {
        get
        {
            return scaledDown;
        }
    }   
    Tween TextTween;
    public virtual void Introduce()
    {
        ParticleSystem_selected.Stop();
        originalPos = egg.transform.position;
        originalPos.z -= 1.5f;
        OnUsing = false;
        DoScaleUp();
    }
    public virtual void Match3G_Egg_Hero_Using_Enter()
    {
        
        Debug.Log("Match3G_Egg_Hero_Using_Enter!");
    }
    public void Match3G_Egg_Hero_Update()
    {
        if(Input.GetMouseButtonDown(0) && inPlace)
        {
            TextMesh.color =  new Color(1,1,1,0);
            OnUse(Input.mousePosition);
            DoScaleDown();
        }    
    }
    public void Match3G_Egg_Hero_Using_Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Used(Input.mousePosition);
        }
    }
    void DoScaleUp()
    {
        string Level_complete_0 = "Match3G_wav/Level_complete_0";
        Sound.Instance.PlaySoundTemp(Level_complete_0);
        inPlace = false;
        scaledDown = false;
        float age = 1f;
        TextMesh.color = new Color(1,1,1,0);
        transform.DOMove(new Vector3(0,0,-1.5f),age).SetEase(Ease.OutBounce);
        Tween t = transform.DOScale(1.5f,age).SetEase(Ease.OutBounce);
        t.onComplete += () => 
        {
            TextTween = TextMesh.DOFade(1f,age);
            inPlace = true;
        };
        egg.CompletedLight.intensity = 10f;
        egg.CompletedLight.DOIntensity(0f,age*3);
        Match3G_GroupInfo.ShowMask = true;
        
    }
    public virtual void OnUse(Vector3 mousePosition)
    {
        if(!scaledDown)return;
        if(Match3G_GroupInfo.groupTurn != egg.groupType)
        {
            if(transform.TryGetComponent(out Shake shake))
            {
                shake.ShakeObjectPosition();
            }
            return;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hitUnit = Physics.Raycast(ray, out hit, Mathf.Infinity, targetMask_hero);
        if(!hitUnit)return;
        Match3G_Egg_Hero hero;
        hit.collider.transform.TryGetComponent(out hero);
        if(!hero)return;
        // if(egg.groupType != Match3G_GroupInfo.GroupType.GroupB)return;
        ParticleSystem_selected.Play();
        OnUsing = true;
    }
    void Used(Vector3 mousePosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // RaycastHit hit;
        RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, targetMask_base);
        // bool hitUnit = Physics.Raycast(ray, out hit, Mathf.Infinity, targetMask_base);
        if(hits.Length == 0)return;
        Match3G_Base base_3g;
        foreach (RaycastHit hit in hits)
        {
            hit.collider.transform.TryGetComponent(out base_3g);
            if(!base_3g)continue;
            if(base_3g.groupType != Match3G_GroupInfo.GroupType.GroupB && Match3G_GroupInfo.Game.automaticPlay)continue;
            if(base_3g.groupType != Match3G_GroupInfo.groupTurn)continue;
            if(base_3g.FindGrid(new int2(base_3g.posID)) == TileState.Freezed)continue;
            DoDifferentUsed(base_3g.posID,egg.groupType);
            egg.Show();
            OnUsing = false;
            transform.DOScale(0.1f,0.25f).SetEase(Ease.InBounce).onComplete += () => 
            {
                Destroy(gameObject);
                string Burning_wick = "Match3G_wav/Burning_wick";
                Sound.Instance.PlaySoundTemp(Burning_wick);
            };
        }
        Match3G_GroupInfo.CurrentGroup.Numerical.enegyMultiplier = 1;
    }
    protected virtual void DoScaleDown()
    {
        if(!inPlace)return;
        TextTween?.Kill();
        float age = 0.5f;
        transform.DOMove(originalPos,age).SetEase(Ease.InSine);
        transform.DOScale(0.5f,age).SetEase(Ease.InSine).onComplete += () => 
        {
            scaledDown = true;
        };
        egg.Hide();
        Match3G_GroupInfo.ShowMask = false;
        Match3G_GroupInfo.match3G_SavingData_temp.useHeroTimes += 1;
    }
    protected virtual void DoDifferentUsed(Vector2 posID,Match3G_GroupInfo.GroupType groupType)
    {
        // Debug.Log(posID);
    }
}
