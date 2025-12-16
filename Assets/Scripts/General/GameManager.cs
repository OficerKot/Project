using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static event Action<bool> OnGameStateChanged;
    bool isGameOver;
    [SerializeField] GameObject gameOverText;
    [SerializeField] private Camera gameCamera;

    GameObject inHand = null;
    private void Awake()
    {
        if(Instance == null)
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
        SceneManager.LoadScene(0);
    }

    private void Start()
    {
        isGameOver = false;
        gameOverText.SetActive(false);
    }



    public void SetGameOver(bool val)
    {
        isGameOver = val;
        OnGameStateChanged?.Invoke(val);
        gameOverText.SetActive(val);

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
