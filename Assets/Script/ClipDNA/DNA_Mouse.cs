using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DNA_PlayerData;
public class DNA_Mouse : MonoBehaviour
{
    private LineRenderer line;
    public LineRenderer Line
    {
        get
        {
            if(line == null)line = GetComponent<LineRenderer>();
            return line;
        }
        set
        {
            line = value;
        }
    }
    private DNA_Manager manager;
    public DNA_Manager Manager
    {
        get
        {
            if(manager == null)manager = FindObjectOfType<DNA_Manager>();
            return manager;
        }
        set
        {
            manager = value;
        }
    }
    public LayerMask dnaTargetMask;
    DNA targetDna;
    public List<DNA> selectedDnas = new List<DNA>();
    DNA forcusDna;
    DNA ForcusDna
    {
        get
        {
            return forcusDna;
        }
        set
        {
            if(forcusDna == value)return;
            forcusDna = value;
            forcusDna.FindNextCanLink();
        }
    }
    void Update()
    {
        MouseButtonDown();
        MouseButtonUp();
        if(!DnaData.IsCollecting)return;
        if(selectedDnas.Count == 0)
        {
            DnaData.IsCollecting = false;
        }
    }
    void MouseButtonDown()
    {
        if(DnaData.IsCollecting)return;
        bool mouseButtonDown = Input.GetMouseButtonDown(0);
        if (!mouseButtonDown)return;
        StartCoroutine(KeepMouseSelect());
    }
    void MouseButtonUp()
    {
        bool mouseButtonUp = Input.GetMouseButtonUp(0);
        if (!mouseButtonUp)return;
        for(int i = 0; i < selectedDnas.Count; i++)
        {
            selectedDnas[i].OnBeenCollecting();
        }
        foreach (var dna in FindObjectsOfType<DNA>())
        {
            dna.Selected = false;
            dna.HideSelectable();
            dna.selectable = true;
        }
        targetDna = null;
        forcusDna = null;
        Line.positionCount = 0;
        // selectedDnas.Clear();
    }
    Vector3 mousePosition;
    Vector3 offset;
    IEnumerator KeepMouseSelect()
    {
        mousePosition = Camera.main.WorldToScreenPoint(transform.position);
        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3
        ( mousePosition.x,  mousePosition.y, mousePosition.z));

        while (Input.GetMouseButton(0))
        {
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool hitUnit = Physics.Raycast(ray, out hit, Mathf.Infinity, dnaTargetMask);
            if(hitUnit)
            {
                DNA dna;
                hit.collider.transform.TryGetComponent(out dna);
                if(dna.selectable)
                {
                    string Sound_Select = "Sound_Select";
                    Sound.Instance.PlaySoundTemp(Sound_Select);
                    string Sound_DrawTheBow = "Sound_DrawTheBow";
                    Sound.Instance.PlaySoundTemp(Sound_DrawTheBow,1,0.2f);
                    Line.positionCount += 2;
                    Line.SetPosition(Line.positionCount - 1, dna.transform.position);
                    Line.SetPosition(Line.positionCount - 2, dna.transform.position);
                    if(dna)dna.Selected = true;
                    if(!targetDna){targetDna = dna;}
                    ForcusDna = dna;
                    selectedDnas.Add(dna);
                }
            }else if(Line.positionCount > 1)
            {
                Vector3 FinalPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                Input.mousePosition.y, mousePosition.z)) + offset;
                transform.position = new Vector3(transform.position.x, transform.position.y, FinalPos.z);
                FinalPos = new Vector3(FinalPos.x, transform.position.y, FinalPos.z);
                Line.SetPosition(Line.positionCount - 1, FinalPos);
            }
          
            
            yield return new WaitForFixedUpdate();
        }
    }
}
