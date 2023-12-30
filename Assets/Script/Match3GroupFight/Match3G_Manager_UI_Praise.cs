using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Match3G_PlayerData;
public class Match3G_Manager_UI_Praise : MonoBehaviour
{
    [SerializeField]
    List<Sprite> sprites;
    Image praise_Text;
    Image Praise_Text 
    { 
        get 
        { 
            if (praise_Text == null) 
                praise_Text = transform.Find("Praise").GetComponent<Image>();
            return praise_Text; 
        } 
    }
    Image praise_Effect;
    Image Praise_Effect 
    { 
        get 
        { 
            if (praise_Effect == null) 
                praise_Effect = transform.Find("Praise_effect").GetComponent<Image>();
            return praise_Effect; 
        } 
    }
    Tween tween_Praise;
    Tween tween_Praise_Effect;
    public void SetPraise(int level)
    {
        Match3G_GroupInfo.Game.Flow.roundTime_Temp += 3f;
        switch(level)
        {
            case 0:
                string Star_win_01 = "Match3G_wav/Star_win_01";
                Sound.Instance.PlaySoundTemp(Star_win_01);
                break;
            case 1:
                string Star_win_02 = "Match3G_wav/Star_win_02";
                Sound.Instance.PlaySoundTemp(Star_win_02);
                break;
            case 2:
                string Star_win_03 = "Match3G_wav/Star_win_03";
                Sound.Instance.PlaySoundTemp(Star_win_03);
            break;
            case 3:
                LoopPlay(2);
            break;
            case 4:
                LoopPlay(3);
            break;
            case 5:
                LoopPlay(4);
            break;
            case 6:
                LoopPlay(5);
            break;
            case 7:
                LoopPlay(6);
            break;
        }
        gameObject.SetActive(true);
        float text_scale = 0.5f + level * 0.05f;
        float effect_scale = 4f + level * 0.8f;
        tween_Praise?.Kill();
        tween_Praise_Effect?.Kill();
        Praise_Text.transform.localScale = Vector3.zero;
        Praise_Effect.transform.localScale = Vector3.zero;
        Praise_Text.sprite = sprites[level];
        Praise_Text.SetNativeSize();
        tween_Praise = Praise_Text.transform.DOScale(text_scale,0.25f).SetEase(Ease.OutBounce);
        tween_Praise.onComplete += () => {
            tween_Praise_Effect = Praise_Effect.transform.DOScale(effect_scale,0.5f).SetEase(Ease.OutSine);
            tween_Praise_Effect.onComplete += () => {
                gameObject.SetActive(false);
            };
        };
    }
    void LoopPlay(int times)
    {
        for(int i = 0; i < times; i++)
        {
            string Star_win_03 = "Match3G_wav/Star_win_03";
            Sound.Instance.PlaySoundTemp(Star_win_03,1,i*0.1f+Random.Range(0.0f,0.5f));
        }
    }
}
