using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NavPro : MonoBehaviour
{
    private NavMeshSurface navSurface;
    public NavMeshSurface NavSurface
    {
        get
        {
            if(navSurface == null)navSurface = GetComponent<NavMeshSurface>();
            return navSurface;
        }
        set
        {
            navSurface = value;
        }
    }
    public void BuildNavMesh()
    {
        NavSurface.BuildNavMesh();
    }
}
