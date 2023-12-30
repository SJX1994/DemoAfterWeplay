using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Match3G_PlayerData;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
public class Match3G_Manager_UI_SavedData : MonoBehaviour
{
    TextMeshProUGUI textMeshProUGUI_macName;
    TextMeshProUGUI TextMeshProUGUI_macName 
    { 
        get 
        { 
            if (textMeshProUGUI_macName == null) 
                textMeshProUGUI_macName = transform.Find("Text_macName").GetComponent<TextMeshProUGUI>();
            return textMeshProUGUI_macName; 
        } 
    }
    TextMeshProUGUI textMeshProUGUI_mergeTimes;
    TextMeshProUGUI TextMeshProUGUI_mergeTimes 
    { 
        get 
        { 
            if (textMeshProUGUI_mergeTimes == null) 
                textMeshProUGUI_mergeTimes = transform.Find("Text_mergeTimes").GetComponent<TextMeshProUGUI>();
            return textMeshProUGUI_mergeTimes; 
        } 
    }
    TextMeshProUGUI textMeshProUGUI_totalKillNumbers;
    TextMeshProUGUI TextMeshProUGUI_totalKillNumbers 
    { 
        get 
        { 
            if (textMeshProUGUI_totalKillNumbers == null) 
                textMeshProUGUI_totalKillNumbers = transform.Find("Text_totalKillNumbers").GetComponent<TextMeshProUGUI>();
            return textMeshProUGUI_totalKillNumbers; 
        } 
    }
    TextMeshProUGUI textMeshProUGUI_useHeroTimes;
    TextMeshProUGUI TextMeshProUGUI_useHeroTimes 
    { 
        get 
        { 
            if (textMeshProUGUI_useHeroTimes == null) 
                textMeshProUGUI_useHeroTimes = transform.Find("Text_useHeroTimes").GetComponent<TextMeshProUGUI>();
            return textMeshProUGUI_useHeroTimes; 
        } 
    }
    TextMeshProUGUI textMeshProUGUI_highScore;
    TextMeshProUGUI TextMeshProUGUI_highScore 
    { 
        get 
        { 
            if (textMeshProUGUI_highScore == null) 
                textMeshProUGUI_highScore = transform.Find("Text_highScore").GetComponent<TextMeshProUGUI>();
            return textMeshProUGUI_highScore; 
        } 
    }
    TextMeshProUGUI textMeshProUGUI_totalPlayTime;
    TextMeshProUGUI TextMeshProUGUI_totalPlayTime 
    { 
        get 
        { 
            if (textMeshProUGUI_totalPlayTime == null) 
                textMeshProUGUI_totalPlayTime = transform.Find("Text_totalPlayTime").GetComponent<TextMeshProUGUI>();
            return textMeshProUGUI_totalPlayTime; 
        } 
    }

