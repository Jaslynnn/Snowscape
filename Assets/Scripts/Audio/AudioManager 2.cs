using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    private void Awake()
    {
        foreach(Sound s in sounds) 
        { 
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        
        
        }
    }

    void Start()
    {
        Play("Theme");
    }

    public void Play ( string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null)
        {
            Debug.LogWarning("Sound: " + name + "not found!");
            return;
        }

        s.source.Play();
        

    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "not found!");
            return;
        }

        s.source.Stop();
    }

    public void AdjustVolume(string name, float volume)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "not found!");
            return;
        }

        s.source.volume = volume;
    }

    public IEnumerator DelayedPlay(string name, float delay)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "not found!");
           
        }
        yield return new WaitForSeconds(delay);

        s.source.Play();
    }
    public void PlayOptionSelected()
    {
        FindObjectOfType<AudioManager>().Play("OptionSelected");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
