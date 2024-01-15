using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;
using Match3G_PlayerData;
public class Match3G_Manager_UI_Welcome : MonoBehaviour
{
    RectTransform rectTransform_upper;
    RectTransform RectTransform_Upper
    {
        get
        {
            if (rectTransform_upper == null)
            {
                rectTransform_upper = transform.Find("Image_background_upper").GetComponent<RectTransform>();
            }
            return rectTransform_upper;
        }
    }
    RectTransform rectTransform_lower;
    RectTransform RectTransform_Lower
    {
        get
        {
            if (rectTransform_lower == null)
            {
                rectTransform_lower = transform.Find("Image_background_lower").GetComponent<RectTransform>();
            }
            return rectTransform_lower;
        }
    }
    Button button_startGame;
    Button Button_StartGame
    {
        get
        {
            if (button_startGame == null)
            {
                button_startGame = transform.Find("Button_startGame").GetComponent<Button>();
            }
            return button_startGame;
        }
    }
    Button button_rules;
    Button Button_Rules
    {
        get
        {
            if (button_rules == null)
            {
                button_rules = transform.Find("Button_rules").GetComponent<Button>();
            }
            return button_rules;
        }
    }
    const float UperFromY = 216f;
    const float UperToY = 0f;
    const float LowerFromY = -139f;
    const float LowerToY = 0f;
    const float Duration = 0.5f;
    Tween UperTween;
    Tween LowerTween;
    public void ShowPage_View()
    {
        gameObject.SetActive(true);
        string Create_bonus = "Match3G_wav/Create_bonus";
        Sound.Instance.PlaySoundTemp(Create_bonus);
        UperTween? .Kill();
        LowerTween? .Kill();
        RectTransform_Upper.anchoredPosition = new Vector3(0, UperFromY, 0);
        RectTransform_Lower.anchoredPosition = new Vector3(0, LowerFromY, 0);
        
        UperTween = RectTransform_Upper.DOAnchorPosY(UperToY, Duration).SetEase(Ease.OutSine);
        LowerTween = RectTransform_Lower.DOAnchorPosY(LowerToY, Duration).SetEase(Ease.OutSine);
        LowerTween.onComplete = () =>
        {
            Button_StartGame.gameObject.SetActive(true);
            Button_StartGame.transform.localScale = Vector3.one*0.65f;
            Button_StartGame.transform.DOScale(Vector3.one*0.7f, 0.5f).SetEase(Ease.OutBounce).onComplete = () =>
            {
                Button_Rules.gameObject.SetActive(true);
                Button_Rules.transform.localScale = Vector3.one*0.45f;
                Button_Rules.transform.DOScale(Vector3.one*0.54f, 0.5f).SetEase(Ease.OutBounce).onComplete = () =>
                {
                    Match3G_GroupInfo.UI.SavedData.DisplayData();
                    Match3G_GroupInfo.UI.Welcom_Rules.Show();
                };
                
            };
        };
    }
    public void HidePage_View()
    {
        string Create_bonus = "Match3G_wav/Create_bonus";
        Sound.Instance.PlaySoundTemp(Create_bonus);
        Button_StartGame.gameObject.SetActive(false);
        Button_Rules.gameObject.SetActive(false);
        Match3G_GroupInfo.UI.SavedData.HideData();
        UperTween? .Kill();
        LowerTween? .Kill();
        RectTransform_Upper.anchoredPosition = new Vector3(0, UperToY, 0);
        RectTransform_Lower.anchoredPosition = new Vector3(0, LowerToY, 0);
        UperTween = RectTransform_Upper.DOAnchorPosY(UperFromY, Duration).SetEase(Ease.OutSine);
        LowerTween = RectTransform_Lower.DOAnchorPosY(LowerFromY, Duration).SetEase(Ease.OutSine);
        LowerTween.onComplete = () =>
        {
            gameObject.SetActive(false);
        };
    }
}
