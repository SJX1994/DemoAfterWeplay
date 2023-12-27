using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Match3G_PlayerData;

public class Match3G_Manager_StepLight : MonoBehaviour
{
    [SerializeField]
    Match3G_StepLight redLight;
    [SerializeField]
    Match3G_StepLight redLight2;
    [SerializeField]
    Match3G_StepLight blueLight;
    [SerializeField]
    Match3G_StepLight blueLight2;
    public void SetLight(Match3G_Group whichGroup,int step)
    {
        if(whichGroup.groupType == Match3G_GroupInfo.GroupType.GroupA)
        {
            if(step == 0)
            {
                redLight.SetLight(false);
                redLight2.SetLight(false);
            }else if(step == 1)
            {
                redLight.SetLight(false);
                redLight2.SetLight(true);
            }else if(step == 2)
            {
                redLight.SetLight(true);
                redLight2.SetLight(true);
            }
        }else if(whichGroup.groupType == Match3G_GroupInfo.GroupType.GroupB)
        {
            if(step == 0)
            {
                blueLight.SetLight(false);
                blueLight2.SetLight(false);
            }else if(step == 1)
            {
                blueLight.SetLight(false);
                blueLight2.SetLight(true);
            }else if(step == 2)
            {
                blueLight.SetLight(true);
                blueLight2.SetLight(true);
            }
        }
        
    }
}