    private Match3G_SavingData playerData;
    public void DisplayData()
    {
        gameObject.SetActive(true);
        string Cash = "Match3G_wav/Cash";
        Sound.Instance.PlaySoundTemp(Cash);
        LoadData();
        SaveData();
        LoadData();
        // transform.DOJump(transform.position, 10f, 1, 0.5f);
        string macNameShort = playerData.macName;
        int maxLength = 5;
        string truncatedString = macNameShort.Substring(0, Mathf.Min(macNameShort.Length, maxLength));
        if (macNameShort.Length > maxLength)
        {
            truncatedString += "...";
        }
        TextMeshProUGUI_macName.text = "设备:" + truncatedString;
        TextMeshProUGUI_mergeTimes.text = /*"最高连击：" + */playerData.mergeTimes.ToString();
        TextMeshProUGUI_totalKillNumbers.text = /*"共消灭：" + */playerData.totalKillNumbers.ToString();
        TextMeshProUGUI_useHeroTimes.text = /*"共孵化：" + */playerData.useHeroTimes.ToString();
        TextMeshProUGUI_highScore.text = /*"最高分：" + */ playerData.highScore.ToString();
        int hours = Mathf.FloorToInt(playerData.totalPlayTime / 3600);
        int minutes = Mathf.FloorToInt((playerData.totalPlayTime % 3600) / 60);
        int seconds = Mathf.FloorToInt(playerData.totalPlayTime % 60);
        string timeStr = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
        TextMeshProUGUI_totalPlayTime.text = /*"总时长：" +*/ timeStr;
        TextMeshProUGUI_mergeTimes.DOFade(0.7f, Random.Range(1.0f,1.5f)).SetLoops(-1, LoopType.Yoyo);
        TextMeshProUGUI_totalKillNumbers.DOFade(0.7f, Random.Range(1.0f,1.5f)).SetLoops(-1, LoopType.Yoyo);
        TextMeshProUGUI_useHeroTimes.DOFade(0.7f, Random.Range(1.0f,1.5f)).SetLoops(-1, LoopType.Yoyo);
        TextMeshProUGUI_highScore.DOFade(0.7f, Random.Range(1.0f,1.5f)).SetLoops(-1, LoopType.Yoyo);
        TextMeshProUGUI_totalPlayTime.DOFade(0.7f, Random.Range(1.0f,1.5f)).SetLoops(-1, LoopType.Yoyo);
    }
    public void HideData()
    {
        gameObject.SetActive(false);
    }
    public void LoadData()
    {
        playerData = new Match3G_SavingData(
            PlayerPrefs.GetString(Match3G_SavingData.macNameKey), 
            PlayerPrefs.GetInt(Match3G_SavingData.mergeTimesKey), 
            PlayerPrefs.GetInt(Match3G_SavingData.totalKillNumbersKey), 
            PlayerPrefs.GetInt(Match3G_SavingData.useHeroTimesKey),
            PlayerPrefs.GetInt(Match3G_SavingData.highScoreKey),
            PlayerPrefs.GetFloat(Match3G_SavingData.totalPlayTimeKey)
            );
        Debug.Log(playerData.ToString());
    }
    public void MergeData()
    {
        playerData.mergeTimes = Mathf.Max(playerData.mergeTimes, Match3G_GroupInfo.match3G_SavingData_temp.mergeTimes);
        playerData.totalKillNumbers += Match3G_GroupInfo.match3G_SavingData_temp.totalKillNumbers;
        playerData.useHeroTimes += Match3G_GroupInfo.match3G_SavingData_temp.useHeroTimes;
        playerData.highScore = Mathf.Max(playerData.highScore, Match3G_GroupInfo.match3G_SavingData_temp.highScore);
        playerData.totalPlayTime += Match3G_GroupInfo.match3G_SavingData_temp.totalPlayTime; 
        Match3G_GroupInfo.match3G_SavingData_temp = new("", 0, 0, 0, 0, 0f);
    }
    public void SaveData()
    {
        MergeData();
        string machineCode = "notFind";
    #if UNITY_EDITOR
         machineCode = System.Environment.MachineName;
        // Debug.Log("Running in Unity Editor");
    #elif UNITY_STANDALONE_WIN
         machineCode = System.Environment.MachineName;
        // Debug.Log("Running on Windows");
    #elif UNITY_STANDALONE_OSX
         machineCode = SystemInfo.deviceUniqueIdentifier;
        // Debug.Log("Running on macOS");
    #elif UNITY_ANDROID
         machineCode = SystemInfo.deviceUniqueIdentifier;
        // Debug.Log("Running on Android");
    #elif UNITY_IOS
         machineCode = UnityEngine.iOS.Device.vendorIdentifier;
        // Debug.Log("Running on iOS");
    #else
        machineCode = "Unsupported";
        // Debug.Log("Unsupported platform");
    #endif
        playerData.macName = machineCode;
        PlayerPrefs.SetString(Match3G_SavingData.macNameKey, playerData.macName);
        PlayerPrefs.SetInt(Match3G_SavingData.mergeTimesKey, playerData.mergeTimes);
        PlayerPrefs.SetInt(Match3G_SavingData.totalKillNumbersKey, playerData.totalKillNumbers);
        PlayerPrefs.SetInt(Match3G_SavingData.useHeroTimesKey, playerData.useHeroTimes);
        PlayerPrefs.SetInt(Match3G_SavingData.highScoreKey, playerData.highScore);
        PlayerPrefs.SetFloat(Match3G_SavingData.totalPlayTimeKey, playerData.totalPlayTime);
        PlayerPrefs.Save();
    }
    public void DeletData()
    {
        // PlayerPrefs.DeleteKey(Match3G_SavingData.macNameKey);
        PlayerPrefs.DeleteKey(Match3G_SavingData.mergeTimesKey);
        PlayerPrefs.DeleteKey(Match3G_SavingData.totalKillNumbersKey);
        PlayerPrefs.DeleteKey(Match3G_SavingData.useHeroTimesKey);
        PlayerPrefs.DeleteKey(Match3G_SavingData.highScoreKey);
        PlayerPrefs.DeleteKey(Match3G_SavingData.totalPlayTimeKey);
        PlayerPrefs.Save();
        DisplayData();
    }
}
