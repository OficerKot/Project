using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Clock : MonoBehaviour
{
    float time;
    public static Clock Instance;
    [SerializeField] float timeSpeed;
    [SerializeField] float maxLightIntensity, minLightIntensity;
    [SerializeField] int timeLimit = 24;
    [SerializeField] Light2D globalLight;
    [SerializeField] Transform ClockHand;

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
        globalLight.intensity = maxLightIntensity; 
    }

    public void TimeTick()
    {
        time += timeSpeed;

        if (time >= timeLimit)
        {
            time = 0;
            ClockHand.rotation = Quaternion.identity;
        }

        float currentRotation = (time / (timeLimit/2)) * 360f;
        ClockHand.rotation = Quaternion.Euler(0, 0, -currentRotation);

        UpdateLighting();

    }
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
