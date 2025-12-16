using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Clock : MonoBehaviour
{
    float time;
    int lightIntensityDestination;
    public static Clock Instance;
    float timeStep, lightStep, rotationStep;
    [SerializeField] float timeSpeed;
    [SerializeField] float maxLightIntensity;
    [SerializeField] int timeLimit = 12;
    [SerializeField] Light2D globalLight;
    [SerializeField] Transform ClockHand;
 

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
    void Start()
    {
        time = 0;
        globalLight.intensity = maxLightIntensity;
        lightIntensityDestination = -1;
        CalculateSteps();

    }

    public void TimeTick()
    {
        time += timeStep;
        Debug.Log("Cur time: " + time + ", time limit: " + timeLimit);
        if (time > timeLimit)
        {
            time = 0;
            lightIntensityDestination *= -1;
        }
        if (lightIntensityDestination < 0 && globalLight.intensity <= 0)
        {
            globalLight.intensity = 0;
        }
        else if (lightIntensityDestination > 0 && globalLight.intensity >= maxLightIntensity)
        {
            globalLight.intensity = maxLightIntensity;
        }
        globalLight.intensity += lightIntensityDestination * lightStep;
        ClockHand.Rotate(Vector3.back, rotationStep);
    }

    void CalculateSteps()
    {
        timeStep = 1 / timeSpeed; 
        lightStep = maxLightIntensity / (timeLimit * timeSpeed); 
        rotationStep = 360f / (timeLimit * timeSpeed); 
    }

}
