using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Управляет системой голода игрока с пошаговым уменьшением и визуализацией.
/// </summary>
public class Hunger : MonoBehaviour
{
    public int stepsLimit = 5;
    public static Hunger Instance;
    [SerializeField] string[] hungerLevels;
    ImageAnimator thisImageAnimator;
    int hungerLevel, stepsCounter;

    void Start()
    {
        thisImageAnimator = GetComponent<ImageAnimator>();
        hungerLevel = 0;
        stepsCounter = 0;
    }

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

    /// <summary>
    /// Учитывает шаг игрока, при достижении лимита уменьшает уровень сытости.
    /// </summary>
    public void MakeStep()
    {
        if (Time.timeScale == 0) return;
        stepsCounter++;
        if (stepsCounter >= stepsLimit)
        {
            stepsCounter = 0;
            HungerUp();
        }
    }

    /// <summary>
    /// Уменьшает уровень сытости. При достижении максимума вызывает проигрыш.
    /// </summary>
    void HungerUp()
    {
        hungerLevel++;
        if (hungerLevel >= hungerLevels.Length)
        {
            GameManager.Instance.Loose();
            enabled = false;
        }
        else
        {
            AudioManager.Play(SoundType.HungerUp);
            thisImageAnimator.ForcePlay(hungerLevels[hungerLevel]);
        }
    }

    /// <summary>
    /// Увеличивает уровень сытости.
    /// </summary>
    void HungerDown()
    {
        if (hungerLevel > 0)
            hungerLevel--;
        AudioManager.Play(SoundType.HungerDown);
        thisImageAnimator.ForcePlay(hungerLevels[hungerLevel]);
    }

    /// <summary>
    /// Публичный метод для увеличения сытости.
    /// </summary>
    public void CallHungerUp()
    {
        HungerDown();
    }

    /// <summary>
    /// Публичный метод для уменьшения сытости.
    /// </summary>
    public void CallHungerDown()
    {
        HungerUp();
    }
}