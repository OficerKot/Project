using System;
using UnityEngine;

/// <summary>
/// Менеджер защиты всех домино на сцене при активации определённых событий. На данный момент - при сборе обелиска.
/// </summary>
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

    /// <summary>
    /// Обработчик события прохождения часа, уменьшает счетчик защиты.
    /// </summary>
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

    /// <summary>
    /// Обработчик события сбора обелиска, запускает защиту.
    /// </summary>
    /// <param name="c">Цвет собранного обелиска.</param>
    void OnObeliskCollected(ObeliskColor c)
    {
        StartProtection(hoursToProtect);
    }

    /// <summary>
    /// Запускает защиту всех домино на указанное количество часов.
    /// </summary>
    /// <param name="hours">Количество часов для защиты.</param>
    void StartProtection(int hours)
    {
        cnter = hours;
        OnProtectionStarted.Invoke(true);
        isProtecting = true;
    }
}