using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Match3G_PlayerData;
public class Match3G_Manager_UI_HealthBar : MonoBehaviour
{
    Match3G_Manager_UI_DamagerSettlement damagerSettlement;
    public Match3G_Manager_UI_DamagerSettlement DamagerSettlement 
    { 
        get 
        { 
            if (damagerSettlement == null) 
                damagerSettlement = GetComponentInChildren<Match3G_Manager_UI_DamagerSettlement>(true);
            return damagerSettlement; 
        } 
    }
    public Match3G_GroupInfo.GroupType groupeBelong;
    RectMask2D mask_helthBar;
    RectMask2D Mask_helthBar 
    { 
        get 
        { 
            if (mask_helthBar == null) 
                mask_helthBar = transform.Find("Mask_HP").GetComponent<RectMask2D>();
            return mask_helthBar; 
        } 
    }
    RectMask2D mask_armorBar;
    RectMask2D Mask_armorBar 
    { 
        get 
        { 
            if (mask_armorBar == null) 
                mask_armorBar = transform.Find("Mask_AC").GetComponent<RectMask2D>();
            return mask_armorBar; 
        } 
    }
    public void SetHelthBar(float value)
    {
        Mask_helthBar.padding = new Vector4(0, 0, value, 0);
    }
    public void SetArmorBar(float value)
    {
        Mask_armorBar.padding = new Vector4(0, 0, value, 0);
    }
}
