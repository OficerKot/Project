using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Наименования проигрываемых звуков.
/// </summary>
[System.Serializable]
public enum SoundType
{
    BonePlace,
    Step,
    Pickup,
    NewReciepe,
    Win,
    Loose,
    /// <summary>
    /// Увеличение шкалы сытости
    /// </summary>
    HungerDown,
    /// <summary>
    /// Уменьшение шкалы сытости
    /// </summary>
    HungerUp
}
/// <summary>
/// Связь названий звуков с их звуковыми компонентами.
/// </summary>
[System.Serializable]
public class Sound
{
    public SoundType soundType;
    public AudioSource audioSource;
}

/// <summary>
/// Скрипт отвечает за воспроизведение звуков при взаимодействии игрока с окружающим миром.
/// </summary>
public class AudioManager : PauseBehaviour
{
    [SerializeField] Sound[] audios;
    Dictionary<SoundType, AudioSource> audiosDict = new Dictionary<SoundType, AudioSource>();
    private static event Action<SoundType> OnSoundRequested;

    private void Awake()
    {
        foreach (var sound in audios)
        {
            audiosDict.Add(sound.soundType, sound.audioSource);
        }
        OnSoundRequested += PlaySound;
    }
    public override void OnGamePaused(bool isGamePaused)
    {
        if (isGamePaused)
        {
            OnSoundRequested -= PlaySound;
        }
        else
        {
            OnSoundRequested += PlaySound;
        }
    }
    private void OnDestroy()
    {
        OnSoundRequested -= PlaySound;
    }
    public static void Play(SoundType soundType)
    {
        OnSoundRequested?.Invoke(soundType);
    }
    void PlaySound(SoundType soundType)
    {
        if (audiosDict.ContainsKey(soundType))
        {
            audiosDict[soundType].Play();
        }
        else Debug.Log("Sound not found.");
    }
}
