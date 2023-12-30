using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Match3G_PlayerData;
using DG.Tweening;
using Unity.Mathematics;
public class Match3G_Manager_BootSystem : MonoBehaviour
{
    public Move PossibleMove;
    private GameObject bootFinger;
    public GameObject BootFinger
    {
        get
        {
            if (bootFinger == null)
                bootFinger = transform.Find("Boot_Finger").gameObject;
            return bootFinger;
        }
    }
    private GameObject bootFram;
    public GameObject BootFram
    {
        get
        {
            if (bootFram == null)
                bootFram = transform.Find("Boot_Fram").gameObject;
            return bootFram;
        }
    }
    Tween tween_path;
    float interval = 7.0f; // 时间间隔
    private float timer = 0.0f; // 计时器变量
    public void BootUpdate()
    {
        timer += Time.deltaTime;

        if (timer >= interval)
        {
            Match3Boot();
            timer = 0.0f;
        }
    }
    public void ResetBoot()
    {
        timer = 0.0f;
        BootFram.gameObject.SetActive(false);
        BootFinger.gameObject.SetActive(false);
    }
    public void HideBoot()
    {
        timer = 0.0f;
        BootFram.gameObject.SetActive(false);
        BootFinger.gameObject.SetActive(false);
    }
    public bool Match3Boot()
    {
        
        if (!Match3G_GroupInfo.Game.automaticPlay) return false;
        if (Match3G_GroupInfo.CurrentGroup.groupType == Match3G_GroupInfo.GroupType.GroupA) return false;
        if (Match3G_GroupInfo.CurrentGroup.NoMoreMove) return false;
        if (Match3G_GroupInfo.Game.IsBusy)return false;
        string Warning_time = "Match3G_wav/Warning_time";
        Sound.Instance.PlaySoundTemp(Warning_time);
        tween_path?.Kill();
        BootFram.gameObject.SetActive(true);
        BootFinger.gameObject.SetActive(true);
        PossibleMove = Move.FindMove(Match3G_GroupInfo.CurrentGroup);
        Match3G_Unit unitFrom = Match3G_GroupInfo.CurrentGroup.Tiles[PossibleMove.From.x, PossibleMove.From.y ];
        Match3G_Unit unitTo = Match3G_GroupInfo.CurrentGroup.Tiles[PossibleMove.To.x, PossibleMove.To.y];
        Vector3 bootFingerStartPos = new Vector3(unitFrom.transform.position.x, unitFrom.transform.position.y, BootFinger.transform.position.z);
        Vector3 bootFingerTargetPos = new Vector3(unitTo.transform.position.x, unitTo.transform.position.y, BootFinger.transform.position.z);
        BootFinger.transform.position = bootFingerStartPos;

        Vector3[] path = new Vector3[] { bootFingerStartPos, bootFingerTargetPos, bootFingerStartPos };
        tween_path = BootFinger.transform.DOPath(path, 1.5f, PathType.CatmullRom)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.Linear);
        
        float mergeLeft = 90;
        float mergeRight = -90;
        float mergeDown = 180;
        float mergeUp = 0;
        BootFram.transform.position = bootFingerStartPos;
        switch(PossibleMove.Direction)
        {
            case MoveDirection.Up:
                BootFram.transform.rotation = Quaternion.Euler(0, 0, mergeUp);
                break;
            case MoveDirection.Down:
                BootFram.transform.rotation = Quaternion.Euler(0, 0, mergeDown);
                break;
            case MoveDirection.Left:
                BootFram.transform.rotation = Quaternion.Euler(0, 0, mergeLeft);
                break;
            case MoveDirection.Right:
                BootFram.transform.rotation = Quaternion.Euler(0, 0, mergeRight);
                break;
        }
        return true;
    }
    public void UseHeroBoot()
    {

    }
}
