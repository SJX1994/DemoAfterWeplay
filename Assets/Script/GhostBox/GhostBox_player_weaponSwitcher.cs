using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBox_player_weaponSwitcher : MonoBehaviour
{
    public GameObject[] weapons_icon;
    public GameObject[] weapons;
    private int currentWeaponIndex;
    public int CurrentWeaponIndex
    {
        get
        {
            return currentWeaponIndex;
        }
        set
        {
            bool isChanged = currentWeaponIndex != value;
            currentWeaponIndex = value;
            GhostBox_player player;
            bool hasPlayer = TryGetComponent(out player);
            if(!player)return;
            player.Obj_holding = weapons[currentWeaponIndex];
            

        }
    }

    public void SwitchToPreviousWeapon()
    {
        string Sound_Select = "Sound_Select";
        Sound.Instance.PlaySoundTemp(Sound_Select);
        
        currentWeaponIndex--;
        if (currentWeaponIndex < 0)
        {
            currentWeaponIndex = weapons_icon.Length - 1;
        }
        EquipWeapon_Icon(currentWeaponIndex);
    }
    public void SwitchToNextWeapon()
    {
        string Sound_Select = "Sound_Select";
        Sound.Instance.PlaySoundTemp(Sound_Select);

        currentWeaponIndex++;
        if (currentWeaponIndex >= weapons_icon.Length)
        {
            currentWeaponIndex = 0;
        }
        EquipWeapon_Icon(currentWeaponIndex);
    }
    public void EquipWeapon_Icon(int weaponIndex)
    {
        
        for (int i = 0; i < weapons_icon.Length; i++)
        {
            weapons_icon[i].SetActive(false);
        }

        weapons_icon[weaponIndex].SetActive(true);

        CurrentWeaponIndex = weaponIndex;
    }
    void EquipWeapon()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].SetActive(false);
        }

        weapons[currentWeaponIndex].SetActive(true);
    }
}
