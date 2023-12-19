
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UC_PlayerData;
public class Blocks : MonoBehaviour
{
    private Manager manager;
    public Manager Manager
    {
        get
        {
            if(manager == null)manager = FindObjectOfType<Manager>();
            return manager;
        }
        set
        {
            manager = value;
        }
    }
    public int gridHeight = 10;
    public int gridWidth = 20;
    public Block block;

    [HideInInspector]
    public List<Block> blocks = new();
    
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
        Block blockTemp = Instantiate(block, new Vector3(i, 0f, j), Quaternion.Euler(new Vector3(0f, 0f, 0f)),transform);
        blockTemp.posId = new Vector2(i, j);
        blocks.Add(blockTemp);
    }
    public void CheckMerge()
    {
        Debug.Log("CheckMerge!");
        // 存储可消除的宝石列表
        List<Unit> matchedUnits = new List<Unit>();
        Vector2 posId_Main = Vector2.zero;
        UnitData.Type type_Main = UnitData.Type.notReady;
        // 遍历棋盘检查匹配
        for(int i = 0; i < gridWidth; i++)
        {
            for(int j = 0; j <  gridHeight; j++)
            {
                Block block = blocks.Where(x=>x.posId == new Vector2(i,j)).FirstOrDefault();
                Unit currentUnit = null;
                
                if(block)
                {
                    currentUnit = block.OccupiedUnit;
                }
                if(currentUnit)
                {
                    // currentUnit.transform.localScale = Vector3.one*0.5f;
                    posId_Main = currentUnit.posId;
                    type_Main = currentUnit.type;
                    
                    // 检查水平方向是否有匹配
                    Block block1 = blocks.Where(x=>x.posId == new Vector2(i+1,j)).FirstOrDefault();
                    Block block2 = blocks.Where(x=>x.posId == new Vector2(i-1,j)).FirstOrDefault();
                    Unit unit1 = null;
                    Unit unit2 = null;
                    if(block1 && block2)
                    {
                        unit1 = block1.OccupiedUnit;
                        unit2 = block2.OccupiedUnit;
                    }
                    if(unit1 && unit2)
                    {
                        // unit1.transform.localScale = Vector3.one*0.5f;
                        // unit2.transform.localScale = Vector3.one*0.5f;
                        if(currentUnit.tag == unit1.tag && currentUnit.tag == unit2.tag)
                        {
                            matchedUnits.Add(currentUnit);
                            matchedUnits.Add(unit1);
                            matchedUnits.Add(unit2);
                        }
                    }
                    // Block block1_upper = blocks.Where(x=>x.posId == new Vector2(i+1,j)).FirstOrDefault();
                    // Block block2_upper = blocks.Where(x=>x.posId == new Vector2(i+2,j)).FirstOrDefault();
                    // Unit unit1_upper = null;
                    // Unit unit2_upper = null;
                    // if(block1_upper && block2_upper)
                    // {
                    //     unit1_upper = block1_upper.OccupiedUnit;
                    //     unit2_upper = block2_upper.OccupiedUnit;
                    // }
                    // if(unit1_upper && unit2_upper)
                    // {
                    //     // unit1.transform.localScale = Vector3.one*0.5f;
                    //     // unit2.transform.localScale = Vector3.one*0.5f;
                    //     if(currentUnit.tag == unit1_upper.tag && currentUnit.tag == unit2_upper.tag)
                    //     {
                    //         matchedUnits.Add(currentUnit);
                    //         matchedUnits.Add(unit1_upper);
                    //         matchedUnits.Add(unit2_upper);
                    //     }
                    // }
                    // Block block1_lower = blocks.Where(x=>x.posId == new Vector2(i-1,j)).FirstOrDefault();
                    // Block block2_lower = blocks.Where(x=>x.posId == new Vector2(i-2,j)).FirstOrDefault();
                    // Unit unit1_lower = null;
                    // Unit unit2_lower = null;
                    // if(block1_lower && block2_lower)
                    // {
                    //     unit1_lower = block1_lower.OccupiedUnit;
                    //     unit2_lower = block2_lower.OccupiedUnit;
                    // }
                    // if(unit1_lower && unit2_lower)
                    // {
                    //     // unit1.transform.localScale = Vector3.one*0.5f;
                    //     // unit2.transform.localScale = Vector3.one*0.5f;
                    //     if(currentUnit.tag == unit1_lower.tag && currentUnit.tag == unit2_lower.tag)
                    //     {
                    //         matchedUnits.Add(currentUnit);
                    //         matchedUnits.Add(unit1_lower);
                    //         matchedUnits.Add(unit2_lower);
                    //     }
                    // }
                    // 检查垂直方向是否有匹配
                    Block block3 = blocks.Where(x=>x.posId == new Vector2(i,j+1)).FirstOrDefault();
                    Block block4 = blocks.Where(x=>x.posId == new Vector2(i,j-1)).FirstOrDefault();
                    Unit unit3 = null;
                    Unit unit4 = null;
                    if(block3 && block4)
                    {
                        unit3 = block3.OccupiedUnit;
                        unit4 = block4.OccupiedUnit;
                    }
                    if(unit3 && unit4)
                    {
                        // unit3.transform.localScale = Vector3.one*0.5f;
                        // unit4.transform.localScale = Vector3.one*0.5f;
                        if(currentUnit.tag == unit3.tag && currentUnit.tag == unit4.tag)
                        {
                            // 当前宝石和相邻两个宝石匹配
                            matchedUnits.Add(currentUnit);
                            matchedUnits.Add(unit3);
                            matchedUnits.Add(unit4);
                        }
                    }
                    // Block block3_upper = blocks.Where(x=>x.posId == new Vector2(i,j+1)).FirstOrDefault();
                    // Block block4_upper = blocks.Where(x=>x.posId == new Vector2(i,j+2)).FirstOrDefault();
                    // Unit unit3_upper = null;
                    // Unit unit4_upper = null;
                    // if(block3_upper && block4_upper)
                    // {
                    //     unit3_upper = block3_upper.OccupiedUnit;
                    //     unit4_upper = block4_upper.OccupiedUnit;
                    // }
                    // if(unit3_upper && unit4_upper)
                    // {
                    //     // unit3.transform.localScale = Vector3.one*0.5f;
                    //     // unit4.transform.localScale = Vector3.one*0.5f;
                    //     if(currentUnit.tag == unit3_upper.tag && currentUnit.tag == unit4_upper.tag)
                    //     {
                    //         // 当前宝石和相邻两个宝石匹配
                    //         matchedUnits.Add(currentUnit);
                    //         matchedUnits.Add(unit3_upper);
                    //         matchedUnits.Add(unit4_upper);
                    //     }
                    // }
                    // Block block3_lower = blocks.Where(x=>x.posId == new Vector2(i,j-1)).FirstOrDefault();
                    // Block block4_lower = blocks.Where(x=>x.posId == new Vector2(i,j-2)).FirstOrDefault();
                    // Unit unit3_lower = null;
                    // Unit unit4_lower = null;
                    // if(block3_lower && block4_lower)
                    // {
                    //     unit3_lower = block3_lower.OccupiedUnit;
                    //     unit4_lower = block4_lower.OccupiedUnit;
                    // }
                    // if(unit3_lower && unit4_lower)
                    // {
                    //     // unit3.transform.localScale = Vector3.one*0.5f;
                    //     // unit4.transform.localScale = Vector3.one*0.5f;
                    //     if(currentUnit.tag == unit3_lower.tag && currentUnit.tag == unit4_lower.tag)
                    //     {
                    //         // 当前宝石和相邻两个宝石匹配
                    //         matchedUnits.Add(currentUnit);
                    //         matchedUnits.Add(unit3_lower);
                    //         matchedUnits.Add(unit4_lower);
                    //     }
                    // }
                }
            }
        }

        int level = 0;
        int count = matchedUnits.Count;
        if(count == 0)return;
        matchedUnits = matchedUnits.Distinct().ToList();
        Debug.Log("count:"+count);
        foreach(Unit unit in matchedUnits)
        {
            level += int.Parse(unit.TextMeshPro.text);
            unit.BlockPair.PlayEffect("Effect_ThereMerge");
            unit.BlockPair.OccupiedUnit = null;
            Destroy(unit.gameObject);
        }
        string Sound_ThereMerge = "Sound_ThereMerge";
        Sound.Instance.PlaySoundTemp(Sound_ThereMerge);
        Manager.Petri.InstantiateUnit(type_Main,level.ToString());

    }
}
