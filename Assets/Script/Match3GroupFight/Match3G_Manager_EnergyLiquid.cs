using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using DG.Tweening;
using Match3G_PlayerData;
using UnityEngine.UIElements;
[System.Serializable]
public struct EnegyLiquid
{
    public Vector3 positionFrom;
    public Vector3 positionTo;
    public float value;
    public SkeletonAnimation skeletonAnimation;
}

public class Match3G_Manager_EnergyLiquid : MonoBehaviour
{
    [SerializeField]
    EnegyLiquid energyLiquid_Red;
    [SerializeField]
    EnegyLiquid energyLiquid_Blue;
    void Awake()
    {
        energyLiquid_Red.value = 0;
        energyLiquid_Blue.value = 0;
    }
    public void SetLiquidValue(Match3G_Group whichGroup)
    {
        if(whichGroup.groupType == Match3G_GroupInfo.GroupType.GroupA)
        {
            float MP_proportion = Match3G_Tool.Remap(whichGroup.Numerical.CurrentMP,0f,whichGroup.Numerical.maxMP,0f,1f);
            energyLiquid_Blue.value = MP_proportion;
            float positionValue = Match3G_Tool.Remap(MP_proportion,0,1,energyLiquid_Blue.positionFrom.y,energyLiquid_Blue.positionTo.y);
            energyLiquid_Blue.skeletonAnimation.transform.parent.position = new Vector3(energyLiquid_Blue.skeletonAnimation.transform.position.x,positionValue,energyLiquid_Blue.skeletonAnimation.transform.position.z);
        }else if(whichGroup.groupType == Match3G_GroupInfo.GroupType.GroupB)
        {
            float MP_proportion = Match3G_Tool.Remap(whichGroup.Numerical.CurrentMP,0f,whichGroup.Numerical.maxMP,0f,1f);
            energyLiquid_Red.value = MP_proportion;
            float positionValue = Match3G_Tool.Remap(MP_proportion,0f,1f,energyLiquid_Red.positionFrom.y,energyLiquid_Red.positionTo.y);
            energyLiquid_Red.skeletonAnimation.transform.parent.position = new Vector3(energyLiquid_Red.skeletonAnimation.transform.position.x,positionValue,energyLiquid_Red.skeletonAnimation.transform.position.z);
            energyLiquid_Red.skeletonAnimation.transform.GetComponent<Shake>().ShakeObjectScale(0.6f);
        }
    }
}
