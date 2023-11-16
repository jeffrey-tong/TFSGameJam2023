using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager Instance { get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();

            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = s.output;
        }
    }

    void Start()
    {
        //if (SceneManager.GetActiveScene().buildIndex == 1)
        //Play("Level1Clip");

        if (SceneManager.GetActiveScene().buildIndex == 2)
            Play("GameOver");

    }

    public void Play(string name)
    {
        //we wnat to find the 'sound' in 'sounds' array that maches the name of the sound
        //sounds is varibale of type Sound[],
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "not found.");
            return;
        }
        s.source.Play();
    }

    public void Stop(string name)
    {
        //we wnat to find the 'sound' in 'sounds' array that maches the name of the sound
        //sounds is varibale of type Sound[],
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "not found.");
            return;
        }
        s.source.Stop();
    }

    public void Pause(string name)
    {
        //we wnat to find the 'sound' in 'sounds' array that maches the name of the sound
        //sounds is varibale of type Sound[],
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "not found.");
            return;
        }
        s.source.Pause();
    }

    public void UnPause(string name)
    {
        //we wnat to find the 'sound' in 'sounds' array that maches the name of the sound
        //sounds is varibale of type Sound[],
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "not found.");
            return;
        }
        s.source.UnPause();
    }
}
