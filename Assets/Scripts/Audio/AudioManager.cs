﻿using System.Collections.Generic;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<AudioClip> soundsList = new List<AudioClip>();

    private Dictionary<string, AudioClip> soundsDic = new Dictionary<string, AudioClip>();

    private void Start()
    {
        SoundDicInit();
        GameEvents.Instance.TileClicked += PlaySound;
        GameEvents.Instance.WrongTileClicked += PlaySound;
        GameEvents.Instance.TilesSwapped += PlaySound;
    }

    private void SoundDicInit()
    {
        foreach (AudioClip sound in soundsList)
        {
            soundsDic.Add(sound.name, sound);
        }
    }

    private void PlaySound(object sender, MyEventsArgs args)
    {
        audioSource.clip = soundsDic[args.clipName];
        audioSource.Play();
    }

    public void PlaySound(string clipName)
    {
        audioSource.clip = soundsDic[clipName];
        audioSource.Play();
    }

    private void OnDisable()
    {
        GameEvents.Instance.TileClicked -= PlaySound;
    }
}