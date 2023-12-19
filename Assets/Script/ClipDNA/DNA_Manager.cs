using UnityEngine;
using DNA_PlayerData;
public class DNA_Manager : MonoBehaviour
{
    private DNA_Monstor monstor;
    public DNA_Monstor Monstor
    {
        get
        {
            if(monstor == null)monstor = FindObjectOfType<DNA_Monstor>();
            return monstor;
        }
        set
        {
            monstor = value;
        }
    }
    private DNA_Mouse mouse;
    public DNA_Mouse Mouse
    {
        get
        {
            if(mouse == null)mouse = FindObjectOfType<DNA_Mouse>();
            return mouse;
        }
        set
        {
            mouse = value;
        }
    }
    private DNAs dnas;
    public DNAs Dnas
    {
        get
        {
            if(dnas == null)dnas = FindObjectOfType<DNAs>();
            return dnas;
        }
        set
        {
            dnas = value;
        }
    }
    void Start()
    {
        Dnas.CreatDNAs();
        DnaData.IsCollecting = false;
    }
}