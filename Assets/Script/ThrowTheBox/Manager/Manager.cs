using UnityEngine;
using UC_PlayerData;
public class Manager : MonoBehaviour
{
    private Mouse mouse;
    public Mouse Mouse
    {
        get
        {
            if(mouse == null)mouse = FindObjectOfType<Mouse>();
            return mouse;
        }
        set
        {
            mouse = value;
        }
    }
    private Blocks blocks;
    public Blocks Blocks
    {
        get
        {
            if(blocks == null)blocks = FindObjectOfType<Blocks>();
            return blocks;
        }
        set
        {
            blocks = value;
        }
    }
    private Petri petri;
    public Petri Petri
    {
        get
        {
            if(petri == null)petri = FindObjectOfType<Petri>();
            return petri;
        }
        set
        {
            petri = value;
        }
    }
    public void Start()
    {
        Blocks.CreateBlocks();
        Petri.InstantiateUnit();
    }

}