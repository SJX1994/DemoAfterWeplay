using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell_Building : MonoBehaviour
{
    Vector3 areaRandomPos;
    public Vector3 AreaRandomPos
    {
         get
         {
              areaRandomPos = GetRandomPos();
              return areaRandomPos;
         }
    }
    Vector3 GetRandomPos()
    {
         Vector3 pos;
         Bounds bounds = GetComponent<MeshRenderer>().bounds;
         pos = new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
        return pos;
    }
}
