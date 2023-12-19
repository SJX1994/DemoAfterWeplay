using UnityEngine;
using System.Collections.Generic;
using Match3G_PlayerData;
using DG.Tweening;
using System.Linq;
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
    
}