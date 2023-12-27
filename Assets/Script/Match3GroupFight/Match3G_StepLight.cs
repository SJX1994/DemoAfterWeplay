using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Match3G_StepLight : MonoBehaviour
{
    GameObject lightBright;
    GameObject LightBright
    {
        get
        {
            if (lightBright == null)
                lightBright = transform.Find("Light").gameObject;
            return lightBright;
        }
    }
    GameObject off;
    GameObject Off
    {
        get
        {
            if (off == null)
                off = transform.Find("Off").gameObject;
            return off;
        }
    }
    GameObject on;
    GameObject On
    {
        get
        {
            if (on == null)
                on = transform.Find("On").gameObject;
            return on;
        }
    }
    public void SetLight(bool isOn)
    {
        LightBright.SetActive(isOn);
        Off.SetActive(!isOn);
        On.SetActive(isOn);
        if(!isOn)return;
        LightBright.transform.DOScale(1.75f,Random.Range(0.4f,0.7f)).SetLoops(-1,LoopType.Yoyo);
    }

}
