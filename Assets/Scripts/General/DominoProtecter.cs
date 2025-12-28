using System;
using UnityEngine;

public class DominoProtecter : MonoBehaviour
{
    public int hoursToProtect = 48;
    public bool isProtecting;
    int cnter = 0;
    public static event Action<bool> OnProtectionStarted;
    void Start()
    {
        isProtecting = false;
        Clock.OnHourPassed += OnHourPassed;
        ObeliskManager.OnObeliskCollected += OnObeliskCollected;
    }
    void OnDestroy()
    {
        Clock.OnHourPassed -= OnHourPassed;
        ObeliskManager.OnObeliskCollected -= OnObeliskCollected;
    }

    void OnHourPassed()
    {
        if (!isProtecting) return;
        cnter--;
        if (cnter == 0)
        {
            isProtecting = false;
            OnProtectionStarted.Invoke(false);
        }
    }

    void OnObeliskCollected(ObeliskColor c)
    {
        StartProtection(hoursToProtect);
    }

    void StartProtection(int hours)
    {
        cnter = hours;
        OnProtectionStarted.Invoke(true);
        isProtecting = true;
    }
}
