using UnityEngine;

public class Clock : MonoBehaviour
{
    int time;
    public static Clock Instance;
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
    }

    public void TimeTick()
    {
        time++;
        RotateClockHand();
    }

    void RotateClockHand()
    {
        ClockHand.Rotate(Vector3.back, 10);
    }
}
