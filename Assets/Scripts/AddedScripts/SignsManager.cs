using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Скрипт, управляющий работой табличек, расставленных по карте.
/// Скрипт получает на вход список табличек, расставленных по игровому миру, и каждый раз, когда игрок сделал шаг, запускает CastSignRays() каждой табличке в списке. 
/// 
/// Дополнительная настройка: Не требуется. Установить на пустой объект и не трогать.
/// </summary>
public class SignsManager : MonoBehaviour
{
    public static SignsManager Instance;
    private List<Sign> signsList;

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
        signsList = new List<Sign>();
    }
    public void PutInList(Sign sign)
    {
        signsList.Add(sign);
    }
    public void CastSignsRays()
    {
        foreach(Sign sign in signsList)
        {
            if (sign != null)
                sign.CastRay();
        }
    }
}
