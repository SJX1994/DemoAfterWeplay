using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UC_PlayerData;
using System.Linq;

public class Petri : MonoBehaviour
{
    public Unit[] units;
    public Unit currentUnit;
    public void InstantiateUnit()
    {
        string Sound_UnitBorn = "Sound_UnitBorn_" + Random.Range(1, 7).ToString();
        Sound.Instance.PlaySoundTemp(Sound_UnitBorn);
        Unit unit = units[Random.Range(0, units.Length)];
        Unit unitTemp = Instantiate(unit,transform.position, Quaternion.Euler(new Vector3(0f, 0f, 0f)));
        unitTemp.Display_MoveUp();
        currentUnit = unitTemp;
        Unit_UI unit_ui;
        unitTemp.transform.TryGetComponent(out unit_ui);
        if(!unit_ui)return;
        unit_ui.Showing = false;
    }

    public void InstantiateUnit(UnitData.Type type, string level)
    {
        if(currentUnit)Destroy(currentUnit.gameObject);
        string Sound_UnitBorn = "Sound_UnitBorn_" + Random.Range(1, 7).ToString();
        Sound.Instance.PlaySoundTemp(Sound_UnitBorn);
        Unit unit = units.Where(x=>x.type == type).FirstOrDefault();
        Unit unitTemp = Instantiate(unit,transform.position, Quaternion.Euler(new Vector3(0f, 0f, 0f)));
        unitTemp.Display_MoveUp();
        unitTemp.TextMeshPro.text = level;
        currentUnit = unitTemp;
        Unit_UI unit_ui;
        unitTemp.transform.TryGetComponent(out unit_ui);
        if(!unit_ui)return;
        unit_ui.Showing = false;
    }
}
