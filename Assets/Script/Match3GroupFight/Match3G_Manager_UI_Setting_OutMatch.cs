using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Match3G_PlayerData;
using DG.Tweening;
public class Match3G_Manager_UI_Setting_OutMatch : MonoBehaviour
{
    Image image_background;
    Image Image_background
    {
        get
        {
            if (image_background == null)
            {
                image_background = transform.Find("Image_background").GetComponent<Image>();
            }
            return image_background;
        }
    }   
    Slider slider_musicVolume;
    Slider Slider_musicVolume
    {
        get
        {
            if (slider_musicVolume == null)
            {
                slider_musicVolume = Image_background.transform.Find("Slider_Music").GetComponent<Slider>();
            }
            return slider_musicVolume;
        }
    }
    Slider slider_soundVolume;
    Slider Slider_soundVolume
    {
        get
        {
            if (slider_soundVolume == null)
            {
                slider_soundVolume = Image_background.transform.Find("Slider_Sound").GetComponent<Slider>();
            }
            return slider_soundVolume;
        }
    }
    Toggle toggle_doubleSpeed;
    Toggle Toggle_doubleSpeed
    {
        get
        {
            if (toggle_doubleSpeed == null)
            {
                toggle_doubleSpeed = Image_background.transform.Find("Toggle_doubleSpeed").GetComponent<Toggle>();
            }
            return toggle_doubleSpeed;
        }
    }
    AudioSource music;
    AudioSource Music
    {
        get
        {
            if (music == null)
            {
                music = GameObject.Find("AudioSystem").transform.Find("musicSource").GetComponent<AudioSource>();
            }
            return music;
        }
    }
    float scaleValue = 0.79f;
    public void ShowUp()
    {
        string Button = "Match3G_wav/Button";
        Sound.Instance.PlaySoundTemp(Button);
        gameObject.SetActive(true);
        Slider_musicVolume.value = Match3G_SystemInfo.musicVolume;
        Slider_soundVolume.value = Match3G_SystemInfo.soundVolume;
        Image_background.transform.localScale = new Vector3(0, 0, 0);
        Image_background.transform.DOScale(scaleValue, 0.25f).SetEase(Ease.OutSine);
    }
    public void Hide()
    {
        string Swishe_banner = "Match3G_wav/Swishe_banner";
        Sound.Instance.PlaySoundTemp(Swishe_banner);
        gameObject.SetActive(false);
    }
    public void SetToggle()
    {
        if(Toggle_doubleSpeed.isOn)
        {
            Time.timeScale = 2;
            Match3G_GroupInfo.globalTimeScale = 2;
        }
        else
        {
            Time.timeScale = 1;
            Match3G_GroupInfo.globalTimeScale = 1;
        }
    }
    public void SetMusicVolume()
    {
        Match3G_SystemInfo.musicVolume = Slider_musicVolume.value;
        if(!Music)return;
        Music.volume = Match3G_SystemInfo.musicVolume;
        
    }
    public void SetSoundVolume()
    {
        Match3G_SystemInfo.soundVolume = Slider_soundVolume.value;
    }
}
