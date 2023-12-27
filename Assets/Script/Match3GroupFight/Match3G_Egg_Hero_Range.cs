using UnityEngine;
using System.Collections.Generic;
using Match3G_PlayerData;
using DG.Tweening;
using System.Linq;
using Spine.Unity;
public class Match3G_Egg_Hero_Range : Match3G_Egg_Hero
{
    public int range = 2;
    public struct RangeAttackInfo
    {
        public Vector2 Info_posID;
        public int Info_range;
        public List<Match3G_Base> Info_targets;
        public RangeAttackInfo(Vector2 posID, int range, List<Match3G_Base> targets)
        {
            Info_posID = posID;
            Info_range = range;
            Info_targets = targets;
        }
    }
    SkeletonAnimation skeletonAnimation_hero;
    SkeletonAnimation SkeletonAnimation_hero
    {
        get
        {
            if (skeletonAnimation_hero == null)
                skeletonAnimation_hero = GetComponentInChildren<SkeletonAnimation>();
            return skeletonAnimation_hero;
        }
    }
    private Renderer spineRenderer;
    private Renderer SpineRenderer
    {
        get
        {
            if (spineRenderer == null)
                spineRenderer = SkeletonAnimation_hero.transform.GetComponent<Renderer>();
            return spineRenderer;
        }
    }
    private MaterialPropertyBlock spinePropertyBlock_OutLineAlpha;
    public override void Introduce()
    {
        base.Introduce();
        SkeletonAnimation_hero.AnimationState.SetAnimation(0, "attack", true);
        UpdateMatAlpha(0.6f);
    }
    protected override void DoScaleDown()
    {
        base.DoScaleDown();
        SkeletonAnimation_hero.AnimationState.SetAnimation(0, "idle", true);
        UpdateMatAlpha(0.0f);
    }
    public override void Match3G_Egg_Hero_Using_Enter()
    {
        base.Match3G_Egg_Hero_Using_Enter();
        SkeletonAnimation_hero.AnimationState.SetAnimation(0, "move", true);
        UpdateMatAlpha(1.0f);
    }
    protected override void DoDifferentUsed(Vector2 posID, Match3G_GroupInfo.GroupType groupType)
    {
        base.DoDifferentUsed(posID, groupType);
        Match3G_Group group = null;
        RangeAttackInfo rangeAttackInfo = new(posID, range, new List<Match3G_Base>());
        switch (groupType)
        {
            case Match3G_GroupInfo.GroupType.GroupA:
                group = Match3G_GroupInfo.Game.GroupA;
                rangeAttackInfo = group.Hero_RangeAttack(rangeAttackInfo);
                break;
            case Match3G_GroupInfo.GroupType.GroupB:
                group = Match3G_GroupInfo.Game.GroupB;
                rangeAttackInfo = group.Hero_RangeAttack(rangeAttackInfo);
                break;
        }
        Effect(group, posID,rangeAttackInfo);
    }
    void Effect(Match3G_Group group, Vector2 posID, RangeAttackInfo rangeAttackInfo)
    {
        float duration = 0.5f;
        Vector3 posStart = group.bases.Where(x => x.posID == posID).FirstOrDefault().transform.position;
        
        foreach (var item in rangeAttackInfo.Info_targets)
        {
            GameObject obj = ParticleLoader.Instance.PlayParticleTemp("ParticleSystem_Hero_RangeAttack", posStart, Vector3.zero);
            Vector3 posEnd = item.transform.position;
            posEnd.z -= 3f;
            if(posEnd.x < posStart.x)
            {
                posEnd.x -= 1f;
            }
            else
            {
                posEnd.x += 1f;
            }
            if(posEnd.y < posStart.y)
            {
                posEnd.y -= 1f;
            }
            else
            {
                posEnd.y += 1f;
            }
            obj.transform.DOMove(posEnd, duration).OnComplete(() =>
            {
                Destroy(obj, duration+0.2f);
            });
        }
        Camera.main.GetComponent<Shake>().ShakeObjectPosition();
    }
    public void UpdateMatAlpha(float SetFloat)
    {
        if (spinePropertyBlock_OutLineAlpha == null)
        {
            spinePropertyBlock_OutLineAlpha = new MaterialPropertyBlock();
        }
        if(SetFloat == spinePropertyBlock_OutLineAlpha.GetFloat("_SelectOutlineAlpha"))return;
        SpineRenderer.GetPropertyBlock(spinePropertyBlock_OutLineAlpha);
        spinePropertyBlock_OutLineAlpha.SetFloat("_SelectOutlineAlpha", SetFloat);
        SpineRenderer.SetPropertyBlock(spinePropertyBlock_OutLineAlpha);
    }
    
}