using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Match3G_PlayerData;
using System;
using TMPro;
using System.Linq;
public class Match3G_Manager_UI_MatchFinish : MonoBehaviour
{
    [SerializeField]
    List<Sprite> sprite_WhichLevels;
    [SerializeField]
    Sprite sprite_Mask_Effect_Win_Red;
    [SerializeField]
    Sprite sprite_Mask_Effect_Win_Blue;
    [SerializeField]
    Sprite sprite_Image_WhichSideText_Red;
    [SerializeField]
    Sprite sprite_Image_WhichSideText_Blue;

    Image image_MidLine;
    Image Image_MidLine
    {
        get
        {
            if (image_MidLine == null)
            {
                image_MidLine = transform.Find("Image_MidLine").GetComponent<Image>();
            }
            return image_MidLine;
        }
    }
    Image image_MidIcon;
    Image Image_MidIcon
    {
        get
        {
            if (image_MidIcon == null)
            {
                image_MidIcon = transform.Find("Image_MidIcon").GetComponent<Image>();
            }
            return image_MidIcon;
        }
    }
    Image image_SideData_Red;
    Image Image_SideData_Red
    {
        get
        {
            if (image_SideData_Red == null)
            {
                image_SideData_Red = transform.Find("Image_SideData_Red").GetComponent<Image>();
            }
            return image_SideData_Red;
        }
    }
    Image image_SideData_Blue;
    Image Image_SideData_Blue
    {
        get
        {
            if (image_SideData_Blue == null)
            {
                image_SideData_Blue = transform.Find("Image_SideData_Blue").GetComponent<Image>();
            }
            return image_SideData_Blue;
        }
    }
    Image image_SpecificLevel;
    Image Image_SpecificLevel
    {
        get
        {
            if (image_SpecificLevel == null)
            {
                image_SpecificLevel = Image_WhichLevelText.transform.Find("Image_SpecificLevel").GetComponent<Image>();
            }
            return image_SpecificLevel;
        }
    }
    Image image_WhichLevelText;
    Image Image_WhichLevelText
    {
        get
        {
            if (image_WhichLevelText == null)
            {
                image_WhichLevelText = transform.Find("Image_WhichLevelText").GetComponent<Image>();
            }
            return image_WhichLevelText;
        }
    }
    Image image_WhichSideText;
    Image Image_WhichSideText
    {
        get
        {
            if (image_WhichSideText == null)
            {
                image_WhichSideText = transform.Find("Image_WhichSideText").GetComponent<Image>();
            }
            return image_WhichSideText;
        }
    }
    Mask mask_effect;
    Mask Mask_effect
    {
        get
        {
            if (mask_effect == null)
            {
                mask_effect = transform.Find("Mask_Effect_Win").GetComponent<Mask>();
            }
            return mask_effect;
        }
    }
    Image image_EffectWin;
    Image Image_EffectWin
    {
        get
        {
            if (image_EffectWin == null)
            {
                image_EffectWin = Mask_effect.transform.Find("Image_EffectWin").GetComponent<Image>();
            }
            return image_EffectWin;
        }
    }
    float maskEffectScaleValue = 2018f;
    // string starEffectName = "";
    // void Start()
    // {
    //     ShowUp(Match3G_GroupInfo.GroupType.GroupB);
    // }
    Vector3 ParticleSystem_PosStart = new Vector3(0, 8, 0);
    Vector3 ParticleSystem_PosStart_star = new Vector3(1.16f, 0.58f, 0);
    GameObject needToDestroy;
    public void ShowUp(Match3G_GroupInfo.GroupType which)
    {
        // showUpTween?.Kill();
        gameObject.SetActive(true);
        Image_SideData_Red.rectTransform.anchoredPosition = new Vector2(713f, Image_SideData_Red.rectTransform.anchoredPosition.y);
        Image_SideData_Blue.rectTransform.anchoredPosition = new Vector2(-713f, Image_SideData_Blue.rectTransform.anchoredPosition.y);
        Image_WhichSideText.transform.localScale = Vector3.zero;
        Image_WhichLevelText.color = new(Image_WhichLevelText.color.r, Image_WhichLevelText.color.g, Image_WhichLevelText.color.b, 0);
        Image_SpecificLevel.gameObject.SetActive(false);
        Image_MidIcon.gameObject.SetActive(false);
        Image_MidIcon.transform.localScale = Vector3.one*4.84f;
        Image_SpecificLevel.transform.localScale = Vector3.one*2.5f;
        Image_MidLine.transform.localScale = new Vector3(Image_MidLine.transform.localScale.x, 0, Image_MidLine.transform.localScale.z);
        // 评级公式 = 本局分数/200 + Max(最高连击-3) + 本局消灭/50
        int Red_Level = 0;
        List<TextMeshProUGUI> texts = Image_SideData_Red.transform.GetComponentsInChildren<TextMeshProUGUI>().ToList();
        for(int i = 0; i < texts.Count; i++)
        {
            texts[i].rectTransform.anchoredPosition = new Vector2(564f, texts[i].rectTransform.anchoredPosition.y);
            switch(i)
            {
                case 0:
                    texts[i].text ="本局分数：" + Match3G_GroupInfo.match3G_SavingData_round_red.highScore.ToString() + "分";
                    Red_Level += Match3G_GroupInfo.match3G_SavingData_round_red.highScore/200;
                    break;
                    
                case 1:
                    texts[i].text = "最高连击：" + Match3G_GroupInfo.match3G_SavingData_round_red.mergeTimes + "次";
                    Red_Level += Mathf.Max(0,Match3G_GroupInfo.match3G_SavingData_round_red.mergeTimes-3);
                    break;
                case 2:
                    texts[i].text = "本局消灭：" + Match3G_GroupInfo.match3G_SavingData_round_red.totalKillNumbers + "个";
                    Red_Level += Match3G_GroupInfo.match3G_SavingData_round_red.totalKillNumbers/50;
                    break;
                case 3:
                    float totalPlayTime = Match3G_GroupInfo.match3G_SavingData_round_red.totalPlayTime;
                    int hours = Mathf.FloorToInt(totalPlayTime / 3600);
                    int minutes = Mathf.FloorToInt(totalPlayTime % 3600 / 60);
                    int seconds = Mathf.FloorToInt(totalPlayTime % 60);
                    string timeString = string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds);
                    texts[i].text = "本局时长：" + timeString;
                    break;
            }
        }
        int Blue_Level = 0;
        List<TextMeshProUGUI> texts2 = Image_SideData_Blue.transform.GetComponentsInChildren<TextMeshProUGUI>().ToList();
        for(int i = 0; i < texts2.Count; i++)
        {
            texts2[i].rectTransform.anchoredPosition = new Vector2(-772f, texts2[i].rectTransform.anchoredPosition.y);
            switch(i)
            {
                case 0:
                    texts2[i].text ="本局分数：" + Match3G_GroupInfo.match3G_SavingData_round_blue.highScore.ToString() + "分";
                    Blue_Level += Match3G_GroupInfo.match3G_SavingData_round_blue.highScore/200;
                    break;
                case 1:
                    texts2[i].text = "最高连击：" + Match3G_GroupInfo.match3G_SavingData_round_blue.mergeTimes + "次";
                    Blue_Level += Mathf.Max(0,Match3G_GroupInfo.match3G_SavingData_round_blue.mergeTimes-3);
                    break;
                case 2:
                    texts2[i].text = "本局消灭：" + Match3G_GroupInfo.match3G_SavingData_round_blue.totalKillNumbers + "个";
                    Blue_Level += Match3G_GroupInfo.match3G_SavingData_round_blue.totalKillNumbers/50;
                    break;
                case 3:
                    float totalPlayTime = Match3G_GroupInfo.match3G_SavingData_round_blue.totalPlayTime;
                    int hours = Mathf.FloorToInt(totalPlayTime / 3600);
                    int minutes = Mathf.FloorToInt(totalPlayTime % 3600 / 60);
                    int seconds = Mathf.FloorToInt(totalPlayTime % 60);
                    string timeString = string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds);
                    texts2[i].text = "本局时长：" + timeString;
                    break;
            }
        }
        int level = which == Match3G_GroupInfo.GroupType.GroupA ? Blue_Level : Red_Level;
        if(level > sprite_WhichLevels.Count-1)level = sprite_WhichLevels.Count-1;
        Image_SpecificLevel.sprite = sprite_WhichLevels[level];
        Image_SpecificLevel.SetNativeSize();
        string starEffectName = which == Match3G_GroupInfo.GroupType.GroupA ? "UI_ParticleSystem_BlueStar" : "UI_ParticleSystem_RedStar";
        Image_EffectWin.sprite = which == Match3G_GroupInfo.GroupType.GroupA ? sprite_Mask_Effect_Win_Blue : sprite_Mask_Effect_Win_Red;
        Image_WhichSideText.sprite = which == Match3G_GroupInfo.GroupType.GroupA ? sprite_Image_WhichSideText_Blue : sprite_Image_WhichSideText_Red;
        RectTransform Mask_Effect_rectTransform = Mask_effect.GetComponent<RectTransform>();
        Mask_Effect_rectTransform.sizeDelta = new Vector2(172.43f, Mask_Effect_rectTransform.sizeDelta.y);
        // --- 
        
        // --- 数据表现 ---
        Image_SideData_Blue.rectTransform.DOAnchorPosX(-215f,0.5f).SetEase(Ease.OutBounce);
        Tween showUpTween5 = Image_SideData_Red.rectTransform.DOAnchorPosX(204f,0.5f).SetEase(Ease.OutBounce);
        showUpTween5.onComplete += () => {
            Image_MidIcon.gameObject.SetActive(true);
        };
        Tween showUpTween6 = Image_MidIcon.transform.DOScale(0.8f, 0.5f).SetEase(Ease.OutBounce);
        showUpTween6.onComplete += () => {
            string Block_jelly_choco_destroy = "Match3G_wav/Block_jelly_choco_destroy";
            Sound.Instance.PlaySoundTemp(Block_jelly_choco_destroy);
        };
        Tween showUpTween7 = Image_MidLine.transform.DOScaleY(0.8f, 0.5f).SetEase(Ease.InSine).SetDelay(0.5f);
        showUpTween7.onComplete += () => {
            string Get_star_ingredient = "Match3G_wav/Get_star_ingredient";
            Sound.Instance.PlaySoundTemp(Get_star_ingredient);
        };
        float delayDuration_showUpTween7 = 2.0f;
        showUpTween7.onComplete += () => {
            List<TextMeshProUGUI> texts = Image_SideData_Red.transform.GetComponentsInChildren<TextMeshProUGUI>().ToList();
            for(int i = 0; i < texts.Count; i++)
            {
                texts[i].rectTransform.DOAnchorPosX(0f,0.25f).SetEase(Ease.OutBounce).SetDelay(0.25f*i);
                
                string spin_bonus = "Match3G_wav/spin_bonus";
                Sound.Instance.PlaySoundTemp(spin_bonus,1,0.35f*i);
            }
            List<TextMeshProUGUI> texts2 = Image_SideData_Blue.transform.GetComponentsInChildren<TextMeshProUGUI>().ToList();
            for(int i = 0; i < texts2.Count; i++)
            {
                texts2[i].rectTransform.DOAnchorPosX(-226f,0.25f).SetEase(Ease.OutBounce).SetDelay(0.25f*i);
                string spin_bonus = "Match3G_wav/spin_bonus";
                Sound.Instance.PlaySoundTemp(spin_bonus,1,0.25f*i);
            }
        };
        
        Tween showUpTween = Mask_Effect_rectTransform.DOSizeDelta(new(maskEffectScaleValue,Mask_Effect_rectTransform.sizeDelta.y), 0.5f).SetDelay(delayDuration_showUpTween7);
        showUpTween.onComplete += () => {
            string wind_gust = "Match3G_wav/wind_gust";
            Sound.Instance.PlaySoundTemp(wind_gust);
        };
        Tween showUpTween2 = Image_WhichSideText.transform.DOScale(0.75f, 0.5f).SetEase(Ease.OutBounce);
        showUpTween2.onComplete += () => {
            needToDestroy = ParticleLoader.Instance.PlayParticleTemp(starEffectName, ParticleSystem_PosStart_star, new Vector3(-90, 0, 0));
            string Cheers = "Match3G_wav/Cheers";
            Sound.Instance.PlaySoundTemp(Cheers);
        };

        Tween showUpTween3 = Image_WhichLevelText.DOFade(1, 0.5f).SetDelay(1.25f);
        showUpTween3.onComplete += () => {
            Image_SpecificLevel.gameObject.SetActive(true);
        };
        Tween showUpTween4 = Image_SpecificLevel.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBounce);
        showUpTween4.onComplete += () => {
            ParticleLoader.Instance.PlayParticleTemp("UI_ParticleSystem_VictoryEffect", ParticleSystem_PosStart, new Vector3(90, 0, 0));
            string Level_complete = "Match3G_wav/Level_complete";
            Sound.Instance.PlaySoundTemp(Level_complete);
        };
        Sequence sequence2 = DOTween.Sequence();
        sequence2
            .Append(showUpTween5)
            .Append(showUpTween6)
            .Append(showUpTween7)
            .Append(showUpTween)
            .Append(showUpTween2)
            .Append(showUpTween3)
            .Append(showUpTween4);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void BackToMenu()
    {
        
        string Drop = "Match3G_wav/Drop";
        Sound.Instance.PlaySoundTemp(Drop);
        if(needToDestroy)Destroy(needToDestroy);
        Match3G_Tool.BackToMenu();
    }
}
