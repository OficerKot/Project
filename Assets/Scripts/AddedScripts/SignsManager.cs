using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

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
