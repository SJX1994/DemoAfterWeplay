using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Match3G_PlayerData;
using DG.Tweening;
using TMPro;
public class Match3G_Group_Numerical : MonoBehaviour
{
    [SerializeField]
    // public int maxAC; // Armor Class
    int currentAC;
    public int CurrentAC 
    { 
        get 
        { 
            return currentAC; 
        } 
        set 
        { 
            currentAC = value;
            if(currentAC < 0)currentAC = 0;
            TextMeshAC.text = "AC:" + currentAC.ToString();
        } 
    }

    [SerializeField]
    public int maxHP; // Health Point
    int currentHP;
    public int CurrentHP 
    { 
        get 
        { 
            return currentHP; 
        } 
        set 
        { 
            int damage = value;
            if(CurrentAC > 0 && damage < 0)
            {
                if(CurrentAC > damage)
                {
                    CurrentAC += damage;
                    damage = 0;
                }
                else
                {
                    damage += CurrentAC;
                    CurrentAC = 0;
                }
            }
            currentHP += damage;
            if(currentHP<0)currentHP = 0;
            if(currentHP>maxHP)currentHP = maxHP;
            
            TextMeshHP.text = "HP:" + currentHP.ToString() + "/" + maxHP.ToString();
        } 
    }
    
    [SerializeField]
    public int maxMP; // Magic Point
    int currentMP;
    public int CurrentMP 
    { 
        get 
        { 
            return currentMP; 
        } 
        set 
        { 
            if(Match3G_GroupInfo.Game.Flow.haveHero)
            {
                currentMP = 0;
            }
            else
            {
                currentMP = value;
            }
            
            if(currentMP>maxMP)currentMP = maxMP;
            TextMeshMP.text = currentMP.ToString() + "/" + maxMP.ToString() + ":MP";
            Group.Egg.CompletedLight.intensity = (float)currentMP / (float)maxMP * 5f;
        } 
    }
    TextMeshPro textMeshMP;
    TextMeshPro TextMeshMP 
    { 
        get 
        { 
            if (textMeshMP == null) 
                textMeshMP = transform.Find("MP").GetComponent<TextMeshPro>();
            return textMeshMP; 
        } 
    }
    TextMeshPro textMeshHP;
    TextMeshPro TextMeshHP 
    { 
        get 
        { 
            if (textMeshHP == null) 
                textMeshHP = transform.Find("HP").GetComponent<TextMeshPro>();
            return textMeshHP; 
        } 
    }
    TextMeshPro textMeshAC;
    TextMeshPro TextMeshAC 
    { 
        get 
        { 
            if (textMeshAC == null) 
                textMeshAC = transform.Find("AC").GetComponent<TextMeshPro>();
            return textMeshAC; 
        } 
    }
    public List<SingleEnegy> enegys = new();
    Match3G_FloatingScore floatingScorePrefab;
    public Match3G_FloatingScore FloatingScorePrefab 
    { 
        get 
        { 
            if (floatingScorePrefab == null) 
                floatingScorePrefab = Resources.Load<Match3G_FloatingScore>("FloatingScore");
                floatingScorePrefab = Instantiate(floatingScorePrefab);
            return floatingScorePrefab; 
        } 
    }
    Match3G_Group group;
    public Match3G_Group Group 
    { 
        get 
        { 
            if (group == null) 
                group = GetComponent<Match3G_Group>();
            return group; 
        } 
    }
    public int enegyMultiplier = 1;
    void Awake()
    {
        CurrentHP = maxHP;
        CurrentMP = 0;
    }
    public void ClearEnegys()
    {
        enegys.Clear();
    }
    public void AddEnegy(Vector3 positionFrom,Vector3 positionTo, int valueIn,Match3G_Egg target)
    {
        AddEnegy_Logic(positionFrom,positionTo,valueIn);
        AddEnegy_View(target);
    }
    void AddEnegy_Logic(Vector3 positionFrom,Vector3 positionTo, int valueIn)
    {
        var score = new SingleEnegy
		{
			positionFrom = positionFrom,
            positionTo = positionTo,
			value = valueIn * enegyMultiplier++
		};
		enegys.Add(score);
    }
    void AddEnegy_View(Match3G_Egg target)
    {
        for (int i = 0; i < enegys.Count; i++)
		{
			SingleEnegy score = enegys[i];
			FloatingScorePrefab.Show(score.positionFrom,score.positionTo,score.value,this,Color.blue);
		}
    }
}