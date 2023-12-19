using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DNA_PlayerData;
using DG.Tweening;
using TMPro;
public class DNA : MonoBehaviour
{
    public DnaData.DNA_Type dna_type;
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
    private Light light_Select;
    public Light Light_Select
    {
        get
        {
            if(!light_Select)light_Select = GetComponentInChildren<Light>();
            return light_Select;
        }
    }
    public DnaData.Type_Position type_positon = new();
    public int ID;
    private Tween tween_scale;
    private bool selected = false;
    public bool Selected
    {
        get => selected;
        set
        {
            if(selected == value)return;
            selected = value;
            if (selected)
            {
                OnSelected();
            }
            else
            {
                OnDeselected();
            }
        }
    }
    public bool selectable = true;
    bool collecting = false;
    bool CheckNext_DNA_Slot()
    {
        
        DNA dna = Manager.Dnas.dnasList.Find(x => x.ID == ID + 1 && x.type_positon == type_positon);
        if(!dna) 
        {
            return false;
        }else
        {
            return true;
        }
    }
    Tween tweenMoveNext;
    public void MoveNext_DNA_Slot()
    {
        if(collecting)return;
        if(CheckNext_DNA_Slot())return;
        tweenMoveNext?.Kill();
        if(type_positon == DnaData.Type_Position.up)
        {
            int IDcheck = ID + 1;
            if(IDcheck > Manager.Dnas.upperDnasSlotes.Length - 1)return;
            DNA_slot dna_Slot = Manager.Dnas.upperDnasSlotes[IDcheck];
            if(!dna_Slot)return;
            tweenMoveNext = transform.DOMove(dna_Slot.transform.position, 0.5f).SetEase(Ease.OutSine);
            ID = dna_Slot.ID;
            dna_Slot.DNA = this;
        }
        if(type_positon == DnaData.Type_Position.down)
        {
            int IDcheck = ID + 1;
            if(IDcheck > Manager.Dnas.lowerDnasSlotes.Length - 1)return;
            DNA_slot dna_Slot = Manager.Dnas.lowerDnasSlotes[IDcheck];
            if(!dna_Slot)return;
            tweenMoveNext = transform.DOMove(dna_Slot.transform.position, 0.5f).SetEase(Ease.OutSine);
            ID = dna_Slot.ID;
            dna_Slot.DNA = this;
        }
        tweenMoveNext.onComplete += () =>
        {
            MoveNext_DNA_Slot();
        };
        
    }
    public void OnBeenCollecting()
    {
        DnaData.IsCollecting = true;
        collecting = true;
        tween_scale?.Kill();
        tweenMoveNext?.Kill();
        DNA_Monstor dna_Monstor = Manager.Monstor;
        Color color = transform.GetComponent<Renderer>().material.color;
        float duration = Vector3.Distance(transform.position, dna_Monstor.transform.position) / 10f + Random.Range(0.1f, 0.8f);
        transform.DOMove(dna_Monstor.transform.position, duration).SetEase(Ease.OutSine).OnComplete(() =>
        {
            dna_Monstor.CollectDisplay(color);
            Manager.Dnas.dnasList.Remove(this);
            Manager.Mouse.selectedDnas.Remove(this);
            Manager.Dnas.Resort();
            Destroy(gameObject);
        });
    }
    public void OnSelected()
    {
        tween_scale?.Kill();
        if(!selectable)return;
        tween_scale = transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.5f).SetEase(Ease.OutBounce);
    }
    public void OnDeselected()
    {
        tween_scale?.Kill();
        tween_scale = transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f).SetEase(Ease.InBounce);
    }
    public void ShowSelectable()
    {
        selectable = true;
        Light_Select.gameObject.SetActive(true);
    }
    public void HideSelectable()
    {
        selectable = false;
        Light_Select.gameObject.SetActive(false);
    }
    public void FindNextCanLink()
    {
        if(!selectable)return;
        foreach (var item in Manager.Dnas.dnasList)
        {
            if(!item)continue;
            item.HideSelectable();
            if(item == this)continue;
            if(item.type_positon == type_positon && (item.ID == ID + 1 || item.ID == ID - 1) && !item.Selected)
            {
                item.ShowSelectable();
            }
            if(item.type_positon != type_positon && (item.ID == ID || item.ID == ID + 1 || item.ID == ID - 1) && !item.Selected)
            {
                item.ShowSelectable();
            }
            if(item.dna_type != dna_type)
            {
                item.HideSelectable();
            }
        }

    }
}
