using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

interface IDominoPart
{
    public void Property();
    public void ChangeIsBeingPlacedFlag(bool val);
    public bool IsBeingPlaced();
    public void ClearAllNeighbors();

}

public enum Location
{
    left, right, up, down 
};

public class DominoPart : MonoBehaviour, IDominoPart
{
    public DominoData data;
    int loopNumber = 0;

    [SerializeField] bool isBeingPlaced = false;

    [SerializeField] Location loc;

    [SerializeField] public List<DominoPart> neighbours = new List<DominoPart>();



    public virtual void Property()
    {
        //do nothing
    }

    public void ChangeLocation(Location l)
    {
        loc = l;
    }

    public Location GetLocation()
    {
        return loc;
    }

    public int GetLoopNumber()
    {
        return loopNumber;
    }

    public void SetLoopNumber(int n)
    {
        loopNumber = n;
    }
    public void ChangeIsBeingPlacedFlag(bool val)
    {
        isBeingPlaced = val;
    }

    public bool IsBeingPlaced()
    {
        return isBeingPlaced;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.layer == 7)
        {
            AddNeighbour(other.GetComponent<DominoPart>() );
        }
    }

    void AddNeighbour(DominoPart dominoPart)
    {
        if (dominoPart.IsBeingPlaced() && isBeingPlaced && !IsAlreadyNeighbour(dominoPart) && NotAngular(dominoPart.transform))
        {
            neighbours.Add(dominoPart);
            RoadManager.Instance.CheckForLoop(this);

        }
    }

    public void RemoveNeighbor(DominoPart n)
    {
        neighbours.Remove(n);
    }
    public void ClearAllNeighbors()
    {
        foreach (DominoPart n in neighbours)
        {
            if(n)
            {
                n.RemoveNeighbor(this);
                RoadManager.Instance.CheckForLoop(n);
            }
        }
        neighbours.Clear();
    }
    bool NotAngular(Transform pos1)
    {
        return Mathf.Abs(pos1.position.x - transform.position.x) < 0.5f || Mathf.Abs(pos1.position.y - transform.position.y) < 0.5f;
    }
    bool IsAlreadyNeighbour(DominoPart neighbour)
    {
        foreach(DominoPart n in neighbours)
        {
            if (n && n == neighbour) return true;
        }
        return false;
    }

}
