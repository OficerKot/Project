using UnityEngine;
using UnityEngine.UI;

public class Hunger : MonoBehaviour
{
    public const int stepsLimit = 5;
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
        else
        {
            thisImageAnimator.ForcePlay(hungerLevels[hungerLevel]);
        }
    }

    void HungerUp()
    {
        if (hungerLevel > 0)
            hungerLevel--;
        thisImageAnimator.ForcePlay(hungerLevels[hungerLevel]);
    }
    public void CallHungerUp()
    {
        HungerUp();
    }
}
