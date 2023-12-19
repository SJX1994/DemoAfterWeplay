using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
public class DNA_Monstor : MonoBehaviour
{
    private DNA_Manager manager;
    public DNA_Manager Manager
    {
        get
        {
            if(manager == null)manager = FindObjectOfType<DNA_Manager>();
            return manager;
        }
        set
        {
            manager = value;
        }
    }
    private Shake shake;
    public Shake Shake
    {
        get
        {
            if(shake == null)shake = GetComponent<Shake>();
            return shake;
        }
        set
        {
            shake = value;
        }
    }
    private TextMeshPro textMeshPro;
    public TextMeshPro TextMeshPro
    {
        get
        {
            if(textMeshPro == null)textMeshPro = GetComponentInChildren<TextMeshPro>();
            return textMeshPro;
        }
        set
        {
            textMeshPro = value;
        }
    }
    private Light collectLight;
    public Light CollectLight
    {
        get
        {
            if(collectLight == null)collectLight = GetComponentInChildren<Light>();
            return collectLight;
        }
        set
        {
            collectLight = value;
        }
    }
    Tween tween_light;
    public void CollectDisplay(Color color)
    {
        if(TextMeshPro.text == ">3 Better!")TextMeshPro.text ="1";
       
        string Sound_TopMerge = "Sound_TopMerge";
        Sound.Instance.PlaySoundTemp(Sound_TopMerge);
        Shake.ShakeObjectPosition();
        CollectLight.color = color;
        tween_light?.Kill();
        tween_light = CollectLight.DOIntensity(1.5f, 0.21f);
        tween_light.OnComplete(() =>
        {
            tween_light = CollectLight.DOIntensity(0.0f, 0.21f);
        });
        if(Manager.Mouse.selectedDnas.Count == 0)return;
        TextMeshPro.text = (int.Parse(TextMeshPro.text) * Manager.Mouse.selectedDnas.Count).ToString();
    }
   
}
