using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    public AudioClip sfxSwoosh;
    public AudioClip sfxSwoosh2;
    public AudioClip sfxSmash;
    public AudioClip sfxScore;
    public AudioClip sfxFall;

    public static AudioManager Instance { get; private set; }
    public AudioSource Source { get; private set; }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    void Start()
    {
        Source = GetComponent<AudioSource>();
    }

    public void Play(AudioClip audio)
    {
        Source.PlayOneShot(audio);
    }
}
