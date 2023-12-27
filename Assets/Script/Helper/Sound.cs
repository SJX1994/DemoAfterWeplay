using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    static Sound soundSys;
    static AudioSource musicSource;
    static AudioSource soundSource;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    public static Sound Instance
    {
        get
        {
            if (soundSys == null)
            {
                soundSys = new GameObject("AudioSystem").AddComponent<Sound>();
                soundSys.gameObject.AddComponent<AudioListener>();
                soundSys.Init();
            }
            return soundSys;
        }
    }
    void Init()
    {
        soundSource = new GameObject("soundSource").AddComponent<AudioSource>();
        soundSource.transform.SetParent(soundSys.transform);
        soundSource.playOnAwake = false;
        soundSource.loop = false;
        musicSource = new GameObject("musicSource").AddComponent<AudioSource>();
        musicSource.transform.SetParent(soundSys.transform);
        musicSource.playOnAwake = false;
        musicSource.loop = false;
    }
    public void PlayMusicSimple(string path,float volume = 1)
    {
        AudioClip clip = Resources.Load<AudioClip>(path);
        if (clip == null)return;
        // if(musicSource.isPlaying)return;
        musicSource.clip = clip;
        musicSource.volume *= volume;
        musicSource.Play();
        musicSource.loop = true;

    }
    public void PlaySoundTemp(string name,float volume = 1,float delay = 0)
    {
        AudioClip clip = Resources.Load<AudioClip>(name);
        AudioSource soundSourceTemp = new GameObject("soundSourceTemp").AddComponent<AudioSource>();
        float destoryTime = clip.length;
        soundSourceTemp.transform.SetParent(soundSys.transform);
        soundSourceTemp.playOnAwake = false;
        soundSourceTemp.loop = false;
        soundSourceTemp.clip = clip;
        soundSourceTemp.volume *= volume;
        if(delay == 0)
        {
            if(clip.length > 3)
            {
                destoryTime = 3;
                float randomStartTime = UnityEngine.Random.Range(0, clip.length-3);
                soundSourceTemp.time = randomStartTime;
                soundSourceTemp.Play();
            }else
            {
                soundSourceTemp.PlayOneShot(clip);
            }
            
        }else
        {
            if(clip.length > 3)
            {
                destoryTime = 3;
                float randomStartTime = UnityEngine.Random.Range(0, clip.length-3);
                soundSourceTemp.time = randomStartTime;
                soundSourceTemp.PlayDelayed(delay);
            }else
            {
                soundSourceTemp.PlayDelayed(delay);
            }
        }
        Destroy(soundSourceTemp.gameObject,destoryTime + delay + 1f);
    }
}
