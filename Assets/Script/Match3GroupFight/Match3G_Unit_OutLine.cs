using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Match3G_PlayerData;
using System;
using TMPro;

public class Match3G_Unit_OutLine : MonoBehaviour
{
    public SpriteRenderer spriteRenderer_description_BackGround;
    public TextMeshPro textMeshPro_description_text;
    [HideInInspector]
    public string Description = "";
    float scaleFrom = 0.75f;
    float scaleTo = 1.0f;
    float longPressThreshold_seconds = 0.5f; 
    float pressTime = 0f; 
   
    public void Show(Vector3 pos)
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        pos.y += 0.02f;
        pos.z += 0.1f;
        transform.position = pos;
        pressTime += Time.deltaTime;
        
        if(pressTime >= longPressThreshold_seconds)
        {
            ShowDescription();
        }else
        {
            transform.localScale = Vector3.one * Mathf.Min(transform.localScale.x +  0.15f * Time.deltaTime, scaleTo);
            
        }
    }
    public void ShowDescription()
    {
        spriteRenderer_description_BackGround.transform.position = new Vector3(0, transform.position.y + 1.36f, -0.99f);
        spriteRenderer_description_BackGround.gameObject.SetActive(true);
        textMeshPro_description_text.text = Description;
        Time.timeScale = 0.01f;
        
    }
    public void HideDescription()
    {
        Time.timeScale = Match3G_GroupInfo.globalTimeScale;
        spriteRenderer_description_BackGround.gameObject.SetActive(false);
        transform.localScale = Vector3.one * scaleFrom;
        pressTime = 0f;
    }
    public void Hide()
    {
        transform.localScale = Vector3.one * scaleFrom;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }
}
