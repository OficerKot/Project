using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

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
