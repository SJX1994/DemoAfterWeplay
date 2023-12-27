using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Match3_PlayerData;
using Match3G_PlayerData;
public class Match3G_Manager_UI_Developing : MonoBehaviour
{
   RectTransform rectTransform_dev;
    RectTransform RectTransform_dev
    {
         get
         {
              if(rectTransform_dev == null)
              {
                rectTransform_dev = transform.Find("Panel_Developing").GetComponent<RectTransform>();
              }
              return rectTransform_dev;
         }
    }
   public void DevelpingShow()
   {
          string Package = "Match3G_wav/Package";
          Sound.Instance.PlaySoundTemp(Package);
          Match3G_GroupInfo.ShowMask = true;
          RectTransform_dev.gameObject.SetActive(true);
   }
   public void DevelpingHide()
   {
          
          string Swishe_banner_02 = "Match3G_wav/Swishe_banner_02";
          Sound.Instance.PlaySoundTemp(Swishe_banner_02);
          Match3G_GroupInfo.ShowMask = false;
          RectTransform_dev.gameObject.SetActive(false);
   }
   void Update()
   {
       if(Input.GetMouseButtonDown(0) && Match3G_GroupInfo.ShowMask)
       {
           DevelpingHide();
       }
   }
}
