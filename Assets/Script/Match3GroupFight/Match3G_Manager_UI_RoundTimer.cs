using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Match3G_PlayerData;
public class Match3G_Manager_UI_RoundTimer : MonoBehaviour
{
    [SerializeField]
    Color startColor;
    [SerializeField]
    Color endColor;
    Slider slider;
    public bool playerWarning = false;
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
        Color interpolatedColor = Color.Lerp(endColor,startColor, Slider.value);
        Slider.fillRect.GetComponent<Image>().color = interpolatedColor;
        if(Slider.value < 0.15f && !playerWarning)
        {
            string Warning_time = "Match3G_wav/Warning_time";
            Sound.Instance.PlaySoundTemp(Warning_time);
            playerWarning = true;
        }
    }
}
