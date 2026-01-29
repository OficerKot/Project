using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Скрипт для управления врагами на карте. Содержит список объектов-врагов, каждому из них посылает команду. Работает в связке с EnemyMovement.
/// Дополнительная настройка: не требуется.
/// </summary>
public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;
    private List<EnemyMovement> enemiesMv;
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
        enemiesMv = new List<EnemyMovement>();
    }
    public void PutInList(EnemyMovement enemy)
    {
        enemiesMv.Add(enemy);
    }

    public void MakeStep()
    {
        foreach(EnemyMovement enemy in enemiesMv)
        {
            enemy.SetIsMoving(true);
        }
    }
}
