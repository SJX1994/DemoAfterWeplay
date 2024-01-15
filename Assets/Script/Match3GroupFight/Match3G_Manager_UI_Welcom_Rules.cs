using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Demo.Scripts;
using System.Linq;
using UnityEngine.UI;
using DG.Tweening;
using Match3G_PlayerData;
public class Match3G_Manager_UI_Welcom_Rules : MonoBehaviour
{
    [SerializeField] private DemoCarouselView _carouselView;
    [SerializeField] [Range(1, 5)] private int _bannerCount = 5;
    Image image_background;
    Image Image_Background
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
    private bool _isSetup = false;
    bool showing = false;
    // void Start()
    // {
    //     Show();
    // }
    public void Show()
    {
        foreach (var item in Match3G_GroupInfo.UI.Welcom_SubUIs)
        {
            if(item.activeSelf)
            {
                item.TryGetComponent(out Match3G_Manager_UI_Welcome_SelectMode subUI);
                if(subUI)subUI.Hide();
                item.TryGetComponent(out Match3G_Manager_UI_Welcom_Rules subUI2);
                if(subUI2)subUI2.Hide();
            }
        }
        if(showing)return;
        showing = true;
        string Button = "Match3G_wav/Button";
        Sound.Instance.PlaySoundTemp(Button);
        gameObject.SetActive(showing);
        Setup();
        Image_Background.transform.localScale = new Vector3(1, 0, 1);
        Image_Background.transform.DOScaleY(1, 0.3f).SetEase(Ease.OutBack);
    }
    public void Hide()
    {
        if(!showing)return;
        showing = false;
        string Swishe_banner = "Match3G_wav/Swishe_banner";
        Sound.Instance.PlaySoundTemp(Swishe_banner);
        gameObject.SetActive(showing);
        
    }
    private void Setup()
    {
        if (_isSetup)
            return;
        var items = Enumerable.Range(0, _bannerCount)
            .Select(i =>
            {
                // var spriteResourceKey = $"tex_demo_banner_{i:D2}";
                var spriteResourceKey = $"Rules_{i:D2}";
                var text = $"Demo Banner {i:D2}";
                return new DemoData(spriteResourceKey, text, () => Debug.Log($"Clicked: {text}"));
            })
            .ToArray();
        _carouselView.Setup(items);
        _isSetup = true;
    }
}
