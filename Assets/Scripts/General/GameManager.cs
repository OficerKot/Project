using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
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

    public void Restart()
    {
        Pause();
        SceneManager.LoadScene(0);
    }

    public void Exit()
    {
        Application.Quit();
    }

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

    public void SetGameOnPause(bool val)
    {
        OnGamePaused?.Invoke(val);
    }

    public void Loose()
    {
        gameOverText.SetActive(true);
        gameEnd = true;
        Pause();
    }
    public void Win()
    {
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
