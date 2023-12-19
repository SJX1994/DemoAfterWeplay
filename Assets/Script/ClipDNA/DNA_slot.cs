using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DNA_PlayerData;
public class DNA_slot : MonoBehaviour
{
    public DnaData.Type_Position type_positon = new();
    public int ID;
    public DNA dna;
    public DNA DNA
    {
        get
        {
            return dna;
        }
        set
        {
            dna = value;
        }
    }
    
}
