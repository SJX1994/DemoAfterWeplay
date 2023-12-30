using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Match3G_PlayerData;
using Unity.Mathematics;
using Random = UnityEngine.Random;
using System.Linq;
using static Unity.Mathematics.math;

public class Match3G_Group : MonoBehaviour
{  
#region 数据对象
    Match3G_Egg egg;
    public Match3G_Egg Egg 
    { 
        get 
        { 
            if (egg == null) 
                egg = FindObjectsOfType<Match3G_Egg>(true).Where(x => x.groupType == groupType).First();
            return egg; } 
    }
    public int2 size;
    [SerializeField, Range(0.1f, 20f)]
	float dropSpeed = 8f;
    [SerializeField, Range(0f, 10f)]
	float newDropOffset = 2f;
    [SerializeField, Range(0.1f, 1f)]
	float dragThreshold = 0.5f;
    public Match3G_GroupInfo.GroupType groupType;
	public Match3G_Unit[] unitPrefabs;
    public Match3G_Base basePrefab;
    public List<Match3G_Base> bases = new();
    public Grid2D<TileState> Grid => grid;
    public Grid2D<Match3G_Unit> Tiles => tiles;
    Grid2D<TileState> grid;
    Grid2D<Match3G_Unit> tiles;
    public TileState this[int2 c] => grid[c];
    public TileState this[int x, int y] => grid[x, y];

    public Move PossibleMove
	{ get; private set; }
    private List<TileDrop> droppedTiles;
    public List<TileDrop> DroppedTiles
	{ 
        get
        {
            if(droppedTiles == null)
                droppedTiles = new();
            return droppedTiles;
        } 
    private set=> droppedTiles = value; }
    private List<int2> clearedTileCoordinates;
    public List<int2> ClearedTileCoordinates
	{ 
        get
        {
            if(clearedTileCoordinates == null)
                clearedTileCoordinates = new();
            return clearedTileCoordinates;
        }
        private set => clearedTileCoordinates = value;
    }
    private List<int2> hero_clearedTileCoordinates;
    public List<int2> Hero_clearedTileCoordinates
	{ 
        get
        {
            if(hero_clearedTileCoordinates == null)
                hero_clearedTileCoordinates = new();
            return hero_clearedTileCoordinates;
        }
        private set => hero_clearedTileCoordinates = value;
    }
    public bool NeedsFilling
	{ get; private set; }
    private Vector3 offSet = new();
    public Vector3 OffSet { 
        get
        {
            if(offSet == Vector3.zero)
                offSet = transform.position;
            return offSet;
        }
        private set => offSet = value;}
    Match3G_Manager manager;
    public Match3G_Manager Manager 
    { 
        get 
        { 
            if (manager == null) 
                manager = FindObjectOfType<Match3G_Manager>();
            return manager; } 
    }
    Match3G_Group_Numerical numerical;
    public Match3G_Group_Numerical Numerical 
    { 
        get 
        { 
            if (numerical == null) 
                numerical = GetComponent<Match3G_Group_Numerical>();
            return numerical; } 
    }
    Match3G_Group otherGroup;
    public Match3G_Group OtherGroup 
    { 
        get 
        { 
            if (otherGroup == null) 
            {
                otherGroup = groupType == Match3G_GroupInfo.GroupType.GroupA ? Manager.GroupB : Manager.GroupA;
            }
                
            return otherGroup; 
            
            } 
    }
    Shake shake;
    public Shake Shake 
    { 
        get 
        { 
            if (shake == null) 
                shake = Egg.GetComponent<Shake>();
            return shake; } 
    }
    public TileSwapper tileSwapper;
    public bool HasMatches => matches.Count > 0;
    List<Match> matches = new();
    public List<Match3G_Unit> gainUnits = new();
    public bool NoMoreMove => !Manager.IsBusy && !PossibleMove.IsValid;
    public List<int2> noMoreMove_clearedTileCoordinates = new();
    
#endregion 数据对象

#region 数据关系
    public void CheckNoMoreMove()
    {
        if(!NoMoreMove)return;
        Debug.Log( groupType + "NoMoreMove!!");
        Regenerate();
    }
    public void DoAutomaticMove () => DoMove(PossibleMove);
    public void SoldiersGain()
    {
        gainUnits.Clear();
        OtherGroup.SoldiersGain_Logic();
        OtherGroup.SoldiersGain_View();
    }
    public void OccupiedSoldiersGain()
    {
        
        
        OccupiedSoldiersGain_Logic();
        OccupiedSoldiersGain_View();
        
        
    }
    public void AttackSettlement()
    {
       gainUnits.Clear();
       OtherGroup.ReduceHealth_Logic();
       OtherGroup.ReduceHealth_View();
    }
    public void AttackSettlementByOccupied()
    {
        ClearGainUnits();
        OccupiedReduceHealth_Logic();
        OccupiedReduceHealth_View();
    }
    public void TakeTheTurn()
    {
        Turn();
        DropTiles();
    }
    public void SwitchTurnClear()
    {
        foreach(var m_base in bases)
        {
            m_base.SwitchTurnClear();
        }
    }
    public void CreatUnits()
    {
        Vector3 pos = transform.position;
        switch(Match3G_GroupInfo.playMode)
        {
            case Match3G_GroupInfo.PlayMode.TwoPlayer:
                size = new int2(6,12);
                break;
            case Match3G_GroupInfo.PlayMode.Hard:
                size = new int2(6,12);
                break;
            case Match3G_GroupInfo.PlayMode.Easy:
                size = new int2(3,12);
                transform.position = new Vector3(pos.x+1.6f,pos.y,pos.z);
                Numerical.maxHP = Numerical.maxHP/2;
                Numerical.maxMP = Numerical.maxMP/2;;
                break;
        }
        CreatUnits_Logic();
        CreatUnits_View();
    }
    public void DropTiles()
    {
        if(groupType == Match3G_GroupInfo.GroupType.GroupA)
        {
            DropTiles_A_Logic();
            DropTiles_A_View();
        }else if(groupType == Match3G_GroupInfo.GroupType.GroupB)
        {
            // DropTiles_A_Logic();
            // DropTiles_A_View();
            DropTiles_B_Logic();
            DropTiles_B_View();
        }
        
    }
    public void ProcessMatches()
    {
        ProcessMatches_Logic();
        ProcessMatches_View();
    }
    void AttackOtherGroup(int2 c,TileState t,bool isHorizontal,bool isHero = false)
    {
        if(
            Manager.Flow.FSM.State == Match3G_Manager_Flow.States.AddEnergy
            || Manager.Flow.FSM.State == Match3G_Manager_Flow.States.HeroOnStage
        )
        {
            
            AttackOtherGroup_Logic(c,t,isHorizontal,isHero);
            Unfreeze(c);
        }
    }
    
