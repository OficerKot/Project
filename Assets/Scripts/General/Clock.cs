using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Управляет игровым временем, сменой дня и ночи,
/// освещением сцены и генерацией временных событий.
/// </summary>
public class Clock : PauseBehaviour
{
    float time;
    public static Clock Instance;
    [SerializeField] float timeSpeed;
    [SerializeField] float maxLightIntensity, minLightIntensity;
    [SerializeField] int timeLimit = 24;
    float hourCounter;
    bool isActive = true;
    [SerializeField] Light2D globalLight;
    [SerializeField] Transform ClockHand;

    /// <summary>
    /// Вызывается каждый раз, когда проходит один игровой час.
    /// </summary>
    public static event Action OnHourPassed;
    /// <summary>
    /// Вызывается каждый раз, когда наступает 6am
    /// </summary>
    public static event Action NightPassed;

    public override void OnGamePaused(bool isGamePaused)
    {
        isActive = !isGamePaused;
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
    void Start()
    {
        time = 12;
        hourCounter = 0;
        globalLight.intensity = maxLightIntensity; 
    }

    /// <summary>
    /// Продвигает игровое время вперёд,
    /// обновляет освещение, стрелку часов
    /// и генерирует временные события.
    /// </summary
    public void TimeTick()
    {
        if (!isActive) return;
        CheckNightPassed();
        time += timeSpeed;
        CheckHourPassed();

        if (time >= timeLimit)
        {
            time = 0;
            ClockHand.rotation = Quaternion.identity;
        }

        float currentRotation = (time / (timeLimit/2)) * 360f;
        ClockHand.rotation = Quaternion.Euler(0, 0, -currentRotation);

        UpdateLighting();

    }

    /// <summary>
    /// Проверяет момент окончания ночи и вызывает соответствующее событие.
    /// </summary>
    void CheckNightPassed()
    {
        if(time < 6 && time + timeSpeed >= 6)
        {
            NightPassed?.Invoke();
        }
    }

    /// <summary>
    /// Отслеживает прохождение игровых часов
    /// и генерирует события для каждого часа.
    /// </summary>
    void CheckHourPassed()
    {
        hourCounter += timeSpeed;
        int hoursPassed = Mathf.FloorToInt(hourCounter);
        if (hoursPassed > 0)
        {
            for (int i = 0; i < hoursPassed; i++)
            {
                OnHourPassed.Invoke();
                //Debug.Log("An hour passed. Time: " + time);
            }
            hourCounter -= hoursPassed;
        }
    }

    /// <summary>
    /// Обновляет интенсивность глобального освещения
    /// в зависимости от текущего времени суток.
    /// </summary>
    void UpdateLighting()
    {
        if (globalLight == null) return;

        float intensity;

        if (time >= 5f && time < 8f) // Рассвет
        {
            float progress = (time - 5f) / 3f;
            intensity = Mathf.Lerp(minLightIntensity, maxLightIntensity, progress);
        }
        else if (time >= 8f && time < 18f) // День
        {
            intensity = maxLightIntensity;
        }
        else if (time >= 18f && time < 21f) // Закат
        {
            float progress = (time - 18f) / 3f;
            intensity = Mathf.Lerp(maxLightIntensity, minLightIntensity, progress);
        }
        else // Ночь
        {
            intensity = minLightIntensity;
        }

        globalLight.intensity = intensity;
    }
}
