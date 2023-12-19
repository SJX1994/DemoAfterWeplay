using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using TMPro;
using UC_PlayerData;
public class Unit : MonoBehaviour
{
    public UnitData.Type type;
    public Vector2 posId;
    private Manager manager;
    public Manager Manager
    {
        get
        {
            if(manager == null)manager = FindObjectOfType<Manager>();
            return manager;
        }
        set
        {
            manager = value;
        }
    }
    private Vector3 mousePosition;
    public Vector3 MousePosition
    {
        get => mousePosition;
        set
        {
            if(!canDrag)return;
            mousePosition = value;
            StartCoroutine(MoveToPosition());
        }
    }
    private Vector3 offset;
    private LineRenderer mouseLineRenderer;
    public LineRenderer MouseLineRenderer
    {
        get
        {
            if(!mouseLineRenderer)mouseLineRenderer = GetComponent<LineRenderer>();
            return mouseLineRenderer;
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
    private float force;
    new Rigidbody rigidbody;
    Rigidbody Rigidbody
    {
        get
        {
            if(!rigidbody)rigidbody = gameObject.AddComponent<Rigidbody>();
            return rigidbody;
        }
    }
    public bool isMoving = false;
    public bool isDragging = false;
    public bool canDrag = false;
    public bool isPleaced = false;
    private Block blockPair;
    public Block BlockPair
    {
        get => blockPair;
        set
        {
            blockPair = value;
            blockPair.OccupiedUnit = this;
        }
    }
    void Update()
    {
        if(!isMoving)return;
        if (Rigidbody.IsSleeping())
        {
            isMoving = false;
            FindPosId();
            isPleaced = true;
        }
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<Rigidbody>() != null)
        {
            Camera.main.GetComponent<Shake>().ShakeObjectPosition();
            string Sound_Hit = "Sound_Hit_" + Random.Range(1, 7).ToString();
            Sound.Instance.PlaySoundTemp(Sound_Hit);
            isMoving = true;
        }
    }
    public bool Emission()
    {
        if(!canDrag)return false;
        Rigidbody.mass = 1f;
        if(force < 2.8f)return false;
        Rigidbody.AddForce(new Vector3(-1f,0.21f,0) * (500f + force * 30f));
        canDrag = false;
        isMoving = true;
        return true;
    }
    public void FindPosId()
    {
        posId = new Vector2(Mathf.RoundToInt(transform.position.x),Mathf.RoundToInt(transform.position.z));
        transform.position = new Vector3(posId.x, transform.position.y, posId.y);
        transform.rotation = Quaternion.Euler(Vector3.zero);
        Block block = Manager.Blocks.blocks.Where(x => x.posId == posId).FirstOrDefault();
        if(!block)
        {
            Destroy(gameObject);
            return;
        }
        transform.localScale = Vector3.one;
        BlockPair = block;
        if(isPleaced)
        {
            return;
        }
        Manager.Blocks.CheckMerge();
        TopMerge();
        
    }
    void TopMerge()
    {
        Unit oldUnit = FindObjectsOfType<Unit>().Where(x => x.posId == posId && x!=this).FirstOrDefault();
        if(oldUnit)
        {
            string oldLevel= oldUnit.TextMeshPro.text;
            Destroy(oldUnit.gameObject);
            string level = (int.Parse(oldLevel) + int.Parse(TextMeshPro.text)).ToString();
            Display_LevelUp(level);
            string Sound_TopMerge = "Sound_TopMerge";
            Sound.Instance.PlaySoundTemp(Sound_TopMerge);
            string Effect_TopMerge = "Effect_TopMerge";
            BlockPair.PlayEffect(Effect_TopMerge);
        }
    }
    IEnumerator MoveToPosition()
    {
        mousePosition = Camera.main.WorldToScreenPoint(transform.position);
        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3
        (Input.mousePosition.x, Input.mousePosition.y, mousePosition.z));
        while (Input.GetMouseButton(0))
        {
            Vector3 FinalPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
            Input.mousePosition.y, mousePosition.z)) + offset;
            transform.position = new Vector3(transform.position.x, transform.position.y, FinalPos.z);
            FinalPos = new Vector3(FinalPos.x, transform.position.y, FinalPos.z);
            DrawLine_Mouse(FinalPos);
            yield return new WaitForFixedUpdate();
        }
    }
    public void DrawLine_Mouse(Vector3 mousePos)
    {
        MouseLineRenderer.positionCount = 2;
        MouseLineRenderer.SetPosition(0, transform.position);
        MouseLineRenderer.SetPosition(1, mousePos);
        force = Vector3.Distance(transform.position, mousePos);
    }
    public void StopDrawLine_Mouse()
    {
        MouseLineRenderer.positionCount = 0;
    }
    public void Display_ShakeMe()
    {
        Shake shake;
        transform.TryGetComponent(out shake);
        if(shake){shake.ShakeObjectPosition();}
    }
    public void Display_StopShakeMe()
    {
        Shake shake;
        transform.TryGetComponent(out shake);
        if(shake){shake.StopShake();}
    }
    public void Display_MoveUp()
    {
        float duration = 0.5f;
        Tween moveY =  transform.DOMoveY(1f, duration);
        moveY.OnComplete(() => {
            canDrag = true;
        });
    }
    Tween scale;
    public void Display_Focus()
    {
        float duration = 0.3f;
        scale?.Kill();
        scale = transform.DOScale(1.2f, duration).SetEase(Ease.OutBounce);
    }
    public void Display_UnFocus()
    {
        float duration = 0.3f;
        scale?.Kill();
        scale = transform.DOScale(1f, duration).SetEase(Ease.InBounce);
    }
    public void Display_LevelUp(string LevelToShow)
    {
        float duration = 0.3f;
        Tween scale = transform.DOScale(1.2f, duration).SetEase(Ease.OutBounce);
        scale.OnComplete(() => {
            transform.DOScale(1f, duration).SetEase(Ease.InBounce);
        });
        TextMeshPro.text = LevelToShow; // (int.Parse(TextMeshPro.text) + 1).ToString();
    }
}
