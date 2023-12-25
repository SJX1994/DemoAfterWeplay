using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Match3G_PlayerData;
using DG.Tweening;

public class Match3G_Manager_UI_Round : MonoBehaviour
{
    Image image_background;
    public Image Image_background 
    { 
        get 
        { 
            if (image_background == null) 
                image_background = GetComponent<Image>();
            return image_background; } 
    }
    Image image_RoundRed;
    public Image Image_RoundRed 
    { 
        get 
        { 
            if (image_RoundRed == null) 
                image_RoundRed = transform.Find("Image_WhichRound_Red").GetComponent<Image>();
            return image_RoundRed; } 
    }
    Image image_RoundBlue;
    public Image Image_RoundBlue 
    { 
        get 
        { 
            if (image_RoundBlue == null) 
                image_RoundBlue = transform.Find("Image_WhichRound_Blue").GetComponent<Image>();
            return image_RoundBlue; } 
    }
    Image image_round;
    public Image Image_round 
    { 
        get 
        { 
            if (image_round == null) 
                image_round = transform.GetChild(0).GetComponent<Image>();
            return image_round; } 
    }
    Image image_roundText;
    public Image Image_roundText 
    { 
        get 
        { 
            if (image_roundText == null) 
                image_roundText = Image_round.transform.Find("Image_Round_Number_Digits").GetComponent<Image>();
            return image_roundText; 
        } 
    }
    public List<Sprite> roundTexts;
    Vector3 image_round_pos,image_round_scale;
    Vector3 image_roundText_scale;

    void Awake()
    {
        image_round_pos = Image_round.transform.localPosition;
        image_round_scale = Image_round.transform.localScale;
        image_roundText_scale = Image_roundText.transform.localScale;
        
    }
    public float RoundSwitch(Match3G_Group wchichGroup)
    {
        gameObject.SetActive(true);
        float timeToWait = 1;
        Image_round.transform.localPosition = new Vector3(-250f, image_round_pos.y, image_round_pos.z);
        Image_round.transform.localScale = new Vector3(0.0f, image_round_scale.y, image_round_scale.z);
        Image_roundText.transform.localScale = new Vector3(0.0f, image_roundText_scale.y, image_roundText_scale.z);
        if(Match3G_GroupInfo.round>9)Match3G_GroupInfo.round = 9;
        Image_roundText.sprite = roundTexts[Match3G_GroupInfo.round];
        Image_background.color = new Color(1f, 1f, 1f, 0f);
        Image_RoundBlue.transform.localScale = new Vector3(0f, 0f, 0f);
        Image_background.DOFade(1f, timeToWait/2);
        Tween tween_round_pos = Image_round.transform.DOLocalMove(image_round_pos, timeToWait/4).SetEase(Ease.OutSine);
        Tween tween_round_scale = Image_round.transform.DOScale(image_round_scale, timeToWait/3).SetEase(Ease.OutSine);
        tween_round_pos.OnComplete(()=>
        {
            Tween tween_roundText_scale = Image_roundText.transform.DOScale(image_roundText_scale, timeToWait/3).SetEase(Ease.OutBounce);
            
            tween_roundText_scale.OnComplete(()=>
            {
                if(wchichGroup.groupType == Match3G_GroupInfo.GroupType.GroupA)
                {
                    Image_RoundBlue.gameObject.SetActive(true);
                    Image_RoundRed.gameObject.SetActive(false);
                    Image_RoundBlue.transform.DOScale(image_round_scale, timeToWait/3).SetEase(Ease.OutBounce).OnComplete(
                    ()=>
                        {
                            SwitchEnd();
                        }
                    );;
                }
                else
                if(wchichGroup.groupType == Match3G_GroupInfo.GroupType.GroupB)
                {
                    Image_RoundBlue.gameObject.SetActive(false);
                    Image_RoundRed.gameObject.SetActive(true);
                    Image_RoundRed.transform.DOScale(image_round_scale, timeToWait/3).SetEase(Ease.OutBounce).OnComplete(
                    ()=>
                        {
                            SwitchEnd();
                        }
                    );
                }
                
            });
        });
        return timeToWait;

    }
    void SwitchEnd()
    {
        Tween TweenFade= Image_background.DOFade(0f, 0.15f).SetEase(Ease.OutExpo);
        TweenFade.OnComplete(()=>
        {
            gameObject.SetActive(false);
        });
        
    }
}
