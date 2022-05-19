using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public sealed class AudioManager : MonoBehaviour
{
    static AudioManager instance;

    public static AudioManager Instance
    {
        get { return instance; }
    }
    public static AudioManager GetInstance()
    {
        return instance;
    }
    [System.Serializable]
    public class Sound
    {
        public string name;

        public List<AudioClip> clip;
    }
    [System.Serializable]
    public class Music
    {
        public string name;

        public AudioClip intro;
        public AudioClip theme;
    }
    public Sound[] sounds;
    public Music[] music;

    static AudioSource music_source;

    static public float music_volume = 0.5f;
    static public float SFX_volume = 0.5f;
    static public float global_volume = 1; //from 0 to 1
    static AudioSource[] SFX_source = new AudioSource[30];

    [SerializeField]bool debug;

    static int nextSFX_source = 0;

    static bool interrupt = false;
    private void Awake() 
    {
        instance = this;
        music_source = gameObject.AddComponent<AudioSource>();
        
        for (int i = 0; i < 30; i++)
        {
            SFX_source[i] = gameObject.AddComponent<AudioSource>();
            SFX_source[i].volume = SFX_volume * global_volume;
        }
        music_source.volume = music_volume * global_volume;
    }

    private void Start()
    {
        StartCoroutine(PlayMusic(music[0], true));
    }


    public static void PlaySFX(string name)
    {
        Sound s = Array.Find(GetInstance().sounds, sound => sound.name == name);
        if (s == null)
        {
            if(instance.debug){Debug.LogWarning("sound: " + name + "not found");}
            return;
        }
        PlaySFX(s);
    }
    public static void PlaySFX(Sound sound)
    {
        SFX_source[nextSFX_source].clip = sound.clip [UnityEngine.Random.Range(0, sound.clip.Count)];
        SFX_source[nextSFX_source].volume = SFX_volume * global_volume;
        SFX_source[nextSFX_source].Play();
        if(instance.debug){Debug.Log("Playing SFX: " + sound.name);}
        nextSFX_source++;
        nextSFX_source = nextSFX_source >= 30 ? 0 : nextSFX_source;
    }
    public static void PlayMusic(string name, bool loop)
    {
        Music m = Array.Find(GetInstance().music, music => music.name == name);
        if (m == null)
        {
            if(instance.debug){Debug.LogWarning("music: " + name + "not found");}
            return;
        }
        AudioManager temp = AudioManager.GetInstance();
        GetInstance().StartCoroutine(PlayMusic(m, loop));
    }
    public static Music GetMusic(string name)
    {
        return Array.Find(GetInstance().music, music => music.name == name);
    }
    public static IEnumerator PlayMusic(Music music, bool loop)
    {
        if(music.intro != null)
        {
            music_source.clip = music.intro;
            music_source.Play();
            yield return new WaitForSecondsRealtime(music_source.clip.length);
            if(!interrupt)
            {
                music_source.Stop();
            }
        }
        if(!interrupt)
        {
            music_source.loop = loop;
            music_source.clip = music.theme;
            music_source.Play();
        }
        if(music.intro == null)
        {
            interrupt = true;
        }
    }

    public static void ChangeSFXVolume(float value)
    {
        SFX_volume = value;
        for(int i = 0; i < SFX_source.Length; i++)
        {
            SFX_source[i].volume = SFX_volume * global_volume;
        }
    }
    public static void ChangeMusicVolume(float value)
    {
        music_volume = value;
        music_source.volume = music_volume * global_volume;
    }
    public static void ChangeGlobalVolume(float value)
    {
        global_volume = value;
        music_source.volume = music_volume * global_volume;
        for(int i = 0; i < SFX_source.Length; i++)
        {
            SFX_source[i].volume = SFX_volume * global_volume;
        }
    }
}