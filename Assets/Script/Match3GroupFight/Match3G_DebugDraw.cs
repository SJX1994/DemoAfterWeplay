using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Match3G_PlayerData;
public class Match3G_DebugDraw : MonoBehaviour
{
    public GUIStyle style;
    void OnDrawGizmos() 
    {
        
        Grid2D<Match3G_Unit> tiles = Match3G_GroupInfo.Game.GroupB.Tiles;
        for (int y = 0; y < tiles.SizeY; y++)
		{
			for (int x = 0; x < tiles.SizeX; x++)
			{
                if(!tiles[x, y])continue;
                string IDstr = x.ToString() + "," + y.ToString();
                
                Handles.Label(tiles[x, y].transform.position, "IDstr:" + IDstr,style);
			}
		}
        
    }
}