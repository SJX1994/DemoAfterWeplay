using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;
using Match3G_PlayerData;
public class Match3G_Manager_Flow : MonoBehaviour
{
    Match3G_Manager game;
    public Match3G_Manager Game 
    { 
        get 
        { 
            if (game == null) 
                game = FindObjectOfType<Match3G_Manager>();
            return game; } 
    }
    private StateMachine<States, Driver> fsm;
    public StateMachine<States, Driver> FSM => fsm;
    public float roundTime = 9f;
    private float playStartTime;
    public float settlementTime_Total = 1.5f;
    private float settlementTime;
    public float soldiersGain_Total = 1.5f;
    private float soldiersGainTime;
    bool started = false;
    bool automaticPlay = false;
    public bool haveHero => Game.GroupB.Egg.PairedHero && !Game.GroupB.Egg.EggRenderer.enabled;
    bool heroUsing  
    {
        get{
            if(!Game.GroupB.Egg.PairedHero)return false;
            return Game.GroupB.Egg.PairedHero.OnUsing;
        }
    }
    private void Awake()
	{
		fsm = new StateMachine<States, Driver>(this);
		fsm.ChangeState(States.Start);
	}
    private void Update()
	{
		fsm.Driver.Update.Invoke();
	}

	void OnGUI()
	{
		fsm.Driver.OnGUI.Invoke(); 
	}
#region 有限状态机
    public enum States
	{
        Start,
        AddEnergy, // 主要循环中 添加能量
        HeroIntroduce, // 解锁英雄时
        HeroOnStage, // 英雄上场时
        SoldiersGain,// 我方增益结算
        Settlement, // 敌方伤害结算
		TurnSwitch, // 攻防切换
        Win, 
        Lose,
	}
    public class Driver
	{
		public StateEvent Update;
		public StateEvent OnGUI;
		public StateEvent<Item> OnItemSelected;
	}
    void Start_OnGUI()
	{
        GUIStyle myButtonStyle = new GUIStyle(GUI.skin.button);
        myButtonStyle.fontSize = 50;
		if (GUI.Button(new Rect(Screen.width/2 - 250, Screen.height/2, 500, 150), "2-Start",myButtonStyle))
		{
			fsm.ChangeState(States.AddEnergy);
		}
        if (GUI.Button(new Rect(Screen.width/2 - 250, Screen.height/2 - 250, 500, 150), "1-Start",myButtonStyle))
		{
            automaticPlay = true;
			fsm.ChangeState(States.AddEnergy);
		}
	}
    void AddEnergy_Enter()
	{
		playStartTime = Time.time;
        if(!started)
        {
            Game.automaticPlay = automaticPlay;
            Game.Match.Match_Start();
            started = true;
        }
	}
    void AddEnergy_OnGUI()
	{
        GUIStyle myTextStyle = new GUIStyle(GUI.skin.button);
        myTextStyle.fontSize = 90;
		int timeRemaining = (int)(roundTime - (Time.time - playStartTime));
		GUI.Label(new Rect(Screen.width/2 - 1500/2, Screen.height/2 - 70, 1500, 70), $"Time Remaining: {timeRemaining:n3}",myTextStyle);
	}
    void AddEnergy_Update()
	{
        if(automaticPlay && Game.GroupA.Numerical.CurrentHP <= 0 && !Game.IsBusy)
        {
            fsm.ChangeState(States.Win);
        }
        if(automaticPlay && Game.GroupB.Numerical.CurrentHP <= 0 && !Game.IsBusy)
        {
            fsm.ChangeState(States.Lose);
        }
        if(automaticPlay && Game.GroupB.Numerical.CurrentMP >= Game.GroupB.Numerical.maxMP && Game.GroupB.Egg.EggRenderer.enabled && !Game.IsBusy)
        {
            fsm.ChangeState(States.HeroIntroduce);
        }
        if(automaticPlay && haveHero && !Game.IsBusy)
        {
            Game.GroupB.Egg.PairedHero.Match3G_Egg_Hero_Update();
        }
        if(automaticPlay && heroUsing && !Game.IsBusy)
        {
            fsm.ChangeState(States.HeroOnStage);
        }
        
        Game.Match.Match_Update(); 
        if (Time.time - playStartTime < roundTime || Game.IsBusy)return;
        fsm.ChangeState(States.SoldiersGain);
		
	}
    void SoldiersGain_Enter()
    {
        soldiersGainTime = Time.time;
        Match3G_Group group = Game.WhichGroupTurn == Match3G_GroupInfo.GroupType.GroupA ? Game.GroupA : Game.GroupB;
        group.SoldiersGain();
        soldiersGain_Total = 0f;
        soldiersGain_Total += Game.WhichGroupTurn == Match3G_GroupInfo.GroupType.GroupA ? Game.GroupB.gainUnits.Count * 0.25f : Game.GroupA.gainUnits.Count * 0.25f;
        Debug.Log($"soldiersGain_Total:{soldiersGain_Total}");
    }
    void SoldiersGain_Update()
    {
        if (Time.time - soldiersGainTime < soldiersGain_Total)return;
        fsm.ChangeState(States.Settlement);
    }
    void HeroOnStage_Enter()
    {
        Game.GroupB.Egg.PairedHero.Match3G_Egg_Hero_Using_Enter();
    }
    void HeroOnStage_Update()
    {
        if(!Game.GroupB.Egg.PairedHero)return;
        Game.GroupB.Egg.PairedHero.Match3G_Egg_Hero_Using_Update();
        Game.Match.Match_Update();
        if(haveHero)return;
        fsm.ChangeState(States.AddEnergy);
    }
    void HeroIntroduce_Enter()
    {
        Game.GroupB.Numerical.CurrentMP = 0;
        Game.GroupB.Egg.HatchingCompleted();
        
    }
    void HeroIntroduce_Update()
    {
        if(!Game.GroupB.Egg.PairedHero)return;
        Game.GroupB.Egg.PairedHero.Match3G_Egg_Hero_Update();
        if(Game.GroupB.Egg.EggRenderer.enabled)return;
        fsm.ChangeState(States.AddEnergy);
    }
    void Settlement_Enter()
    {
        settlementTime = Time.time;
        Game.AttackSettlement();
    }
    void Settlement_Update()
    {
        if (Game.IsBusy)
        {
            Game.Match.Match_Update();
        }
        else
        {
            if (Time.time - settlementTime < settlementTime_Total)return;
            fsm.ChangeState(States.TurnSwitch);
        }
    }
    void TurnSwitch_Enter()
    {
        Game.SwitchTurn();
    }
    void TurnSwitch_Update()
    {
        if (Game.IsBusy)
        {
            Game.Match.Match_Update();
        }
        else
        {
            fsm.ChangeState(States.AddEnergy);
        }
    }
    void Win_Enter()
    {

    }
    void Win_OnGUI()
    {
        GUIStyle myTextStyle = new GUIStyle(GUI.skin.button);
        myTextStyle.fontSize = 290;
        GUI.Label(new Rect(Screen.width/2 - 1500/2, Screen.height/2 - 70, 1500, 370), $"Win",myTextStyle);
    }
    void Lose_Enter()
    {

    }
    void Lose_OnGUI()
    {
        GUIStyle myTextStyle = new GUIStyle(GUI.skin.button);
        myTextStyle.fontSize = 290;
        GUI.Label(new Rect(Screen.width/2 - 1500/2, Screen.height/2 - 70, 1500, 370), $"Lose",myTextStyle);
    }

#endregion 有限状态机
}
