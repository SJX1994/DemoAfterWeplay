using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Cell_PlayerData;
using System.Linq;
using DG.Tweening;
using TMPro;
public class Cell : MonoBehaviour
{
    public int guardDistance = 10;
    public CellData.Cell_Type cell_Type;
    CellData.Cell_Type eattingType;
    private Cell_Manager cell_Manager;
    public Cell_Manager Cell_Manager
    {
        get
        {
            if(cell_Manager == null)cell_Manager = FindObjectOfType<Cell_Manager>();
            return cell_Manager;
        }
        set
        {
            cell_Manager = value;
        }
    }
    private TextMeshPro textMeshPro;
    public TextMeshPro TextMeshPro
    {
        get
        {
            if(textMeshPro == null)textMeshPro = GetComponentInChildren<TextMeshPro>();
            return textMeshPro;
        }
        set
        {
            textMeshPro = value;
        }
    }
    private NavMeshAgent navMesh;
    public NavMeshAgent NavMesh
    {
        get
        {
            if(navMesh == null)navMesh = GetComponent<NavMeshAgent>();
            return navMesh;
        }
        set
        {
            navMesh = value;
        }
    }
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
    bool active = false;
    List<Cell_Building> cellBuildings;
    List<Cell_Building> Cell_Buildings
    {
        get
        {
            if(cellBuildings == null)cellBuildings = Cell_Manager.Cell_Buildings.Cell_Buildings_Set;
            return cellBuildings;
        }
    }
    Vector3 target;
    Cell targetCell;
    public float lastGuardCheckTime, guardCheckInterval = 1f;
    public bool winner = false;
    void LateUpdate()
    {
        if(winner)return;
        FindAndEat();
        if(targetCell)
        {
            DrawLine();
        }else
        {
            if(cell_Type == CellData.Cell_Type.good)Line.positionCount = 0;
        }
        if(!active)return;
        if(NavMesh.remainingDistance > 0.5f)return;
        Active();
    }
    
    void OnDisable()
    {
        active = false;
        CancelInvoke();
    }
    void OnDestroy()
    {
        active = false;
        CancelInvoke();
    }
    public void Active()
    {
        active = true;
        target = Cell_Buildings[Random.Range(0,cellBuildings.Count)].AreaRandomPos;
        Patrol();
    }
    void Patrol()
    {
        active = false;
        Invoke(nameof(MoveTo),Random.Range(1.5f,3.5f));
    }
    void MoveTo()
    {
        active = true;
        NavMesh.SetDestination(target);
    }
    void FindAndEat()
    {
        if(cell_Type == CellData.Cell_Type.bad)return;
        WhatToEat();
        if (Time.time > lastGuardCheckTime + guardCheckInterval)
        {
            lastGuardCheckTime = Time.time;
            targetCell = GetNearestHostileCell();
            if(!targetCell)return;
            Line.positionCount = 2;
            Line.SetPosition(0,transform.position);
            Line.SetPosition(1,targetCell.transform.position);
            StartAttacking();
        }
    }
    void WhatToEat()
    {
        if(Cell_Manager.Cells.cell_Bad_Set.Count > 0)
        { 
            eattingType = CellData.Cell_Type.bad;
        }else if(Cell_Manager.Cells.cell_Good_Set.Count > 0)
        {
            eattingType = CellData.Cell_Type.good;
        }
    }
    protected virtual void StartAttacking()
    {
        if (IsDeadOrNull(targetCell))return;
        StartCoroutine(DealAttack());
    }
    Tween tween_textMesh;
    public virtual IEnumerator DealAttack()
    {
        TextMeshPro.text = "";
        tween_textMesh?.Kill();
        float attackSpeed = 1f / 0.5f;
        while (targetCell != null)
        {
            float eating = 0.003f;
            transform.localScale += new Vector3(eating, eating, eating);
            TextMeshPro.text = "+1";
            tween_textMesh = TextMeshPro.transform.DOScale(Vector3.one * 1.5f, 0.5f).SetEase(Ease.OutBounce).OnComplete(() => {
                TextMeshPro.transform.localScale = Vector3.one;
            });
            targetCell.SufferAttack(eating*10f);

            yield return new WaitForSeconds(attackSpeed);

            if (IsDeadOrNull(targetCell))break;
        }
    }
    public void SufferAttack(float damage)
    {
        TextMeshPro.text = "";
        tween_textMesh?.Kill();
        transform.localScale -= new Vector3(damage, damage, damage);
        TextMeshPro.text = "-1";
        tween_textMesh = TextMeshPro.transform.DOScale(Vector3.one * 1.5f, 0.5f).SetEase(Ease.OutBounce).OnComplete(() => {
                TextMeshPro.transform.localScale = Vector3.one;
        });
        if (transform.localScale.x < 0.1f)
        {
            if(cell_Type == CellData.Cell_Type.bad)
            {
                Cell_Manager.Cells.cell_Bad_Set.Remove(this);
            }else if(cell_Type == CellData.Cell_Type.good)
            {
                Cell_Manager.Cells.cell_Good_Set.Remove(this);
            }
            
            string Sound_Bubble_Die = "Sound_Bubble_Die";
            Sound.Instance.PlaySoundTemp(Sound_Bubble_Die);
            Destroy(gameObject);
        }
    }
    Cell GetNearestHostileCell()
    {
        Cell nearestEnemy = null;
        List<Cell> hostiles = new(); 
        if(eattingType == CellData.Cell_Type.bad)
        {
            hostiles = Cell_Manager.Cells.cell_Bad_Set;
        }else if (eattingType == CellData.Cell_Type.good)
        {
            hostiles = Cell_Manager.Cells.cell_Good_Set;
        }

        float nearestEnemyDistance = 1000f;
        
        for (int i = 0; i < hostiles.Count(); i++)
        {
            if(hostiles[i] == this)continue;
            if (IsDeadOrNull(hostiles[i]))continue;
            float distanceFromHostile = Vector3.Distance(hostiles[i].transform.position, transform.position);
            if (distanceFromHostile <= guardDistance)
            {
                if (distanceFromHostile < nearestEnemyDistance)
                {
                    nearestEnemy = hostiles[i];
                    nearestEnemyDistance = distanceFromHostile;
                }
            }
        }
        return nearestEnemy;
    }
    bool IsDeadOrNull(Cell u)
    {
        return (u == null);
    }
    void DrawLine()
    {
        if(cell_Type != CellData.Cell_Type.good)return;
        if(!targetCell)return;
        Line.SetPosition(0,transform.position);
        Line.SetPosition(1,targetCell.transform.position);
    }
    public void Win()
    {
        winner = true;
        NavMesh.isStopped = true;
        NavMesh.ResetPath();
            transform.DOScale(Vector3.one * 2f ,0.5f).SetEase(Ease.OutBounce).OnComplete(() => {
            TextMeshPro.text = " Ultimate Cell ";
        });
        Camera.main.GetComponent<Shake>().ShakeObjectPosition();
        string Sound_ThereMerge = "Sound_ThereMerge";
        Sound.Instance.PlaySoundTemp(Sound_ThereMerge,0.5f,0.2f);
    }
}
