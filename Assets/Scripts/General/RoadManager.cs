using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;


interface IRoadManager
{
    public void CheckForLoop(DominoPart d);
}

public class RoadManager : MonoBehaviour, IRoadManager
{
    public static RoadManager Instance;
    [SerializeField] int loopsCnt = 1;

    [SerializeField] public GameObject[] settlementsPrefabs;

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

        HashSet<DominoPart> visited = new HashSet<DominoPart>();
        HashSet<DominoPart> curWay = new HashSet<DominoPart>();

        visited.Clear();
        curWay.Clear();

        if (HasLoopDFS(d, curWay, visited))
        {
            RemoveNonCycleVertex(d, curWay);
            SpawnSettlement(curWay);
            SetNumbersToVertexInThisLoop(curWay);
        }
    }

    void RemoveNonCycleVertex(DominoPart vertex, HashSet<DominoPart> curWay, HashSet<DominoPart> visited = null)
    {
        Debug.Log("Visiting " + vertex.name);
        if (visited == null) visited = new HashSet<DominoPart>();
        visited.Add(vertex);

        if (CountNeighbours(vertex, curWay) < 2  && curWay.Contains(vertex))
        {
            Debug.Log("Removed " + vertex.name);
            curWay.Remove(vertex);
            RemoveNonCycleVertex(vertex, curWay, visited);
        }
        else
        {
            foreach (DominoPart n in vertex.neighbours)
            {
                if (!visited.Contains(n))
                {
                    RemoveNonCycleVertex(n, curWay, visited);
                }
            }
        }
    }

    int CountNeighbours(DominoPart vertex, HashSet<DominoPart> curWay)
    {
        int count = 0;
        foreach(DominoPart n in vertex.neighbours)
        {
            if (curWay.Contains(n)) count++;
        }
        return count;
    }
    int CountNeighboursInCycle(DominoPart vertex)
    {
        int cnt = 0;
        foreach(DominoPart d in vertex.neighbours)
        {
            if(d.GetLoopNumber() != 0)
            {
                cnt++;
            }
        }
        return cnt - CountNeighboursInCycle(vertex);
    }
    void SetNumbersToVertexInThisLoop(HashSet<DominoPart> curWay)
    {
        loopsCnt++;
        foreach (var vertex in curWay)
        {
            vertex.SetLoopNumber(loopsCnt);
        }
    }

    void SpawnSettlement(HashSet<DominoPart> curWay)
    {
        {
            const float zPos = 0.16f;
            float[] Coords = GetSettlementCoords(curWay);
            float minX = Coords[0];
            float maxX = Coords[1];
            float minY = Coords[2];
            float maxY = Coords[3];

            Vector3 settlementPos = new Vector3((maxX + minX) / 2, (maxY + minY) / 2, zPos);
            Vector3 settlementScale = new Vector3(maxX - minX, maxY - minY);
            GameObject s = Instantiate(settlementsPrefabs[0], settlementPos, transform.rotation);
            s.transform.localScale = settlementScale;
            Debug.Log(minX + " " + maxX + " " + minY + " " + maxY);

        }
    }

    float[] GetSettlementCoords(HashSet<DominoPart> curWay)
    {
        float minX = float.MaxValue, maxX = float.MinValue, minY = float.MaxValue, maxY = float.MinValue;
        float[] coords = { minX, maxX, minY, maxY };
        foreach (var vertex in curWay)
        {
            Vector3 pos = vertex.transform.position;

            coords[0] = Mathf.Min(coords[0], pos.x);
            coords[1] = Mathf.Max(coords[1], pos.x);
            coords[2] = Mathf.Min(coords[2], pos.y);
            coords[3] = Mathf.Max(coords[3], pos.y);
        }
        return coords;
    }
    bool HasLoopDFS(DominoPart start, HashSet<DominoPart> visited, HashSet<DominoPart> curWay, DominoPart previous = null)
    {
        visited.Add(start);
        curWay.Add(start);

        foreach (DominoPart neighbour in start.neighbours)
        {
            if (neighbour && neighbour.GetLoopNumber() == 0)
            {
                if (!visited.Contains(neighbour))
                {
                    if (HasLoopDFS(neighbour, curWay, visited, start))
                    {
                        return true;
                    }
                }
                else if (neighbour != previous && curWay.Count > 4) return true;
            }

        }
        curWay.Remove(start);
        return false;
    }

}
