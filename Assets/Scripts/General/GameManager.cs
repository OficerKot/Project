using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Управляет игровым процессом: начало, перезагрузка, окончание игры, выход из приложения. 
/// Хранит в себе объект, находящийся в руке игрока (в будущем будет вынесено в отдельный скрипт)
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    /// <summary>
    /// Событие, активируемое при установке игры на паузу или снятии с паузы.
    /// </summary>
    public static event Action<bool> OnGamePaused;
    bool paused = false;
    public bool gameEnd = false;
    [SerializeField] GameObject gameOverText;
    [SerializeField] GameObject winText;
    [SerializeField] private Camera gameCamera;

    GameObject inHand = null;
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
    /// Перезагрузка уровня
    /// </summary>
    public void Restart()
    {
        Pause();
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Выход из игры, закрытие приложения.
    /// </summary>
    public void Exit()
    {
        Application.Quit();
    }

    /// <summary>
    /// Пауза.
    /// </summary>
    public void Pause()
    {
        paused = !paused; 
        Time.timeScale = paused ? 0f : 1f;
        SetGameOnPause(paused);
    }
    private void Start()
    {
        gameEnd = false;
        winText.SetActive(false);
        gameOverText.SetActive(false);
    }

    /// <summary>
    /// Активация события паузы.
    /// </summary>
    /// <param name="val">true - игра приостановится, false - игра возообновится</param>
    public void SetGameOnPause(bool val)
    {
        OnGamePaused?.Invoke(val);
    }

    /// <summary>
    /// Окончание игры с проигрышем.
    /// </summary>
    public void Loose()
    {
        AudioManager.Play(SoundType.Loose);
        gameOverText.SetActive(true);
        gameEnd = true;
        Pause();
    }
    /// <summary>
    /// Окончание игры с выигрышем.
    /// </summary>
    public void Win()
    {
        AudioManager.Play(SoundType.Win);
        winText.SetActive(true);
        Pause();
    }

    public bool IsHandFree()
    {
        return inHand = null;
    }

    public GameObject WhatInHand()
    {
        return inHand;
    }
    public void PutInHand(GameObject obj)
    {
        inHand = obj;
    }
    public Camera GetCamera()
    {
        return gameCamera;
    }
}