    public Match3G_Egg_Hero_Cross.CrossAttackInfo Hero_CrossAttack(Match3G_Egg_Hero_Cross.CrossAttackInfo info)
    {
        Match3G_Egg_Hero_Cross.CrossAttackInfo infoOut = new();
        infoOut = Hero_CrossAttack_Logic(info.Info_posID,info.Info_horizontallyCount,info.Info_verticallyCount);
        Hero_Attack_View();
        return infoOut;
    }
    public Match3G_Egg_Hero_Range.RangeAttackInfo Hero_RangeAttack(Match3G_Egg_Hero_Range.RangeAttackInfo info)
    {
        Match3G_Egg_Hero_Range.RangeAttackInfo infoOut = new();
        infoOut = Hero_RangeAttack_Logic(info);
        Hero_Attack_View();
        return infoOut;
    }
    public Match3G_Egg_Hero_SameColor.SameColorAttackInfo Hero_SameColorAttack(Match3G_Egg_Hero_SameColor.SameColorAttackInfo info)
    {
        Match3G_Egg_Hero_SameColor.SameColorAttackInfo infoOut = new();
        infoOut = Hero_SameColorAttack_Logic(info);
        Hero_Attack_View();
        return infoOut;
    }
#endregion 数据关系
#region 数据操作
    void Regenerate()
    {
        Regenerate_Logic();
        Regenerate_View();
        NeedsFilling = true;
        string Alert = "Match3G_wav/Alert";
        Sound.Instance.PlaySoundTemp(Alert);
        Match3G_GroupInfo.Game.Flow.roundTime_Temp += 10;
    }
    void Regenerate_Logic()
    {
        noMoreMove_clearedTileCoordinates.Clear();
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                int2 c = int2(x,y);
                // if(grid.AreValidCoordinates(c))continue;
                if(grid[c] == TileState.Freezed)continue;
                grid[c] = TileState.None;
                noMoreMove_clearedTileCoordinates.Add(c);
                
            }
        }
    }
    void Regenerate_View()
    {
        for(int i = 0; i < noMoreMove_clearedTileCoordinates.Count; i++)
        {
            int2 c = noMoreMove_clearedTileCoordinates[i];
            Manager.busyDuration = Mathf.Max(tiles[c].Disappear(), Manager.busyDuration);
			tiles[c] = null;
        }
    }
    
    public bool isMaxStep()
    {
        return Numerical.ExhaustedStep;
    }
    void Unfreeze(int2 c)
    {
        if(
            Manager.Flow.FSM.State == Match3G_Manager_Flow.States.AddEnergy
            || Manager.Flow.FSM.State == Match3G_Manager_Flow.States.HeroOnStage
        )
        {
            if(groupType == Match3G_GroupInfo.GroupType.GroupB)
            {
                c = new int2(c.x,5);
                do
                {
                    c = new int2(c.x,c.y+1);
                    if(!grid.AreValidCoordinates(c))return;
                    if(!tiles[c])continue;
                }
                while(grid[c] != TileState.Freezed);

            }else if(groupType == Match3G_GroupInfo.GroupType.GroupA)
            {
                c = new int2(c.x,6);
                do
                {
                    c = new int2(c.x,c.y-1);
                    if(!grid.AreValidCoordinates(c))return;
                    if(!tiles[c])continue;
                }
                while(grid[c] != TileState.Freezed);
            }
            

            grid[c] = TileState.None;
            ClearedTileCoordinates.Add(c);
            bases.Where(x => x.posID == new Vector2(c.x,c.y)).FirstOrDefault().MeshRendererSet(true);
        }
    }
    void ClearGainUnits()
    {
        for(int i = 0; i < gainUnits.Count; i++)
        {
            if(!gainUnits[i])continue;
            gainUnits[i].Disappear();
        }
        gainUnits.Clear();
    }
    void CreatUnits_Logic()
    {
        grid = new(size); // 创建逻辑容器
        do
		{
			FillGrid(); // 填充网格 并 确保没有三联
			PossibleMove = Move.FindMove(this);
		}
		while (!PossibleMove.IsValid);
        ResetFreeze();
    }
    void CreatUnits_View()
    {   
        tiles = new(size); // 创建表现容器
        for (int y = 0; y < grid.SizeY; y++)
		{
			for (int x = 0; x < grid.SizeX; x++)
			{
                TileState t = grid[x, y];
                bool withBase = t == TileState.Freezed ? false : true;
				tiles[x, y] = SpawnTile(t, x, y,withBase);
			}
		}
    }
    public void Turn()
    {
        Numerical.CurrentAC = 0;
        bases.ForEach(x => x.Turn());
        OtherGroup.bases.ForEach(x => x.TurnBack());
        ResetFreeze();
        OtherGroup.ResetFreeze();
        //FindObjectsOfType<Match3G_Unit>().ToList().ForEach(x => x.TurnChange());
    }
    void ReduceHealth_Logic()
    {
        for (int y = 0; y < grid.SizeY; y++)
        {
            for (int x = 0; x < grid.SizeX; x++)
            {
                if(tiles[x,y].groupType != groupType)
                {
                    grid[x,y] = TileState.None;
                }
            }
        }
    }
    void ReduceHealth_View()
    {
        for (int y = 0; y < grid.SizeY; y++)
        {
            for (int x = 0; x < grid.SizeX; x++)
            {
                if(tiles[x,y].groupType != groupType)
                {
                    
                    tiles[x,y].MoveToHealthBar(Egg.transform.position,this);
                    tiles[x,y].Disappear();
                    tiles[x,y] = null;
                }
            }
        }
    }
    void OccupiedReduceHealth_Logic()
    {
        int fromY = 0;
        int toY = 0;
        if(groupType == Match3G_GroupInfo.GroupType.GroupA)
        {
            fromY = 0;
            toY = grid.SizeY/2;
        }else if(groupType == Match3G_GroupInfo.GroupType.GroupB)
        {
            fromY = 6;
            toY = grid.SizeY;
        }
        for (int y = fromY; y < toY; y++)
        {
            for (int x = 0; x < grid.SizeX; x++)
            {
                if(grid[x,y] == TileState.Freezed)continue;
                grid[x,y] = TileState.Freezed;
                Match3G_Unit gainUnit = tiles[x,y];
                gainUnits.Add(gainUnit);
                if(groupType == Match3G_GroupInfo.GroupType.GroupB)
                {
                    Match3G_GroupInfo.match3G_SavingData_round_red.totalKillNumbers += 1;
                }else
                {
                    Match3G_GroupInfo.match3G_SavingData_round_blue.totalKillNumbers += 1;
                }
                if(groupType != Match3G_GroupInfo.GroupType.GroupB)continue;
                Match3G_GroupInfo.match3G_SavingData_temp.totalKillNumbers += 1;
            }
        }
        Match3G_GroupInfo.Game.UI.SavedData.SaveData();
    }
    void OccupiedReduceHealth_View()
    {
        int totalDamage = 0;
        for(int i = 0; i < gainUnits.Count; i++)
        {
            gainUnits[i].Disappear(2.5f);
            totalDamage += gainUnits[i].MoveToHealthBar(OtherGroup.Egg.transform.position,OtherGroup,i);
            
        }
        if(OtherGroup.groupType == Match3G_GroupInfo.GroupType.GroupB)
        {
            Match3G_GroupInfo.UI.HealthBar_Red.DamagerSettlement.Show(totalDamage);
        }else
        {
            Match3G_GroupInfo.UI.HealthBar_Blue.DamagerSettlement.Show(totalDamage);
        }
    }
    void SoldiersGain_Logic()
    {
        for (int y = 0; y < grid.SizeY; y++)
        {
            for (int x = 0; x < grid.SizeX; x++)
            {
                if(tiles[x,y].groupType != groupType)
                {
                    Match3G_Unit gainUnit = tiles[x,y];
                    gainUnits.Add(gainUnit);
                }
            }
        }
    }
    void SoldiersGain_View()
    {
        for(int i = 0; i < gainUnits.Count; i++)
        {
            gainUnits[i].Gain(OtherGroup,i);
        }
    }
    void OccupiedSoldiersGain_Logic()
    {
        int fromY = 0;
        int toY = 0;
        if(groupType == Match3G_GroupInfo.GroupType.GroupA)
        {
            fromY = 0;
            toY = grid.SizeY/2-1;
        }else if(groupType == Match3G_GroupInfo.GroupType.GroupB)
        {
            fromY = 6;
            toY = grid.SizeY;
        }
        for (int y = fromY; y < toY; y++)
        {
            for (int x = 0; x < grid.SizeX; x++)
            {
                if(grid[x,y] == TileState.Freezed)continue;
                Match3G_Unit gainUnit = tiles[x,y];
                gainUnits.Add(gainUnit);
            }
        }
    }
    void OccupiedSoldiersGain_View()
    {
        for(int i = 0; i < gainUnits.Count; i++)
        {
            if(!gainUnits[i])continue;
            gainUnits[i].Gain(this,i);
        }
    }
    void FillGrid ()
	{
        int typesCount = unitPrefabs.Length+1;
		for (int y = 0; y < size.y; y++)
		{
			for (int x = 0; x < size.x; x++)
			{
				TileState a = TileState.None, b = TileState.None;
				int potentialMatchCount = 0;
				if (x > 1)
				{
					a = grid[x - 1, y];
					if (a == grid[x - 2, y])
					{
						potentialMatchCount = 1;
					}
				}
				if (y > 1)
				{
					b = grid[x, y - 1];
					if (b == grid[x, y - 2])
					{
						potentialMatchCount += 1;
						if (potentialMatchCount == 1)
						{
							a = b;
						}
						else if (b < a)
						{
							(a, b) = (b, a);
						}
					}
				}
				TileState t = (TileState)Random.Range(Match3G_GroupInfo.TileStateStart, typesCount - potentialMatchCount);
				if (potentialMatchCount > 0 && t >= a)
				{
					t += 1;
				}
				if (potentialMatchCount == 2 && t >= b)
				{
					t += 1;
				}
				grid[x, y] = t;
			}
		}
	}
    public Match3G_Unit SpawnTile(TileState tileState , int x , int y, bool withBase = true)
    {
        Match3G_Unit u = Instantiate(unitPrefabs.Where(x => x.tileState == tileState).First());
        u.transform.position = new Vector3(x, y, 0);
        u.transform.position += OffSet;
        u.Spawn();
        Manager.Match.OnTurnSwitch += u.TurnChange;
        Match3G_Base b = Instantiate(basePrefab);
        b.transform.position = new Vector3(x, y, +1f);
        b.posID = new Vector2Int(x, y);
        b.pairedUnit = u;
        b.transform.position += OffSet;
        b.midPos = b.transform.position;
        b.MeshRendererSet(withBase);
        b.Display_BasesCheckerboard();
        bases.Add(b);
        return u;
    }
    public Match3G_Unit SpawnTile(TileState tileState , int x , int y,Match3G_Unit[] unitPrefabs)
    {
        if(tileState == TileState.None)tileState = TileState.Freezed;
        Match3G_Unit uPrefab = unitPrefabs.Where(x => x.tileState == tileState).First();
        Match3G_Unit u = Instantiate(uPrefab);
        u.transform.position = new Vector3(x, y, 0);
        u.transform.position += OffSet;
        u.Spawn();
        Manager.Match.OnTurnSwitch += u.TurnChange;
        return u;
    }
    public bool EvaluateDrag (Vector3 start, Vector3 end)
	{
		float2 a = ScreenToTileSpace(start), b = ScreenToTileSpace(end);
        if(a.x == -1 && a.y == -1)return false;
		Move move = new(
			(int2)floor(a), (b - a) switch
			{
				var d when d.x > dragThreshold => MoveDirection.Right,
				var d when d.x < -dragThreshold => MoveDirection.Left,
				var d when d.y > dragThreshold => MoveDirection.Up,
				var d when d.y < -dragThreshold => MoveDirection.Down,
				_ => MoveDirection.None
			}
		);
        Manager.OutLine.Show(tiles[move.From].transform.position);
		if (
			move.IsValid &&
			grid.AreValidCoordinates(move.From) && grid.AreValidCoordinates(move.To)
			)
		{
            Manager.OutLine.Hide();
			DoMove(move);
			return false;
		}
		return true;
	}
    float2 ScreenToTileSpace (Vector3 screenPosition)
	{
		Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        RaycastHit hit;
        Match3G_Base unit;

        if(!Physics.Raycast(ray, out hit))return float2(-1,-1);
        hit.collider.TryGetComponent(out unit);
        if(!unit)return float2(-1,-1);

		Vector3 p = ray.origin - ray.direction * (ray.origin.z / ray.direction.z);
		return float2(p.x - OffSet.x + 0.5f, p.y - OffSet.y + 0.5f);
	}
    void DoMove (Move move)
	{
		bool succcess = TryMove(move);
		Match3G_Unit a = tiles[move.From], b = tiles[move.To];
		Manager.busyDuration = tileSwapper.Swap(a, b, !succcess);
		if (succcess)
		{
			tiles[move.From] = b;
			tiles[move.To] = a;
            Numerical.CurrentStep -= 1;
            string Jelly_crash = "Match3G_wav/Jelly_crash";
            Sound.Instance.PlaySoundTemp(Jelly_crash);
            Match3G_GroupInfo.Game.BootSystem.HideBoot();
		}else
        {
            string Wrong_match = "Match3G_wav/Wrong_match";
            Sound.Instance.PlaySoundTemp(Wrong_match);
        }
	}
    public bool TryMove (Move move)
	{
        if(grid.AreValidCoordinates(move.From) && grid.AreValidCoordinates(move.To))
        {
            if(grid[move.From] == TileState.Freezed || grid[move.To] == TileState.Freezed)
            {
                return false;
            }
        }
        
		Manager.Numerical.enegyMultiplier = 1;
        
		grid.Swap(move.From, move.To);
		if (FindMatches())
		{
			return true;
		}
		grid.Swap(move.From, move.To);
		return false;
	}
    bool FindMatches ()
	{
		matches.Clear();

		for (int y = 0; y < size.y; y++)
		{
			TileState start = grid[0, y];
			int length = 1;
			for (int x = 1; x < size.x; x++)
			{
				TileState t = grid[x, y];
				if (t == start && t != TileState.Freezed)
				{
					length += 1;
				}
				else
				{
					if (length >= 3)
					{
						matches.Add(new Match(x - length, y, length, true));
					}
					start = t;
					length = 1;
				}
			}
			if (length >= 3)
			{
				matches.Add(new Match(size.x - length, y, length, true));
			}
		}

		for (int x = 0; x < size.x; x++)
		{
			TileState start = grid[x, 0];
			int length = 1;
			for (int y = 1; y < size.y; y++)
			{
				TileState t = grid[x, y];
				if (t == start && t != TileState.Freezed)
				{
					length += 1;
				}
				else
				{
					if (length >= 3)
					{
						matches.Add(new Match(x, y - length, length, false));
					}
					start = t;
					length = 1;
				}
			}
			if (length >= 3)
			{
				matches.Add(new Match(x, size.y - length, length, false));
			}
		}

		return HasMatches;
	}
    void DropTiles_A_Logic()
    {
        int typesCount = unitPrefabs.Length + 1;
		DroppedTiles.Clear();
        
        for (int x = 0; x < size.x; x++)
        {
            int holeCount = 0;
            for (int y = 0; y < size.y; y++)
            {
                if (grid[x, y] == TileState.None)
                {
                    holeCount += 1;
                }
                else if (holeCount > 0)
                {
                    grid[x, y - holeCount] = grid[x, y];
                    DroppedTiles.Add(new TileDrop(x, y - holeCount, holeCount));
                }
            }
            
            for (int h = 1; h <= holeCount; h++)
            {
                grid[x, size.y - h] = (TileState)Random.Range(Match3G_GroupInfo.TileStateStart, typesCount);
                DroppedTiles.Add(new TileDrop(x, size.y - h, holeCount));
            }
        }
		

		NeedsFilling = false;
		if (!FindMatches())
		{
			PossibleMove = Move.FindMove(this);
		}
    }
    void DropTiles_B_Logic()
    {
        int typesCount = unitPrefabs.Length + 1;
		DroppedTiles.Clear();
        
        for (int x = 0; x < size.x; x++)
        {
            int holeCount = 0;
            for (int y = size.y - 1; y >= 0; y--)
            {
                if (grid[x, y] == TileState.None)
                {
                    holeCount += 1;
                }
                else if (holeCount > 0)
                {
                    grid[x, y + holeCount] = grid[x, y];
                    DroppedTiles.Add(new TileDrop(x, y + holeCount, -holeCount));
                }
            }
            for (int h = 1; h <= holeCount; h++)
            {
                grid[x, h-1] = (TileState)Random.Range(Match3G_GroupInfo.TileStateStart, typesCount);
                DroppedTiles.Add(new TileDrop(x, h-1, - holeCount));
            }
        }
		

		NeedsFilling = false;
		if (!FindMatches())
		{
			PossibleMove = Move.FindMove(this);
		}
    }
    void DropTiles_A_View()
    {
        for (int i = 0; i < DroppedTiles.Count; i++)
		{
			TileDrop drop = DroppedTiles[i];
			Match3G_Unit tile;
			if (drop.fromY < tiles.SizeY)
			{
				tile = tiles[drop.coordinates.x, drop.fromY];
			}
			else
			{
                TileState t = grid[drop.coordinates];
				tile = SpawnTile(
					t, drop.coordinates.x, drop.fromY + (int)newDropOffset,false
				);
			}
			tiles[drop.coordinates] = tile;
			Manager.busyDuration = Mathf.Max(
				tile.Fall(drop.coordinates.y + OffSet.y, dropSpeed), Manager.busyDuration
			);
		}
    }
    void DropTiles_B_View()
    {
        for (int i = 0; i < DroppedTiles.Count; i++)
		{
			TileDrop drop = DroppedTiles[i];
			Match3G_Unit tile = null;
			if (drop.fromY >= 0)
			{
				tile = tiles[drop.coordinates.x, drop.fromY];
			}
			else
			{
                TileState t = grid[drop.coordinates];
				tile = SpawnTile(
					t, drop.coordinates.x, drop.fromY - (int)newDropOffset,false
				);
			}
			tiles[drop.coordinates] = tile;
			Manager.busyDuration = Mathf.Max(
				tile.Fall(drop.coordinates.y + OffSet.y, -dropSpeed), Manager.busyDuration
			);
		}
    }
    void ProcessMatches_Logic()
    {
        ClearedTileCoordinates.Clear();
        Manager.Numerical.ClearEnegys();
        List<Match> realMatches = new();
        for (int m = 0; m < matches.Count; m++)
		{
            Match match = matches[m];
			int2 c = match.coordinates;
            if(grid[c] == TileState.Freezed)continue;
            realMatches.Add(match);
        }
		for (int m = 0; m < realMatches.Count; m++)
		{
			Match match = realMatches[m];
			int2 step = match.isHorizontal ? int2(1, 0) : int2(0, 1);
			int2 c = match.coordinates;
			for (int i = 0; i < match.length; c += step, i++)
			{
                
				if (grid[c] != TileState.None) 
				{
                    AttackOtherGroup(c,grid[c],match.isHorizontal);
					grid[c] = TileState.None;
					ClearedTileCoordinates.Add(c);
				}
			}
            int2 pos = match.coordinates + step * (match.length - 1);
            Vector3 pos3 = tiles[pos].transform.position;
            int value = match.length;
            Manager.Numerical.AddEnegy(pos3,Egg.transform.position,value,Egg);
		}
		matches.Clear();
		NeedsFilling = true;
    }
    void ProcessMatches_View()
    {
        for (int i = 0; i < ClearedTileCoordinates.Count; i++)
		{
			int2 c = ClearedTileCoordinates[i];
			Manager.busyDuration = Mathf.Max(tiles[c].Disappear(), Manager.busyDuration);
			tiles[c] = null;
            // Debug.Log("消除"+c);
		}
       
    } 
    void AttackOtherGroup_Logic(int2 c,TileState t,bool isHorizontal,bool isHero = false)
    {
        
        // 从下往上递增，从左往右递增
        if(groupType == Match3G_GroupInfo.GroupType.GroupB)
        {
            // c = new int2(c.x,-1);
            c = new int2(c.x,5);
            do
            {
                // c = new int2(c.x,c.y+1);
                c = new int2(c.x,c.y+1);
                if(!OtherGroup.tiles.AreValidCoordinates(c))return;
                if(!OtherGroup.tiles[c])continue;
            }
            while(OtherGroup.tiles[c].groupType == groupType);

           

        }else if(groupType == Match3G_GroupInfo.GroupType.GroupA)
        {
            c = new int2(c.x,6);
            do
            {
                c = new int2(c.x,c.y-1);
                if(!OtherGroup.tiles.AreValidCoordinates(c))return;
                if(!OtherGroup.tiles[c])continue;
            }
            while(OtherGroup.tiles[c].groupType == groupType);
        }
        AttackOtherGroup_View(c,t);
        OtherGroup.grid[c] = TileState.None;
        OtherGroup.ClearedTileCoordinates.Add(c);
        
    }
    void AttackOtherGroup_View(int2 c,TileState t)
    {
        if(!OtherGroup.tiles[c])return;
        // OtherGroup.tiles[c].MakeEnergy();
        OtherGroup.tiles[c].KnockedAway();
        // OtherGroup.tiles[c].Disappear();
        OtherGroup.tiles[c] = null;      
		OtherGroup.tiles[c] = OtherGroup.SpawnTile(t, c.x, c.y,unitPrefabs);
        if(!OtherGroup.tiles[c])return;
        Match3G_Base otherBase = OtherGroup.bases.Where(x => x.posID == new Vector2(c.x,c.y)).FirstOrDefault();
        otherBase.BeenOccupied();
        OtherGroup.tiles[c].Gain(this,1);
        OtherGroup.tiles[c].HideAfterOccupied();
        //otherBase.TempBase = basePrefab;
    }
    Match3G_Egg_Hero_SameColor.SameColorAttackInfo Hero_SameColorAttack_Logic(Match3G_Egg_Hero_SameColor.SameColorAttackInfo info)
    {
        Hero_clearedTileCoordinates.Clear();
        int2 currentPosition = new int2((int)info.Info_posID.x, (int)info.Info_posID.y);
        Match3G_Egg_Hero_SameColor.SameColorAttackInfo infoOut = new(info.Info_posID,info.Info_number,new List<Match3G_Base>());
        List<Match3G_Base> basesFit = new();
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                int2 c = new int2(x,y);
                if(grid[c] == grid[currentPosition])
                {
                    basesFit.Add(bases.Where(x => x.posID == new Vector2(c.x,c.y)).FirstOrDefault());
                }
            }
        }
        foreach(var m_base in basesFit)
        {
            int2 c = new int2((int)m_base.posID.x,(int)m_base.posID.y);
            // AttackOtherGroup(c,grid[c],true,isHero:true);
            Hero_clearedTileCoordinates.Add(c);
            infoOut.Info_targets.Add(m_base);
            grid[c] = TileState.None;
        }
        return infoOut;
    }
    Match3G_Egg_Hero_Range.RangeAttackInfo Hero_RangeAttack_Logic(Match3G_Egg_Hero_Range.RangeAttackInfo info)
    {
        Hero_clearedTileCoordinates.Clear();
        int2 currentPosition = new int2((int)info.Info_posID.x, (int)info.Info_posID.y);
        Match3G_Egg_Hero_Range.RangeAttackInfo infoOut = new(info.Info_posID,info.Info_range,new List<Match3G_Base>());
        for (int x = currentPosition.x - info.Info_range; x <= currentPosition.x + info.Info_range; x++)
        {
            for (int y = currentPosition.y - info.Info_range; y <= currentPosition.y + info.Info_range; y++)
            {
                Vector2Int gridPosition = new Vector2Int(x, y);
                int2 c = new int2(x,y);
                if (Vector2Int.Distance(new Vector2Int(currentPosition.x,currentPosition.y), gridPosition) > info.Info_range)continue;
                if(!grid.AreValidCoordinates(c))continue;
                
                AttackOtherGroup(c,grid[c],true,isHero:true);
                grid[c] = TileState.None;
                Hero_clearedTileCoordinates.Add(c);
                infoOut.Info_targets.Add(bases.Where(x => x.posID == new Vector2(c.x,c.y)).FirstOrDefault());
            }
            
        }
        return infoOut;
    }
    Match3G_Egg_Hero_Cross.CrossAttackInfo Hero_CrossAttack_Logic(Vector2 posID, int horizontallyCount, int verticallyCount)
    {
        bool isHero = true;
        Hero_clearedTileCoordinates.Clear();
        int2 c = new int2((int)posID.x, (int)posID.y);
        AttackOtherGroup(c,grid[c],true,isHero);
        grid[c] = TileState.None;
        Hero_clearedTileCoordinates.Add(c);
        int leftCount = 0;
        int rightCount = 0;
        int upCount = 0;
        int downCount = 0;
        for(int i = 1; i <= horizontallyCount; i++)
        {
            c = new int2((int)posID.x+i, (int)posID.y);
            if(grid.AreValidCoordinates(c))
            {
                if(grid[c] == TileState.Freezed || !OtherGroup.tiles[c])continue;
                AttackOtherGroup(c,grid[c],true,isHero);
                grid[c] = TileState.None;
                Hero_clearedTileCoordinates.Add(c);
                rightCount++;
            }
            c = new int2((int)posID.x-i, (int)posID.y);
            if(grid.AreValidCoordinates(c))
            {
                if(grid[c] == TileState.Freezed || !OtherGroup.tiles[c])continue;
                AttackOtherGroup(c,grid[c],true,isHero);
                grid[c] = TileState.None;
                Hero_clearedTileCoordinates.Add(c);
                leftCount++;
            }
        }
        for(int i = 1; i <= verticallyCount; i++)
        {
            c = new int2((int)posID.x, (int)posID.y+i);
            if(grid.AreValidCoordinates(c))
            {
                if(grid[c] == TileState.Freezed || !OtherGroup.tiles[c])continue;
                AttackOtherGroup(c,grid[c],false,isHero);
                grid[c] = TileState.None;
                Hero_clearedTileCoordinates.Add(c);
                upCount++;
            }
            c = new int2((int)posID.x, (int)posID.y-i);
            if(grid.AreValidCoordinates(c))
            {
                if(grid[c] == TileState.Freezed || !OtherGroup.tiles[c])continue;
                AttackOtherGroup(c,grid[c],false,isHero);
                grid[c] = TileState.None;
                Hero_clearedTileCoordinates.Add(c);
                downCount++;
            }
        }
        Match3G_Egg_Hero_Cross.CrossAttackInfo infoOut = new(posID,verticallyCount,horizontallyCount,leftCount,rightCount,upCount,downCount);
        return infoOut;
    }
    void Hero_Attack_View()
    {
        for (int i = 0; i < Hero_clearedTileCoordinates.Count; i++)
		{
			int2 c = Hero_clearedTileCoordinates[i];
            if(!tiles[c])continue;
			Manager.busyDuration = Mathf.Max(tiles[c].Disappear(), Manager.busyDuration);
			tiles[c] = null;
            // Debug.Log("消除"+c);
		}
        Invoke(nameof(Filling_Delay),0.5f);
        
    }
    void Filling_Delay()
    {
        NeedsFilling = true;
    }
    void ResetFreeze()
    {
        int fromY = 0;
        int toY = 0;
        if(groupType == Match3G_GroupInfo.GroupType.GroupA)
        {
            fromY = 0;
            toY = grid.SizeY/2;
        }else if(groupType == Match3G_GroupInfo.GroupType.GroupB)
        {
            fromY = 6;
            toY = grid.SizeY;
        }
        
        for (int y = fromY; y < toY; y++)
        {
            for (int x = 0; x < grid.SizeX; x++)
            {
                grid[x,y] = TileState.Freezed;
                if(tiles.IsUndefined)continue;
                if(tiles[x,y])continue;
                tiles[x,y] = SpawnTile(TileState.Freezed, x, y,false);
            }
        }
        
    }
#endregion 数据操作
}
