using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBox_map : MonoBehaviour
{
    public GhostBox_map_block block;
    
    [HideInInspector]
    public List<GhostBox_map_block> blocks = new();
    public int gridHeight = 10;
    public int gridWidth = 20;
    public void CreateBlocks()
    {
        blocks.Clear();
        blocks = new();
        for(int i = 0; i < gridWidth; i++)
        {
            for(int j = 0; j < gridHeight; j++)
            {
               InstantiateBlock(i,j);
            }
        }
    }
    void InstantiateBlock(int i, int j)
    {
        GhostBox_map_block blockTemp = Instantiate(block, new Vector3(i, 0f, j), Quaternion.Euler(new Vector3(0f, 0f, 0f)),transform);
        blockTemp.posId = new Vector2(i, j);
        blocks.Add(blockTemp);
    }
}
