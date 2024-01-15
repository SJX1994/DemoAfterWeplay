using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match3G_Manager_UI : MonoBehaviour
{
    public Match3G_Manager_UI_Round Round;
    public Match3G_Manager_UI_RoundTimer Timer;
    public Match3G_Manager_UI_HealthBar HealthBar_Red; 
    public Match3G_Manager_UI_HealthBar HealthBar_Blue;
    public Match3G_Manager_UI_Praise Praise;
    public Match3G_Manager_UI_Developing Developing;
    public Match3G_Manager_UI_SavedData SavedData;
    public Match3G_Manager_UI_Welcome Welcome;
    public Match3G_Manager_UI_Welcome_SelectMode Welcome_SelectMode;
    public Match3G_Manager_UI_Welcom_Rules Welcom_Rules;
    public Match3G_Manager_UI_MatchingTools MatchingTools;
    public Match3G_Manager_UI_Setting_OutMatch Setting_OutMatch;
    public Match3G_Manager_UI_Setting_InMatch Setting_InMatch;
    public Match3G_Manager_UI_MatchFinish MatchFinish;
    public List<GameObject> Welcom_SubUIs = new();
    void Start()
    {
        Welcom_SubUIs.Add(Welcome_SelectMode.gameObject);
        Welcom_SubUIs.Add(Welcom_Rules.gameObject);
    }
}
