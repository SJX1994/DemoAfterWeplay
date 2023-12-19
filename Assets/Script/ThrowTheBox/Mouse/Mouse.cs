using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{
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
    public LayerMask unitTargetMask;
    public Unit focusUnit;
    Unit_UI focusUnit_ui;
    
    public Unit FocusUnit
    {
        get => focusUnit;
        set 
        {
            if(focusUnit)focusUnit.Display_UnFocus();
            focusUnit = value;
            if(focusUnit)focusUnit.Display_Focus();
        }
    }

    void Update()
    {
        MouseButtonDown();
        MouseButtonUp();
        MouseButtonDown_UI();
        MouseButtonUp_UI();
    }
    public void MouseButtonDown_UI()
    {
        bool mouseButtonDown = Input.GetMouseButtonDown(0);
        if (!mouseButtonDown)return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hitUnit = Physics.Raycast(ray, out hit, Mathf.Infinity, unitTargetMask);
        if(!hitUnit)return;
        Unit_UI unit_ui;
        hit.collider.transform.TryGetComponent(out unit_ui);
        Unit unit;
        hit.collider.transform.TryGetComponent(out unit);
        if(!unit_ui)return;
        if(unit.canDrag)return;
        focusUnit_ui  = unit_ui;
        unit_ui.Showing = true;
    }
    public void MouseButtonUp_UI()
    {
        bool mouseButtonUp = Input.GetMouseButtonUp(0);
        if(!mouseButtonUp)return;
        if(!focusUnit_ui)return;
        focusUnit_ui.Showing = false;
    }
    public void MouseButtonDown()
    {
        bool mouseButtonDown = Input.GetMouseButtonDown(0);
        if (!mouseButtonDown)return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hitUnit = Physics.Raycast(ray, out hit, Mathf.Infinity, unitTargetMask);
        if(!hitUnit)return;
        Unit unit;
        hit.collider.transform.TryGetComponent(out unit);
        if(!unit)return;
        if(!unit.canDrag)return;
        FocusUnit = unit;
        FocusUnit.isDragging = true;
        unit.MousePosition = Input.mousePosition;
        string Sound_DrawTheBow = "Sound_DrawTheBow";
        Sound.Instance.PlaySoundTemp(Sound_DrawTheBow);
    }
    public void MouseButtonUp()
    {
        bool mouseButtonUp = Input.GetMouseButtonUp(0);
        if(!mouseButtonUp)return;
        if(!FocusUnit)return;
        if(!FocusUnit.canDrag)return;
        FocusUnit.isDragging = false;
        FocusUnit.StopDrawLine_Mouse();
        bool enoughForce = FocusUnit.Emission();
        if(!enoughForce)return;
        string Sound_FireArrow = "Sound_FireArrow";
        Sound.Instance.PlaySoundTemp(Sound_FireArrow);
        Invoke(nameof(InstantiateUnit), 0.5f);
    }
    void InstantiateUnit()
    {
        Manager.Petri.InstantiateUnit();
    }
}
