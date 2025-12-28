using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static event Action<bool> OnGameStateChanged;
    bool paused = false;
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
        Time.timeScale = 1f;
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
        gameOverText.SetActive(false);
    }

    public void SetGameOnPause(bool val)
    {
        OnGameStateChanged?.Invoke(val);
    }

    public void Loose()
    {
        gameOverText.SetActive(true);
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
