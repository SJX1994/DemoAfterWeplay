using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Match3G_PlayerData;
public class Match3G_Unit_OutLine : MonoBehaviour
{
    public void Show(Vector3 pos)
    {
        gameObject.SetActive(true);
        pos.y += 0.02f;
        pos.z += 0.1f;
        transform.position = pos;
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
