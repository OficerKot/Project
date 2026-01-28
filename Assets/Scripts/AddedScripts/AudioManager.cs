using UnityEngine;

public class AudioManager : PauseBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private AudioSource[] audios;
    bool isActive = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public override void OnGamePaused(bool isGamePaused)
    {
        isActive = !isGamePaused;
    }

    public void Step()
    {
        audios[0].Play();
    }
    public void Boneplace()
    {
        audios[1].Play();
    }
    public void Pickup()
    {
        audios[2].Play();
    }
}
