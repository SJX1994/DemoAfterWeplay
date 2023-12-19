using UnityEngine;
using Match3G_PlayerData;
using System.Linq;
using DG.Tweening;
public class Match3G_Egg_Hero_Cross : Match3G_Egg_Hero
{
    public int verticallyCount = 0;
    public int horizontallyCount = 0;
    public struct CrossAttackInfo
    {
        public Vector2 Info_posID;
        public int Info_verticallyCount;
        public int Info_horizontallyCount;
        public int Info_leftCount;
        public int Info_rightCount;
        public int Info_upCount;
        public int Info_downCount;
        public CrossAttackInfo(Vector2 posID, int verticallyCount, int horizontallyCount, int leftCount, int rightCount, int upCount, int downCount)
        {
            this.Info_posID = posID;
            this.Info_verticallyCount = verticallyCount;
            this.Info_horizontallyCount = horizontallyCount;
            this.Info_leftCount = leftCount;
            this.Info_rightCount = rightCount;
            this.Info_upCount = upCount;
            this.Info_downCount = downCount;
        }
    }
    protected override void DoDifferentUsed(Vector2 posID, Match3G_GroupInfo.GroupType groupType)
    {
        base.DoDifferentUsed(posID, groupType);
        Match3G_Group group = null;
        CrossAttackInfo crossAttackInfo = new CrossAttackInfo(posID, verticallyCount, horizontallyCount,0,0,0,0);
        switch (groupType)
        {
            case Match3G_GroupInfo.GroupType.GroupA:
                group = Match3G_GroupInfo.Game.GroupA;
                crossAttackInfo = group.Hero_CrossAttack(crossAttackInfo);
                break;
            case Match3G_GroupInfo.GroupType.GroupB:
                group = Match3G_GroupInfo.Game.GroupB;
                crossAttackInfo = group.Hero_CrossAttack(crossAttackInfo);
                break;
        }
        Effect(group, posID,crossAttackInfo);
        
    }
    void Effect(Match3G_Group group, Vector2 posID,CrossAttackInfo crossAttackInfo)
    {
        string particleName = "ParticleSystem_Hero_CrossAttack";
        float duration = 0.5f;
        Vector3 posStart = group.bases.Where(x => x.posID == posID).FirstOrDefault().transform.position;
        Vector3 posUp = group.bases.Where(x => x.posID == posID + new Vector2(0,crossAttackInfo.Info_upCount)).FirstOrDefault().transform.position;
        Vector3 posDown = group.bases.Where(x => x.posID == posID - new Vector2(0,crossAttackInfo.Info_downCount)).FirstOrDefault().transform.position;
        Vector3 posLeft = group.bases.Where(x => x.posID == posID - new Vector2(crossAttackInfo.Info_leftCount,0)).FirstOrDefault().transform.position;
        Vector3 posRight = group.bases.Where(x => x.posID == posID + new Vector2(crossAttackInfo.Info_rightCount,0)).FirstOrDefault().transform.position;
        posStart.z -= 1.5f;
        posUp.z -= 1.5f;
        posDown.z -= 1.5f;
        posLeft.z -= 1.5f;
        GameObject obj_Up = ParticleLoader.Instance.PlayParticleTemp(particleName,posStart,Vector3.zero);
        GameObject obj_Down = ParticleLoader.Instance.PlayParticleTemp(particleName,posStart,Vector3.zero);
        GameObject obj_Left = ParticleLoader.Instance.PlayParticleTemp(particleName,posStart,Vector3.zero);
        GameObject obj_Right = ParticleLoader.Instance.PlayParticleTemp(particleName,posStart,Vector3.zero);
        posUp.y += 1.5f;
        posDown.y -= 1.5f;
        posLeft.x -= 1.5f;
        posRight.x += 1.5f;
        obj_Up.transform.DOMove(posUp,duration);
        obj_Down.transform.DOMove(posDown,duration);
        obj_Left.transform.DOMove(posLeft,duration);
        obj_Right.transform.DOMove(posRight,duration).onComplete += () => {
            Camera.main.GetComponent<Shake>().ShakeObjectPosition();
        };
        Destroy(obj_Up,duration+0.1f);
        Destroy(obj_Down,duration+0.1f);
        Destroy(obj_Left,duration+0.1f);
        Destroy(obj_Right,duration+0.1f);

    }
}