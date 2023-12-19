using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Match3G_PlayerData;
using DG.Tweening;
using System.Linq;
public class Match3G_Egg : MonoBehaviour
{
    public LayerMask targetMask;
    public Match3G_GroupInfo.GroupType groupType;
    public List<Match3G_Egg_Hero> heros = new();
    Match3G_Egg_Hero pairedHero;
    public Match3G_Egg_Hero PairedHero
    {
        get
        {
            return pairedHero;
        }
    }
    Light completedlight;
    public Light CompletedLight
    {
        get
        {
            if (completedlight == null)
                completedlight = GetComponentInChildren<Light>();
            return completedlight;
        }
    }
    Renderer eggRenderer;
    public Renderer EggRenderer
    {
        get
        {
            if (eggRenderer == null)
                eggRenderer = GetComponent<Renderer>();
            return eggRenderer;
        }
    }
    Tween hideTween;
    Tween showTween;
    Vector3 originalScale;
    private Vector3 mousePosition;
    private Vector3 offset;
    void Update()
    {
        // if(Input.GetMouseButton(0))
        // {
        //     KeepEggMove(Input.mousePosition);
        // }
    }
    public void HatchingCompleted()
    {
        Vector3 pos = transform.position;
        pos.z -= 1.5f;
        Match3G_Egg_Hero hero = heros[Random.Range(0,heros.Count)];
        pairedHero = Instantiate(hero,pos,Quaternion.Euler(Vector3.zero));
        pairedHero.egg = this;
        pairedHero.Introduce();
    }
    public void Show()
    {
        showTween?.Kill();
        EggRenderer.enabled = true;
        showTween = transform.DOScale(originalScale,0.5f).SetEase(Ease.OutSine);
    }
    public void Hide()
    {
        originalScale = transform.localScale;
        hideTween?.Kill();
        hideTween = transform.DOScale(0.1f,0.5f).SetEase(Ease.InSine);
        hideTween.onComplete += () => EggRenderer.enabled = false;
    }
    // 待启用
    void KeepEggMove(Vector3 mousePos)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hitUnit = Physics.Raycast(ray, out hit, Mathf.Infinity, targetMask);
        if(!hitUnit)return;
        Match3G_Egg egg;
        hit.collider.transform.TryGetComponent(out egg);
        if(!egg)return;
        if(groupType != Match3G_GroupInfo.GroupType.GroupB)return;
        mousePosition = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 FinalPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(FinalPos.x, transform.position.y, transform.position.z);
    }
}
