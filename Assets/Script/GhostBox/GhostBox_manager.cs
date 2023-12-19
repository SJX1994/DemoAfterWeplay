using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using GhostBox_PlayerData;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;
public class GhostBox_manager : MonoBehaviour
{
    private GhostBox_player player;
    public GhostBox_player Player
    {
        get
        {
            if(player == null)player = FindObjectOfType<GhostBox_player>();
            return player;
        }
        set
        {
            player = value;
        }
    }
    private Camera_Follower camera_Follower;
    public Camera_Follower Camera_Follower
    {
        get
        {
            if(camera_Follower == null)camera_Follower = FindObjectOfType<Camera_Follower>();
            return camera_Follower;
        }
        set
        {
            camera_Follower = value;
        }
    }
    private GhostBox_map map;
    public GhostBox_map Map
    {
        get
        {
            if(map == null)map = FindObjectOfType<GhostBox_map>();
            return map;
        }
        set
        {
            map = value;
        }
    }
    public GhostBox_player playerObj;
    public GhostBox_NPC npc_bad;
    public GhostBox_NPC npc_good;
    public TextMeshProUGUI roundText;
    public TextMeshProUGUI RoundText
    {
        get
        {
            if(roundText == null)roundText = GameObject.Find("RoundText").GetComponent<TextMeshProUGUI>();
            return roundText;
        }
        set
        {
            roundText = value;
        }
    }
    Tween roundTextTween;

    void Start()
    {
        Map.CreateBlocks();
        Vector3 creatPos = Map.blocks.Where(x=>x.posId.x == Map.gridWidth/2 && x.posId.y == Map.gridHeight/2).FirstOrDefault().transform.position;
        creatPos.y += 5f;
        CreatPlayer(creatPos);
        CreatNPC(5,NPC_Data.NPC_Type.bad);
        CreatNPC(5,NPC_Data.NPC_Type.good);
        if(playerObj)Camera_Follower.target = playerObj.transform;
        Time.timeScale = 1.0f;
        Referee.Round = 0;
        Referee.OnRoundChange += (int round)=>
        {
            OnSpacePress(round);
            
        };
        DoMoveNPC();
    }
    void OnSpacePress(int round)
    {
        roundTextTween?.Kill();
        roundTextTween = RoundText.transform.DOScale(1.5f, 0.5f).SetEase(Ease.InBack).OnComplete(()=>
        {
            roundTextTween = RoundText.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
        });
        RoundText.text = round.ToString();
        List<GhostBox_NPC> npcList = FindObjectsOfType<GhostBox_NPC>().ToList();
        npcList.ForEach(x=>x.Action());
        DoMoveNPC();
        WinCheck();
    }
    void DoMoveNPC()
    {
        List<GhostBox_NPC> npcList = FindObjectsOfType<GhostBox_NPC>().ToList();
        npcList.ForEach(x=>x.IsMoving = true);
    }
    void WinCheck()
    {
        int badNumb = FindObjectsOfType<GhostBox_NPC>().ToList().Where(x=>x.npcType == NPC_Data.NPC_Type.bad).Count();
        if(badNumb>0)return;
        int boxNumb = FindObjectsOfType<GhostBox_box>().ToList().Count();
        if(boxNumb>0)return;
        FindObjectsOfType<GhostBox_NPC>().ToList().Where(x=>x.npcType == NPC_Data.NPC_Type.good).ToList().ForEach(x=>x.Win());
        Camera.main.GetComponent<Shake>().ShakeObjectPosition();
        string Sound_ThereMerge = "Sound_ThereMerge";
        Sound.Instance.PlaySoundTemp(Sound_ThereMerge,0.5f,0.2f);

    }
    void CreatNPC(int howMany, NPC_Data.NPC_Type type)
    {
        for(int i = 0; i < howMany; i++)
        {
            Vector3 creatPos  = new Vector3(Random.Range(0, Map.gridWidth), 0f, Random.Range(0, Map.gridHeight));
            var pairedBlock = Map.blocks.Where(x=>x.posId == new Vector2(creatPos.x, creatPos.z)).FirstOrDefault();
            if(!pairedBlock || pairedBlock.IsOccupied){i-=1; continue;}
            creatPos.y += 15f;
            GhostBox_NPC npc_temp = null;
            switch(type)
            {
                case NPC_Data.NPC_Type.bad:
                    npc_temp = npc_bad;
                    break;
                case NPC_Data.NPC_Type.good:
                    npc_temp = npc_good;
                    break;
            }
            GhostBox_NPC npc_bad_temp = Instantiate(npc_temp, creatPos, Quaternion.Euler(new Vector3(0f, Random.Range(0f,360f), 0f)));
            pairedBlock.OccupiedNpc = npc_bad_temp;
            npc_bad_temp.blockPaired = pairedBlock;
            npc_bad_temp.gameObject.name = "NPC_bad_" + i.ToString();
            npc_bad_temp.npcType = type;
        }
    }
  
    void CreatPlayer(Vector3 creatPos)
    {
        playerObj = Instantiate(playerObj, creatPos, Quaternion.Euler(new Vector3(0f, 0f, 0f)));
        playerObj.CreatBox();
    }
    public void Restart()
    {
        DOTween.Clear(true);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
