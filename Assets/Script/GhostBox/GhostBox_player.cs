using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using GhostBox_PlayerData;
public class GhostBox_player : MonoBehaviour
{
    private HodingObjectData.HodingObjectType hodingObjectType;
    private List<GameObject> obj_holdingList = new List<GameObject>();
    private GameObject obj_holding;
    public GameObject Obj_holding
    {
        get
        {
            return obj_holding;
        }
        set
        {
            if(obj_holding) Destroy(obj_holding);
            obj_holding = value;
            obj_holding = Instantiate(obj_holding.gameObject,transform.position,Quaternion.identity,transform);
            obj_holding.GetComponent<BoxCollider>().enabled = false;
            if(obj_holding.TryGetComponent(out GhostBox_box box))
            {
                hodingObjectType = HodingObjectData.HodingObjectType.box;
                box.objectType = hodingObjectType;
            }
            else if(obj_holding.TryGetComponent(out GhostBox_bomb bomb))
            {
                hodingObjectType = HodingObjectData.HodingObjectType.bomb;
                bomb.objectType = hodingObjectType;
            }
            else if(obj_holding.TryGetComponent(out GhostBox_timer timer))
            {
                hodingObjectType = HodingObjectData.HodingObjectType.timer;
                timer.objectType = hodingObjectType;
            }
            else
            {
                hodingObjectType = HodingObjectData.HodingObjectType.box;
            }
        }
    }
    [HideInInspector]
    public bool pleacedBox = false;
    List<GameObject> pleacedObjectList = new List<GameObject>();
    private LineRenderer lineRenderer;
    public LineRenderer LineRenderer
    {
        get
        {
            if(lineRenderer == null)lineRenderer = GetComponent<LineRenderer>();
            return lineRenderer;
        }
        set
        {
            lineRenderer = value;
        }
    }
    public float speed = 6f;                            
    public float turnSmoothTime = 0.5f;
    public Transform cameraTransform;
    public Transform CameraTransform
    {
        get
        {
            if(cameraTransform == null)cameraTransform = Camera.main.transform;
            return cameraTransform;
        }
        set
        {
            cameraTransform = value;
        }
    }
    private Rigidbody playerRigidbody;
    public Rigidbody PlayerRigidbody
    {
        get
        {
            if(playerRigidbody == null)playerRigidbody = GetComponent<Rigidbody>();
            return playerRigidbody;
        }
        set
        {
            playerRigidbody = value;
        }
    }
    private Vector3 point2Pos;
    public Vector3 Point2Pos
    {
        get
        {
            return point2Pos;
        }
        set
        {
            if(point2Pos == value)return;
            point2Pos = value;
            LineRenderer.positionCount = 2;
            LineRenderer.SetPosition(0, transform.position);
            LineRenderer.SetPosition(1, point2Pos);
        }
    }
    private GhostBox_player_weaponSwitcher weaponSwitcher;
    public GhostBox_player_weaponSwitcher WeaponSwitcher
    {
        get
        {
            if(weaponSwitcher == null)weaponSwitcher = GetComponent<GhostBox_player_weaponSwitcher>();
            return weaponSwitcher;
        }
        set
        {
            weaponSwitcher = value;
        }
    }
    private float turnSmoothVelocity;
    void Update()
    {
        Emission();
        SwitchWeapon();
    }
    void FixedUpdate()
    {
        PlayerControl();
    }
    void PlayerControl()
    {
        
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + CameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            Vector3 moveDirection = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;

            PlayerRigidbody.MovePosition(PlayerRigidbody.position + moveDirection.normalized * speed * Time.unscaledDeltaTime);
            
            transform.rotation = Quaternion.LookRotation(moveDirection * Time.unscaledDeltaTime);
        }
    }
    void SwitchWeapon()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            WeaponSwitcher.SwitchToPreviousWeapon();
        }
        else if(Input.GetKeyDown(KeyCode.E))
        {
            WeaponSwitcher.SwitchToNextWeapon();
        }
    }
    public void PlaceBox(float force,Vector3 direction,Vector3 pointA,Vector3 pointB)
    {
        obj_holding.transform.parent = null;
        pleacedBox = true;
        obj_holding.GetComponent<BoxCollider>().enabled = true;
        switch(hodingObjectType)
        {
            case HodingObjectData.HodingObjectType.box:
                GhostBox_box box = obj_holding.GetComponent<GhostBox_box>();
                box.force = force;
                box.direction = direction;
                box.SetLine(new Vector3[]{pointA,pointB});
                break;
            case HodingObjectData.HodingObjectType.bomb:
                GhostBox_bomb bomb = obj_holding.GetComponent<GhostBox_bomb>();
                bomb.force = force;
                bomb.direction = direction;
                bomb.SetLine(new Vector3[]{pointA,pointB});
                break;
            case HodingObjectData.HodingObjectType.timer:
                GhostBox_timer timer = obj_holding.GetComponent<GhostBox_timer>();
                break;
        }
        pleacedObjectList.Add(obj_holding);
        obj_holding = null;
    }
    public void CreatBox()
    {
        // hodingObjectType = HodingObjectData.HodingObjectType.box;
        WeaponSwitcher.EquipWeapon_Icon(((int)hodingObjectType));
    }
    public void Emission()
    {
        bool space = Input.GetKeyDown(KeyCode.Space);
        if(!space)return;
        Referee.Round += 1;
        foreach (var item in pleacedObjectList)
        {
            if(item.TryGetComponent(out GhostBox_box box))
            {
                box.Emission();
            }
            else if(item.TryGetComponent(out GhostBox_bomb bomb))
            {
                bomb.Emission();
            }
            else if(item.TryGetComponent(out GhostBox_timer timer))
            {
                timer.Emission();
            }
        }
        pleacedObjectList.Clear();
    }
    
}
