using TMPro;
using Unity.Mathematics;
using UnityEngine;
using static Unity.Mathematics.math;
using Match3_PlayerData;
public class Match3_Manager_View : MonoBehaviour
{
    [SerializeField]
	public Match3_Tile[] tilePrefabs;
    Match3_Manager_Main game;
    public Match3_Manager_Main Game 
    { 
        get 
        { 
            if (game == null) 
                game = FindObjectOfType<Match3_Manager_Main>();
            return game; } 
    }
    [SerializeField, Range(0.1f, 1f)]
	float dragThreshold = 0.5f;
    [SerializeField, Range(0f, 10f)]
	float newDropOffset = 2f;
    [SerializeField, Range(0.1f, 20f)]
	float dropSpeed = 8f;
    [SerializeField]
	TileSwapper tileSwapper;
    [SerializeField]
	TextMeshPro gameOverText, totalScoreText;
    [SerializeField]
	Match3_FloatingScore floatingScorePrefab;
    float busyDuration;
    Grid2D<Match3_Tile> tiles;
    float2 tileOffset;
    public bool IsBusy => busyDuration > 0f;
    public bool IsPlaying => IsBusy || Game.Logic.PossibleMove.IsValid;
    float floatingScoreZ;
    public void StartNewGame () 
    {
        busyDuration = 0f;
		totalScoreText.SetText("0");
		gameOverText.gameObject.SetActive(false);
		
		Game.Logic.StartNewGame();

		
		tileOffset = -0.5f * (float2)(Game.Logic.Size - 1);
		
		
		if (tiles.IsUndefined)
		{
			tiles = new(Game.Logic.Size);
		}
		else
		{
			for (int y = 0; y < tiles.SizeY; y++)
			{
				for (int x = 0; x < tiles.SizeX; x++)
				{
					tiles[x, y].Despawn();
					tiles[x, y] = null;
				}
			}
		}
		for (int y = 0; y < tiles.SizeY; y++)
		{
			for (int x = 0; x < tiles.SizeX; x++)
			{
                TileState t = Game.Logic[x, y];
                
				tiles[x, y] = SpawnTile(t, x, y);
			}
		}
    }
    public void DoWork () 
    {
        if (busyDuration > 0f)
		{
			tileSwapper.Update();
			busyDuration -= Time.deltaTime;
			if (busyDuration > 0f)return;
		}

		if (Game.Logic.HasMatches)
		{
			ProcessMatches();
		}
		else if (Game.Logic.NeedsFilling)
		{
			DropTiles();
		}
		else if (!IsPlaying)
		{
			gameOverText.gameObject.SetActive(true);
		}
    }
    public void DoAutomaticMove () => DoMove(Game.Logic.PossibleMove);
    public bool EvaluateDrag (Vector3 start, Vector3 end)
	{
		float2 a = ScreenToTileSpace(start), b = ScreenToTileSpace(end);
		var move = new Move(
			(int2)floor(a), (b - a) switch
			{
				var d when d.x > dragThreshold => MoveDirection.Right,
				var d when d.x < -dragThreshold => MoveDirection.Left,
				var d when d.y > dragThreshold => MoveDirection.Up,
				var d when d.y < -dragThreshold => MoveDirection.Down,
				_ => MoveDirection.None
			}
		);
		if (
			move.IsValid &&
			tiles.AreValidCoordinates(move.From) && tiles.AreValidCoordinates(move.To)
			)
		{
			DoMove(move);
			return false;
		}
		return true;
	}
    void DropTiles()
	{
		Game.Logic.DropTiles();
		
		for (int i = 0; i < Game.Logic.DroppedTiles.Count; i++)
		{
			TileDrop drop = Game.Logic.DroppedTiles[i];
			Match3_Tile tile;
			if (drop.fromY < tiles.SizeY)
			{
				tile = tiles[drop.coordinates.x, drop.fromY];
			}
			else
			{
                TileState t = Game.Logic[drop.coordinates];
				tile = SpawnTile(
					t, drop.coordinates.x, drop.fromY + newDropOffset
				);
			}
			tiles[drop.coordinates] = tile;
			busyDuration = Mathf.Max(
				tile.Fall(drop.coordinates.y + tileOffset.y, dropSpeed), busyDuration
			);
		}
	}
    
	void ProcessMatches ()
	{
		Game.Logic.ProcessMatches();

		for (int i = 0; i < Game.Logic.ClearedTileCoordinates.Count; i++)
		{
			int2 c = Game.Logic.ClearedTileCoordinates[i];
			busyDuration = Mathf.Max(tiles[c].Disappear(), busyDuration);
			tiles[c] = null;
		}

		totalScoreText.SetText("{0}", Game.Logic.TotalScore);

		for (int i = 0; i < Game.Logic.Scores.Count; i++)
		{
			SingleScore score = Game.Logic.Scores[i];
			floatingScorePrefab.Show(
				new Vector3(
					score.position.x + tileOffset.x,
					score.position.y + tileOffset.y,
					floatingScoreZ
				),
				score.value
			);
			floatingScoreZ = floatingScoreZ <= -0.02f ? 0f : floatingScoreZ - 0.001f;
		}
	}
    void DoMove (Move move)
	{
		bool succcess = Game.Logic.TryMove(move);
		Match3_Tile a = tiles[move.From], b = tiles[move.To];
		busyDuration = tileSwapper.Swap(a, b, !succcess);
		if (succcess)
		{
			tiles[move.From] = b;
			tiles[move.To] = a;
		}
	}
    float2 ScreenToTileSpace (Vector3 screenPosition)
	{
		Ray ray = Camera.main.ScreenPointToRay(screenPosition);
		Vector3 p = ray.origin - ray.direction * (ray.origin.z / ray.direction.z);
		return float2(p.x - tileOffset.x + 0.5f, p.y - tileOffset.y + 0.5f);
	}
    Match3_Tile SpawnTile (TileState t, float x, float y) =>
		tilePrefabs[(int)t - 1].Spawn(new Vector3(x + tileOffset.x, y + tileOffset.y));
}
