using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Match3G_PlayerData;
using UnityEngine.Events;

public class Match3G_Manager_Match : MonoBehaviour
{
    public UnityAction OnTurnSwitch;
    private Match3G_Manager game;
    public Match3G_Manager Game 
    { 
        get 
        { 
            if (game == null) 
                game = FindObjectOfType<Match3G_Manager>();
            return game; } 
    }
    float busyDuration{get=>Game.busyDuration;set=>Game.busyDuration = value;}
    Match3G_Group GroupA  => Game.GroupA; 
    Match3G_Group GroupB  => Game.GroupB;
    Match3G_GroupInfo.GroupType WhichGroupTurn{get=>Game.WhichGroupTurn;set=>Game.WhichGroupTurn = value;}
    bool IsBusy => Game.IsBusy;
    bool isDragging{get=>Game.isDragging;set=>Game.isDragging = value;}
    bool HasMatches => Game.HasMatches;
    Vector3 dragStart{get=>Game.dragStart;set=>Game.dragStart = value;}
    private float ai_waitTime = 3f;
    public float Ai_waitTime 
    { 
        get
        {
            if(GroupA.Numerical.CurrentHP <= GroupA.Numerical.maxHP / 8f)
            {
                return 0.1f;
            }
            if(GroupA.Numerical.CurrentHP <= GroupA.Numerical.maxHP / 4f)
            {
                return 1f;
            }
            if(GroupA.Numerical.CurrentHP <= GroupA.Numerical.maxHP / 2f)
            {
                return 2f;
            }
            return ai_waitTime;
        }
    }
    private bool ai_shouldWait = true;
    private float ai_timer = 0f;
    public void Match_Start()
    {
        busyDuration = 0f;
        GroupA.CreatUnits();
        GroupB.CreatUnits();
        // WhichGroupTurn = Random.Range(0, 2) == 0 ? Match3G_GroupInfo.GroupType.GroupA : Match3G_GroupInfo.GroupType.GroupB;
        WhichGroupTurn = Match3G_GroupInfo.GroupType.NotReady;
        WhichGroupTurn = Match3G_GroupInfo.GroupType.GroupB;
        Game.Mask.enabled = false;
    }
    public void Match_Update()
    {
        // if(!IsPlaying)return;
        if(!IsBusy)
        {
            HandleInput();
        }
		DoWork();
        
    }
    void HandleInput ()
	{
        if (Game.automaticPlay && WhichGroupTurn == Match3G_GroupInfo.GroupType.GroupA)
        {
            if (!ai_shouldWait)return;
            ai_timer += Time.deltaTime;
            if(ai_timer < Ai_waitTime)return;
            ai_shouldWait = false;
            ai_timer = 0f;
            GroupA.DoAutomaticMove();
            ai_shouldWait = true;
        }
        else if (!isDragging && Input.GetMouseButtonDown(0))
		{
			dragStart = Input.mousePosition;
			isDragging = true;
		}
		else if (isDragging && Input.GetMouseButton(0))
		{
			isDragging = EvaluateDrag(dragStart, Input.mousePosition);
		}
		else
		{
			isDragging = false;
		}
    }
    public bool EvaluateDrag (Vector3 start, Vector3 end)
	{
		if(WhichGroupTurn == Match3G_GroupInfo.GroupType.GroupA)
        {
            return GroupA.EvaluateDrag(start, end);
        }else
        {
            return GroupB.EvaluateDrag(start, end);
        }
        
	}
    public void DoWork () 
    {
        if (busyDuration > 0f)
		{
			GroupA.tileSwapper.Update();
            GroupB.tileSwapper.Update();
			busyDuration -= Time.deltaTime;
			if (busyDuration > 0f)return;
		}

		if (HasMatches)
		{
			ProcessMatches();
		}
		else if (GroupA.NeedsFilling || GroupB.NeedsFilling)
		{
			DropTiles();
		}
		// else if (!IsPlaying)
		// {
		// 	gameOverText.gameObject.SetActive(true);
		// }
    }
    void DropTiles()
    {
        if(WhichGroupTurn == Match3G_GroupInfo.GroupType.GroupA)
        {
            GroupA.DropTiles();
        }else if(WhichGroupTurn == Match3G_GroupInfo.GroupType.GroupB)
        {
            GroupB.DropTiles();
        }
    }
    void ProcessMatches ()
    {
        if(WhichGroupTurn == Match3G_GroupInfo.GroupType.GroupA)
        {
            GroupA.ProcessMatches();
        }else if(WhichGroupTurn == Match3G_GroupInfo.GroupType.GroupB)
        {
            GroupB.ProcessMatches();
        }
    }
    public void TakeTheTurn()
    {
        // GroupA.SwitchTurnClear();
        // GroupB.SwitchTurnClear();
        
        if(Match3G_GroupInfo.groupTurn == Match3G_GroupInfo.GroupType.GroupA)
        {
            GroupA.TakeTheTurn();
        }else if(Match3G_GroupInfo.groupTurn == Match3G_GroupInfo.GroupType.GroupB)
        {
            GroupB.TakeTheTurn();
        }
        OnTurnSwitch?.Invoke();
    }

}
