using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DNA_PlayerData;
using System.Linq;
public class DNAs : MonoBehaviour
{
    
    public DNA[] dnas;
    public List<DNA> dnasList = new List<DNA>();
    public DNA_slot[] upperDnasSlotes;
    public DNA_slot[] lowerDnasSlotes;
    public void CreatDNAs()
    {
        GameObject upperDnas = transform.Find("Upper").gameObject;
        GameObject lowerDnas = transform.Find("Lower").gameObject;
        upperDnasSlotes =  upperDnas.GetComponentsInChildren<DNA_slot>();
        lowerDnasSlotes = lowerDnas.GetComponentsInChildren<DNA_slot>();

        for (int i = 0; i < upperDnasSlotes.Length; i++)
        {
            DNA dna = dnas[Random.Range(0, dnas.Length)];
            upperDnasSlotes[i].type_positon = DnaData.Type_Position.up;
            upperDnasSlotes[i].ID = i;
            GameObject upperDna = Instantiate(dna.gameObject, upperDnasSlotes[i].transform.position, Quaternion.Euler(new Vector3(0f, 0f, 0f)));
            DNA dnaTemp;
            dnaTemp = upperDna.GetComponent<DNA>();
            dnaTemp.type_positon = DnaData.Type_Position.up;
            dnaTemp.ID = i;
            dnaTemp.Light_Select.gameObject.SetActive(false);
            upperDnasSlotes[i].DNA = dnaTemp;
            dnasList.Add(dnaTemp);
        }
        for (int i = 0; i < lowerDnasSlotes.Length; i++)
        {
            DNA dna = dnas[Random.Range(0, dnas.Length)];
            lowerDnasSlotes[i].type_positon = DnaData.Type_Position.down;
            lowerDnasSlotes[i].ID = i;
            GameObject lowerDna = Instantiate(dna.gameObject, lowerDnasSlotes[i].transform.position, Quaternion.Euler(new Vector3(0f, 0f, 0f)));
            DNA dnaTemp;
            dnaTemp = lowerDna.GetComponent<DNA>();
            dnaTemp.type_positon = DnaData.Type_Position.down;
            dnaTemp.ID = i;
            dnaTemp.Light_Select.gameObject.SetActive(false);
            lowerDnasSlotes[i].DNA = dnaTemp;
            dnasList.Add(dnaTemp);
        }
    }
    public void Resort()
    {
        dnasList = dnasList.OrderByDescending(dna => dna.ID).ToList();
        foreach (var dna in dnasList)
        {
            dna.MoveNext_DNA_Slot();
        }
        
    }
}
