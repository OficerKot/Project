using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Hunger : MonoBehaviour
{
    public int stepsLimit = 5;
    public static Hunger Instance;
    [SerializeField] string[] hungerLevels;
    [SerializeField] TMP_InputField hungerInput;
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

    public void MakeStep()
    {
        if (hungerInput.text == "")
        {
            stepsLimit = 5;
        }
        else
        {
            stepsLimit = int.Parse(hungerInput.text);
        }

        stepsCounter++;
        if (stepsCounter >= stepsLimit)
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

    }
}
