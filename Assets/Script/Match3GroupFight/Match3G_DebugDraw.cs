#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Match3G_PlayerData;
public class Match3G_DebugDraw : MonoBehaviour
{
    public Match3G_GroupInfo.GroupType Debug_groupType;
    public GUIStyle style;

    void OnDrawGizmos() 
    {
        Grid2D<Match3G_Unit> tiles = new();
        if(Debug_groupType == Match3G_GroupInfo.GroupType.GroupA)
        {
            tiles = Match3G_GroupInfo.Game.GroupA.Tiles;
        }else if(Debug_groupType == Match3G_GroupInfo.GroupType.GroupB)
        {
            tiles = Match3G_GroupInfo.Game.GroupB.Tiles;
        }
        for (int y = 0; y < tiles.SizeY; y++)
		{
			for (int x = 0; x < tiles.SizeX; x++)
			{
                if(!tiles[x, y])continue;
                string IDstr = x.ToString() + "," + y.ToString();
                
                Handles.Label(tiles[x, y].transform.position, "IDstr:" + IDstr + "\n" + "Type:" + tiles[x, y].tileState.ToString(),style );
			}
		}
        
    }
}
#endif