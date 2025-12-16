using UnityEngine;
<<<<<<< Updated upstream
<<<<<<< Updated upstream
<<<<<<< Updated upstream

public class Clock : MonoBehaviour
{
    int time;
    public static Clock Instance;
=======
=======
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
<<<<<<< Updated upstream
<<<<<<< Updated upstream
        time = 0;
=======
=======
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
        globalLight.intensity = maxLightIntensity;
        lightIntensityDestination = -1;
        time = 0;

<<<<<<< Updated upstream
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
    }

    public void TimeTick()
    {
<<<<<<< Updated upstream
<<<<<<< Updated upstream
<<<<<<< Updated upstream
        time++;
        RotateClockHand();
    }

    void RotateClockHand()
    {
        ClockHand.Rotate(Vector3.back, 10);
    }
=======
=======
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
        CalculateSteps();
        time += timeStep;
        if (time > timeLimit)
        {
            time = 0;
            lightIntensityDestination *= -1;
        }
        globalLight.intensity += lightIntensityDestination * lightStep;
        Debug.Log(globalLight.intensity);
        ClockHand.Rotate(Vector3.back, rotationStep);
    }

    void CalculateSteps()
    {
        timeStep = 1f / (timeLimit * timeSpeed); 
        lightStep = maxLightIntensity / (timeLimit * timeSpeed); 
        rotationStep = 360f / (timeLimit * timeSpeed); 
    }
 
<<<<<<< Updated upstream
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
}
