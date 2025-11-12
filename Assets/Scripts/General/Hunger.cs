using UnityEngine;
using UnityEngine.UI;

public class Hunger : MonoBehaviour
{
    public const int stepsLimit = 5;
    public static Hunger Instance;
    [SerializeField] Sprite[] hungerLevels;
    Image thisImage;
    int hungerLevel, stepsCounter;

    void Start()
    {
        thisImage = GetComponent<Image>();
        hungerLevel = 0;
        thisImage.sprite = hungerLevels[hungerLevel];
        stepsCounter = 0;
    }

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

    public void MakeStep()
    {
        stepsCounter++;
        if(stepsCounter >= stepsLimit)
        {
            stepsCounter = 0;
            HungerDown();
        }
    }

    void HungerDown()
    {
        hungerLevel++;
        if (hungerLevel == hungerLevels.Length)
        {
            GameManager.Instance.SetGameOver(true);
            enabled = false;
        }
        thisImage.sprite = hungerLevels[hungerLevel];
    }

    void HungerUp()
    {

    }
}
