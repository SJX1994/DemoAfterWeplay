using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Match3G_PlayerData;

public class Match3G_Manager_Welcome : MonoBehaviour
{
    GameObject backGround_WelcomPage;
    GameObject BackGround_WelcomPage
    {
        get
        {
            if (backGround_WelcomPage == null)
            {
                backGround_WelcomPage = transform.Find("BackGround_WelcomPage").gameObject;
            }
            return backGround_WelcomPage;
        }
    }
    public void ShowPage()
    {
        BackGround_WelcomPage.SetActive(true);
        Match3G_GroupInfo.UI.Welcome.ShowPage_View();
        Match3G_GroupInfo.UI.Welcome_SelectMode.Hide();
        Match3G_GroupInfo.UI.Welcom_Rules.Hide();
    }
    public void HidePage()
    {
        BackGround_WelcomPage.SetActive(false);
        Match3G_GroupInfo.UI.Welcome.HidePage_View();
        Match3G_GroupInfo.UI.Welcome_SelectMode.Hide();
        Match3G_GroupInfo.UI.Welcom_Rules.Hide();
    }
    public void HideOther()
    {
        
    }

}
