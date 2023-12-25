using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Match3G_PlayerData;
public class Match3G_Manager_UI_RoundTimer : MonoBehaviour
{
    Slider slider;
    public Slider Slider 
    { 
        get 
        { 
            if (slider == null) 
                slider = GetComponent<Slider>();
            return slider; } 
    }
    public void UpdateTimer(float currentTime,float maxTime)
    {
        Slider.value = currentTime / maxTime;
    }
}
