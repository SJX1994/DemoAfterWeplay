using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Match3G_PlayerData;
public class Match3G_Manager : MonoBehaviour
{
    public bool automaticPlay;
    public float busyDuration;
    public bool IsBusy => busyDuration > 0f;
    public Move PossibleMove
	{ get; private set; }
    public bool NeedsFilling
	{ get; private set; }
    public bool IsPlaying => IsBusy || PossibleMove.IsValid;
    Match3G_Manager_UI ui;
    public Match3G_Manager_UI UI 
    { 
        get 
        { 
            if (ui == null) 
                ui = FindObjectOfType<Match3G_Manager_UI>(true);
            return ui; } 
    }
    Match3G_Manager_Match match;
    public Match3G_Manager_Match Match 
    { 
        get 
        { 
            if (match == null) 
                match = FindObjectOfType<Match3G_Manager_Match>(true);
            return match; } 
    }
    Match3G_Manager_Flow flow;
    public Match3G_Manager_Flow Flow 
    { 
        get 
        { 
            if (flow == null) 
                flow = FindObjectOfType<Match3G_Manager_Flow>(true);
            return flow; } 
    }
    Match3G_Group_Numerical numerical;
    public Match3G_Group_Numerical Numerical 
    { 
        get 
        { 
            if(WhichGroupTurn == Match3G_GroupInfo.GroupType.GroupA)
            {numerical = GroupA.GetComponent<Match3G_Group_Numerical>();}
            else 
            if(WhichGroupTurn == Match3G_GroupInfo.GroupType.GroupB)
            {numerical = GroupB.GetComponent<Match3G_Group_Numerical>();}
            return numerical; 
        } 
    }
    Match3G_Manager_StepLight stepLight;
    public Match3G_Manager_StepLight StepLight 
    { 
        get 
        { 
            if (stepLight == null) 
                stepLight = FindObjectOfType<Match3G_Manager_StepLight>(true);
            return stepLight; } 
    }
    Match3G_Manager_Welcome welcome;
    public Match3G_Manager_Welcome Welcome 
    { 
        get 
        { 
            if (welcome == null) 
                welcome = FindObjectOfType<Match3G_Manager_Welcome>(true);
            return welcome; } 
    }

    public Match3G_GroupInfo.GroupType WhichGroupTurn 
    { 
        get 
        { 
            return Match3G_GroupInfo.groupTurn; 
        }
        set
        {
            if(value == Match3G_GroupInfo.groupTurn)return;
            Match3G_GroupInfo.groupTurn = value;
            Match.TakeTheTurn();
        }
    }
    Match3G_Manager_BootSystem bootSystem;
    public Match3G_Manager_BootSystem BootSystem 
    { 
        get 
        { 
            if (bootSystem == null) 
                bootSystem = FindObjectOfType<Match3G_Manager_BootSystem>(true);
            return bootSystem; } 
    }
    Match3G_Group groupA, groupB;
    public Match3G_Group GroupA 
    { 
        get 
        { 
            if (groupA == null) 
                groupA = FindObjectsOfType<Match3G_Group>().Where(x => x.groupType == Match3G_GroupInfo.GroupType.GroupA).FirstOrDefault();
            return groupA; 
        } 
    }
    public Match3G_Group GroupB 
    { 
        get 
        { 
            if (groupB == null) 
                groupB = FindObjectsOfType<Match3G_Group>().Where(x => x.groupType == Match3G_GroupInfo.GroupType.GroupB).FirstOrDefault();
            return groupB; 
        } 
    }
    private SpriteRenderer mask;
    public SpriteRenderer Mask 
    { 
        get 
        { 
            if (mask == null) 
                mask = transform.Find("Mask").GetComponent<SpriteRenderer>();
            return mask;
        } 
    }
    private Match3G_Manager_EnergyLiquid energyLiquid;
    public Match3G_Manager_EnergyLiquid EnergyLiquid 
    { 
        get 
        { 
            if (energyLiquid == null) 
                energyLiquid = FindObjectOfType<Match3G_Manager_EnergyLiquid>(true);
            return energyLiquid; 
        } 
    }
    private Match3G_Unit_OutLine outLine;
    public Match3G_Unit_OutLine OutLine 
    { 
        get 
        { 
            if (outLine == null) 
                outLine = FindObjectOfType<Match3G_Unit_OutLine>(true);
            return outLine; 
        } 
    }
    
    public bool isDragging;
    public Vector3 dragStart;
    public bool HasMatches => GroupA.HasMatches || GroupB.HasMatches;
    
    public void SwitchTurn()
    {
        if(WhichGroupTurn == Match3G_GroupInfo.GroupType.GroupA)
        {WhichGroupTurn = Match3G_GroupInfo.GroupType.GroupB;}
        else
        {WhichGroupTurn = Match3G_GroupInfo.GroupType.GroupA;}
    }
    public void AttackSettlement()
    {
        if(WhichGroupTurn == Match3G_GroupInfo.GroupType.GroupA)
        {GroupA.AttackSettlement();}
        else
        {GroupB.AttackSettlement();}
    }
    public void AttackSettlementByOccupied()
    {
        if(WhichGroupTurn == Match3G_GroupInfo.GroupType.GroupA)
        {GroupA.AttackSettlementByOccupied();}
        else
        {GroupB.AttackSettlementByOccupied();}
    }
}
