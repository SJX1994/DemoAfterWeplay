using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
public class Match3G_Manager_UI_DamagerSettlement : MonoBehaviour
{
    Image image_background;
    public Image Image_background 
    { 
        get 
        { 
            if (image_background == null) 
                image_background = transform.Find("Image").GetComponent<Image>();
            return image_background; } 
    }
    TextMeshProUGUI  textMeshPro;
    public TextMeshProUGUI  TextMeshPro 
    { 
        get 
        { 
            if (textMeshPro == null) 
                textMeshPro = transform.Find("Text").GetComponent<TextMeshProUGUI>();
            return textMeshPro; } 
    }
    Tween textTween;
    Tween bgTween;
    static float delayDuration = 2f;
    public void Show(int damage)
    {
        gameObject.SetActive(true);
        textTween?.Kill();
        bgTween?.Kill();
        Image_background.transform.localScale = Vector3.zero;
        TextMeshPro.transform.localScale = Vector3.zero;
        TextMeshPro.text = "-" + damage.ToString();
        bgTween = Image_background.transform.DOScale(2.37f,0.25f).SetEase(Ease.OutBack);
        bgTween.SetEase(Ease.OutExpo);
        bgTween.onComplete += () => {
            textTween = TextMeshPro.transform.DOScale(1f,0.25f).SetEase(Ease.OutBack);
            textTween.SetEase(Ease.OutExpo);
            textTween.onComplete += () => {
                bgTween = Image_background.transform.DOScale(0.35f,1.5f).SetEase(Ease.InBack).SetDelay(delayDuration);
                bgTween.onComplete += () => {
                    gameObject.SetActive(false);
                };
            };
        };
        
    }
}
