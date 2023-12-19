using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleLoader : MonoBehaviour
{
    static ParticleLoader particleSys;
    static ParticleSystem particleSource;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    public static ParticleLoader Instance
    {
        get
        {
            if (particleSys == null)
            {
                particleSys = new GameObject("ParticleSystem").AddComponent<ParticleLoader>();
            }
            return particleSys;
        }
    }
    public GameObject PlayParticleTemp(string name,Vector3 position,Vector3 rotation)
    {
        ParticleSystem particleSystem = Resources.Load<ParticleSystem>(name);
        GameObject prefabInstance = Instantiate(particleSystem.gameObject,position, Quaternion.Euler(rotation), particleSys.transform);
        return prefabInstance;
    }
}
