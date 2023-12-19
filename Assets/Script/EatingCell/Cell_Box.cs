using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
public class Cell_Box : MonoBehaviour
{
    private Cell_Manager manager;
    public Cell_Manager Manager
    {
        get
        {
            if(!manager)manager = FindObjectOfType<Cell_Manager>();
            return manager;
        }
    }
    private TextMeshPro textMeshPro;
    public TextMeshPro TextMeshPro
    {
        get
        {
            if(!textMeshPro)textMeshPro = GetComponentInChildren<TextMeshPro>();
            return textMeshPro;
        }
    }
    new Rigidbody rigidbody;
    Rigidbody Rigidbody
    {
        get
        {
            if(!rigidbody)
            {
                rigidbody = gameObject.AddComponent<Rigidbody>();
            }
            return rigidbody;
        }
    }
    bool collisioned = false;
    bool opend = false;
    int howMany = 0;
    private void OnCollisionEnter(Collision collision)
    {
        if(collisioned)return;
        Camera.main.GetComponent<Shake>().ShakeObjectPosition();
        string Sound_Hit = "Sound_Hit_" + Random.Range(1, 7).ToString();
        Sound.Instance.PlaySoundTemp(Sound_Hit);
        collisioned = true;
    }
    void Update()
    {
        if(Rigidbody.IsSleeping() && !opend)
        {
            OpenBox();
        }
        
    }
    public void Emission()
    {
        howMany = Random.Range(1, 6);
        TextMeshPro.text = howMany.ToString() ;
        float randomRange = Random.Range(0f, 360f);
        transform.rotation = Quaternion.Euler(randomRange, randomRange, randomRange);
        Rigidbody.mass = 1f;
        Rigidbody.AddForce(Vector3.up * 300f);
    }
    void OpenBox()
    {
        opend = true;
        transform.DOScale(Vector3.one * 2 ,0.25f).SetEase(Ease.OutBounce).OnComplete(() => {
            transform.DOScale(Vector3.zero ,0.25f).OnComplete(() => {
                Manager.Cells.CereatGoodCell(transform.position,howMany);
                string Sound_UnitBorn = "Sound_UnitBorn_" + Random.Range(1, 7).ToString();
                Sound.Instance.PlaySoundTemp(Sound_UnitBorn);
                Destroy(gameObject);
            });
        });
    }
  
}


