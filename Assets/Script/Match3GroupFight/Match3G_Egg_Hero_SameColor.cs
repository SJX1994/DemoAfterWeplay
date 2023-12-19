using UnityEngine;
using System.Collections.Generic;
using Match3G_PlayerData;
using DG.Tweening;
using System.Linq;

public class Match3G_Egg_Hero_SameColor : Match3G_Egg_Hero
{
    public int number = 2;
    public struct SameColorAttackInfo
    {
        public Vector2 Info_posID;
        public int Info_number;
        public List<Match3G_Base> Info_targets;
        public SameColorAttackInfo(Vector2 posID, int number, List<Match3G_Base> targets)
        {
            Info_posID = posID;
            Info_number = number;
            Info_targets = targets;
        }
    }
    protected override void DoDifferentUsed(Vector2 posID, Match3G_GroupInfo.GroupType groupType)
    {
        base.DoDifferentUsed(posID, groupType);
        Match3G_Group group = null;
        SameColorAttackInfo sameColorAttackInfo = new(posID, number, new List<Match3G_Base>());
        switch (groupType)
        {
            case Match3G_GroupInfo.GroupType.GroupA:
                group = Match3G_GroupInfo.Game.GroupA;
                sameColorAttackInfo = group.Hero_SameColorAttack(sameColorAttackInfo);
                break;
            case Match3G_GroupInfo.GroupType.GroupB:
                group = Match3G_GroupInfo.Game.GroupB;
                sameColorAttackInfo = group.Hero_SameColorAttack(sameColorAttackInfo);
                break;
        }
        Effect(group, posID,sameColorAttackInfo);
    }
    void Effect(Match3G_Group group, Vector2 posID, SameColorAttackInfo sameColorAttackInfo)
    {
        // float duration = 0.5f;
        Vector3 posStart = group.bases.Where(x => x.posID == posID).FirstOrDefault().transform.position;
        for(int i = 0; i < sameColorAttackInfo.Info_targets.Count;i++)
        {
            Vector3 pos = sameColorAttackInfo.Info_targets[i].transform.position;
            pos.z -= 3f;
            GameObject obj = ParticleLoader.Instance.PlayParticleTemp("ParticleSystem_Hero_SameColorAttack", pos, Vector3.zero);
        }
        Camera.main.GetComponent<Shake>().ShakeObjectPosition();
    }
}