using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Match3G_PlayerData;
public class Match3G_Manager_Welcome_SelectMode : MonoBehaviour
{
    RectTransform rectTransform_self;
    RectTransform RectTransform_Self
    {
        get
        {
            if (rectTransform_self == null)
            {
                rectTransform_self = transform.Find("Image_background").GetComponent<RectTransform>();
            }
            return rectTransform_self;
        }
    }
    Button button_2P;
    Button Button_2P
    {
        get
        {
            if (button_2P == null)
            {
                button_2P = RectTransform_Self.Find("Button_2P").GetComponent<Button>();
            }
            return button_2P;
        }
    }
    Button button_1P_simple;
    Button Button_1P_simple
    {
        get
        {
            if (button_1P_simple == null)
            {
                button_1P_simple = RectTransform_Self.Find("Button_1P_simple").GetComponent<Button>();
            }
            return button_1P_simple;
        }
    }
    Button button_1P_hard;
    Button Button_1P_hard
    {
        get
        {
            if (button_1P_hard == null)
            {
                button_1P_hard = RectTransform_Self.Find("Button_1P_hard").GetComponent<Button>();
            }
            return button_1P_hard;
        }
    }
    Tween tween_Self;
    public bool showing = false;
    private void Start()
    {
        Button_2P.onClick.AddListener(On2P_ButtonClick);
        Button_1P_simple.onClick.AddListener(On1P_simple_ButtonClick);
        Button_1P_hard.onClick.AddListener(On1P_hard_ButtonClick);
    }
    void On2P_ButtonClick()
    {
        var fsm = Match3G_GroupInfo.Game.Flow.FSM;
        Match3G_GroupInfo.playMode = Match3G_GroupInfo.PlayMode.TwoPlayer;
	    fsm.ChangeState(Match3G_Manager_Flow.States.AddEnergy);
        string Button = "Match3G_wav/Button";
        Sound.Instance.PlaySoundTemp(Button);
    }
    void On1P_simple_ButtonClick()
    {
        var fsm = Match3G_GroupInfo.Game.Flow.FSM;
        Match3G_GroupInfo.Game.Flow.automaticPlay = true;
        Match3G_GroupInfo.playMode = Match3G_GroupInfo.PlayMode.Easy;
		fsm.ChangeState(Match3G_Manager_Flow.States.AddEnergy);
        string Button = "Match3G_wav/Button";
        Sound.Instance.PlaySoundTemp(Button);
    }
    void On1P_hard_ButtonClick()
    {
        var fsm = Match3G_GroupInfo.Game.Flow.FSM;
        Match3G_GroupInfo.Game.Flow.automaticPlay = true;
        Match3G_GroupInfo.playMode = Match3G_GroupInfo.PlayMode.Hard;
		fsm.ChangeState(Match3G_Manager_Flow.States.AddEnergy);
        string Button = "Match3G_wav/Button";
        Sound.Instance.PlaySoundTemp(Button);
    }
   public void ShowUp()
   {
    if(showing)return;
    showing = true;
    gameObject.SetActive(showing);
    tween_Self?.Kill();
    RectTransform_Self.localScale = new Vector3(1, 0, 1);
    tween_Self = RectTransform_Self.DOScaleY(1, 0.3f).SetEase(Ease.OutBack);
    string Button = "Match3G_wav/Button";
    Sound.Instance.PlaySoundTemp(Button);
   }
   public void Hide()
   {
    if(!showing)return;
    showing = false;
    string Swishe_banner = "Match3G_wav/Swishe_banner";
    Sound.Instance.PlaySoundTemp(Swishe_banner);
    gameObject.SetActive(showing);
   }
}
