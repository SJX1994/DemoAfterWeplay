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
    public float roundTime_Temp = 0f;
    private float playStartTime;
    public float settlementTime_Total = 1.8f;
    private float settlementTime;
    public float soldiersGain_Total = 1.5f;
    private float soldiersGainTime;
    private float roundWaitTime;
    private float roundWaitTime_Total = 1.5f;
    bool started = false;
    public bool automaticPlay = false;
    public bool haveHero => Game.GroupB.Egg.PairedHero && !Game.GroupB.Egg.EggRenderer.enabled;
    bool heroUsing  
    {
        get{
            if(!Game.GroupB.Egg.PairedHero)return false;
            return Game.GroupB.Egg.PairedHero.OnUsing;
        }
    }
    private float gameStartTime;
    private void Awake()
	{
        string game_map_music = "Match3G_wav/game_map_music";
        Sound.Instance.PlayMusicSimple(game_map_music);
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
        SoldiersGain,// 我方增益结算 // 弃用 改为操作后结算
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
    void Start_Enter()
    {
        Game.Welcome.ShowPage();
        Game.Match.HidePage();
        Game.UI.SavedData.gameObject.SetActive(true);
        Match3G_GroupInfo.match3G_SavingData_temp = new("", 0, 0, 0, 0, 0f);
        Match3G_GroupInfo.match3G_SavingData_round_red = new("", 0, 0, 0, 0, 0f);
        Match3G_GroupInfo.match3G_SavingData_round_blue = new("", 0, 0, 0, 0, 0f);
        gameStartTime = Time.time;
        Time.timeScale = 1;

    }
    void Start_OnGUI()
	{
        GUIStyle myButtonStyle = new GUIStyle(GUI.skin.button);
        myButtonStyle.fontSize = 50;
        started = false;
		// if (GUI.Button(new Rect(Screen.width/2 - 250, Screen.height/2, 500, 150), "双人同屏",myButtonStyle))
		// {
        //     Match3G_GroupInfo.playMode = Match3G_GroupInfo.PlayMode.TwoPlayer;
		// 	fsm.ChangeState(States.AddEnergy);
        //     string Button = "Match3G_wav/Button";
        //     Sound.Instance.PlaySoundTemp(Button);
		// }
        // if (GUI.Button(new Rect(Screen.width/2 - 250, Screen.height/2 - 250, 500, 150), "单人闯关(困难)",myButtonStyle))
		// {
        //     automaticPlay = true;
        //     Match3G_GroupInfo.playMode = Match3G_GroupInfo.PlayMode.Hard;
		// 	fsm.ChangeState(States.AddEnergy);
        //     string Button = "Match3G_wav/Button";
        //     Sound.Instance.PlaySoundTemp(Button);
		// }
        // if (GUI.Button(new Rect(Screen.width/2 - 250, Screen.height/2 - 500, 500, 150), "单人闯关(简单)",myButtonStyle))
		// {
        //     automaticPlay = true;
        //     Match3G_GroupInfo.playMode = Match3G_GroupInfo.PlayMode.Easy;
		// 	fsm.ChangeState(States.AddEnergy);
        //     string Button = "Match3G_wav/Button";
        //     Sound.Instance.PlaySoundTemp(Button);
		// }
	}
    void AddEnergy_Enter()
	{
		playStartTime = Time.time;
        roundTime_Temp = roundTime;
        Match3G_GroupInfo.Game.BootSystem.ResetBoot();
        if(!started)
        {
            started = true;
            Game.Match.ShowPage();
            Game.Welcome.HidePage();
            Game.automaticPlay = automaticPlay;
            Match3G_GroupInfo.ShowMask = false;
            Game.Match.Match_Start();
            Game.GroupA.Numerical.CurrentMP = 0;
            Game.GroupA.Numerical.CurrentAC = 0;
            Game.GroupA.Numerical.CurrentHP = Game.GroupB.Numerical.maxHP;
            Game.GroupB.Numerical.CurrentMP = 0;
            Game.GroupB.Numerical.CurrentAC = 0;
            Game.GroupB.Numerical.CurrentHP = Game.GroupB.Numerical.maxHP;
            Game.GroupA.Numerical.CurrentStep = 0;
            Game.GroupB.Numerical.CurrentStep = Game.GroupB.Numerical.maxStep;
            Match3G_GroupInfo.round = 0;
            Game.UI.Round.RoundSwitch(Match3G_GroupInfo.CurrentGroup);
            Game.UI.SavedData.gameObject.SetActive(false);
            string game_music = "Match3G_wav/game_music";
            Sound.Instance.PlayMusicSimple(game_music);
        }
	}
    void AddEnergy_OnGUI()
	{
        // GUIStyle myTextStyle = new GUIStyle(GUI.skin.button);
        // myTextStyle.fontSize = 90;
		// int timeRemaining = (int)(roundTime - (Time.time - playStartTime));
        // if(timeRemaining < 0)timeRemaining = 0;
		// GUI.Label(new Rect(Screen.width/2 - 1500/2, Screen.height/2 - 70, 1500, 70), $"Time Remaining: {timeRemaining:n3}",myTextStyle);
        float timeRemainingF = roundTime_Temp - (Time.time - playStartTime);
        Game.UI.Timer.UpdateTimer(timeRemainingF,roundTime_Temp);
        
	}
    void AddEnergy_Update()
	{
        if(Game.GroupA.Numerical.CurrentHP <= 0 && !Game.IsBusy)
        {
            fsm.ChangeState(States.Win);
        }
        if(Game.GroupB.Numerical.CurrentHP <= 0 && !Game.IsBusy)
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
        if(Match3G_GroupInfo.CurrentGroup.isMaxStep() && !Game.IsBusy && !fsm.State.Equals(States.HeroIntroduce))
        {
            Match3G_GroupInfo.CurrentGroup.Numerical.CurrentStep = 0;
            fsm.ChangeState(States.Settlement);
        }
        Game.Match.Match_Update(); 
        Match3G_GroupInfo.Game.BootSystem.BootUpdate();
        Match3G_GroupInfo.match3G_SavingData_temp.totalPlayTime = Time.time - gameStartTime;
        if (Match3G_GroupInfo.CurrentGroup == Game.GroupA)
        {
            Match3G_GroupInfo.match3G_SavingData_round_blue.totalPlayTime += Time.deltaTime;
        }
        else
        {
            Match3G_GroupInfo.match3G_SavingData_round_red.totalPlayTime += Time.deltaTime;
        }
        if (Time.time - playStartTime < roundTime_Temp || Game.IsBusy)return;
        // fsm.ChangeState(States.SoldiersGain);
        fsm.ChangeState(States.Settlement);
		
	}
    void SoldiersGain_Enter()
    {
        soldiersGainTime = Time.time;
        Match3G_Group group = Game.WhichGroupTurn == Match3G_GroupInfo.GroupType.GroupA ? Game.GroupA : Game.GroupB;
        // group.SoldiersGain(); // 以消除的单位计算收益
        group.OccupiedSoldiersGain(); // 以占领的单位计算收益
        soldiersGain_Total = 0f;
        // soldiersGain_Total += Game.WhichGroupTurn == Match3G_GroupInfo.GroupType.GroupA ? Game.GroupB.gainUnits.Count * 0.25f : Game.GroupA.gainUnits.Count * 0.25f;
        soldiersGain_Total += Game.WhichGroupTurn == Match3G_GroupInfo.GroupType.GroupB ? Game.GroupB.gainUnits.Count * 0.25f : Game.GroupA.gainUnits.Count * 0.25f;
        Debug.Log($"soldiersGain_Total:{soldiersGain_Total}");
    }
    void SoldiersGain_Update()
    {
        if (Time.time - soldiersGainTime < soldiersGain_Total)return;
        fsm.ChangeState(States.Settlement);
    }
    void HeroOnStage_Enter()
    {
        string Lollipop = "Match3G_wav/Lollipop";
        Sound.Instance.PlaySoundTemp(Lollipop);
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
        Game.AttackSettlementByOccupied(); // 按照占领攻击
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
            // Game.AttackSettlement(); // 按照消除攻击
            fsm.ChangeState(States.TurnSwitch);
        }
    }
    void Settlement_Exit()
    {
        
    }
    
    void TurnSwitch_Enter()
    {
        Game.OutLine.Hide();
        
        Game.SwitchTurn();
        roundWaitTime = Time.time;
        Match3G_GroupInfo.round++;
        Match3G_GroupInfo.CurrentGroup.Numerical.CurrentStep = Match3G_GroupInfo.CurrentGroup.Numerical.maxStep;
        roundWaitTime_Total = Game.UI.Round.RoundSwitch(Match3G_GroupInfo.CurrentGroup);
        Game.UI.Timer.playerWarning = false;
    }
    void TurnSwitch_Update()
    {
        if (Game.IsBusy)
        {
            Game.Match.Match_Update();
        }
        else
        {
            if (Time.time - roundWaitTime < roundWaitTime_Total)return;
            fsm.ChangeState(States.AddEnergy);
        }
    }
    void Win_Enter()
    {
        string Game_over_2 = "Match3G_wav/Game_over_2";
        Sound.Instance.PlaySoundTemp(Game_over_2);
        string game_music_2 = "Match3G_wav/game_music_2";
        Sound.Instance.PlayMusicSimple(game_music_2);
        Match3G_GroupInfo.UI.MatchFinish.ShowUp(Match3G_GroupInfo.GroupType.GroupB);
    }
    void Win_OnGUI()
    {
        // string showText = automaticPlay?"胜利":"红方胜利";
        // GUIStyle myTextStyle = new GUIStyle(GUI.skin.button);
        // myTextStyle.fontSize = 210;
        // if(GUI.Button(new Rect(Screen.width/2 - 1500/2, Screen.height/2 - 70, 1500, 300), showText ,myTextStyle))
        // {
        //     string Button = "Match3G_wav/Button";
        //     Sound.Instance.PlaySoundTemp(Button);
        //     Match3G_Tool.BackToMenu();
        // }
    }
    void Lose_Enter()
    {
        string Game_over_1 = "Match3G_wav/Game_over_1";
        Sound.Instance.PlaySoundTemp(Game_over_1);
        string game_music_2 = "Match3G_wav/game_music_2";
        Sound.Instance.PlayMusicSimple(game_music_2);
        Match3G_GroupInfo.UI.MatchFinish.ShowUp(Match3G_GroupInfo.GroupType.GroupA);
    }
    void Lose_OnGUI()
    {
        // string showText = automaticPlay?"失败":"蓝方胜利";
        // GUIStyle myTextStyle = new GUIStyle(GUI.skin.button);
        // myTextStyle.fontSize = 210;
        // if(GUI.Button(new Rect(Screen.width/2 - 1500/2, Screen.height/2 - 70, 1500, 300), showText,myTextStyle))
        // {
        //     string Button = "Match3G_wav/Button";
        //     Sound.Instance.PlaySoundTemp(Button);
        //     Match3G_Tool.BackToMenu();
        // }
    }

#endregion 有限状态机
}
