using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Match3G_PlayerData;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class Match3G_Manager_UI_Setting_InMatch : MonoBehaviour
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
    float scaleValue = 0.94f;
    int TimeScale = 1;
    public void ShowUp()
    {
        string Button = "Match3G_wav/Button";
        Sound.Instance.PlaySoundTemp(Button);
        gameObject.SetActive(true);
        Image_background.transform.localScale = Vector3.zero;
        Image_background.transform.DOScale(scaleValue, 0.25f).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            Time.timeScale = 0;
        });
        
    }
    public void Hide()
    {
        Time.timeScale = TimeScale;
        string Swishe_banner = "Match3G_wav/Swishe_banner";
        Sound.Instance.PlaySoundTemp(Swishe_banner);
        gameObject.SetActive(false);
    }
    public void SetDoubleSpeed()
    {
        if (Toggle_doubleSpeed.isOn)
        {
            TimeScale = 2;
        }
        else
        {
            TimeScale = 1;
        }
    }
    public void BackToMenu()
    {
        Match3G_Tool.BackToMenu();
    }
}
