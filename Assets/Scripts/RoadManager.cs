using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using System.Numerics;

interface IRoadManager
{
    public void CheckForLoop(DominoPart d);
    public bool HasLoopDFS(DominoPart start, DominoPart prrevious = null);
}

public class RoadManager : MonoBehaviour, IRoadManager
{
    public static RoadManager Instance; // шаблон singleton

    [SerializeField] HashSet<DominoPart> visited = new HashSet<DominoPart>();
    [SerializeField] HashSet<List<DominoPart>> alreadyCycles = new HashSet<List<DominoPart>>(); // тут должны будут храниться кольца, чтобы не учитывали их повторно

    [SerializeField] List<DominoPart> curWay = new List<DominoPart>();
    [SerializeField] List<DominoPart> nodes = new List<DominoPart>();


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CheckForLoop(DominoPart d)
    {
        visited.Clear();

        if(HasLoopDFS(d))
        {
            Debug.Log("Has loop");
        }
        else
        {
            Debug.Log("No loop");
        }

    }
    public bool HasLoopDFS(DominoPart start, DominoPart previous = null)
    {
        foreach (DominoPart neighbour in start.neighbours)
        {
            if (neighbour)
            {
                if (!visited.Contains(neighbour))
                {
                    visited.Add(neighbour);
                    return HasLoopDFS(neighbour, start);
                }
                else if (neighbour != previous) return true;
            }
        }
        return false;
    }

}
