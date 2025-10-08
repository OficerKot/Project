using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

interface IDominoPart
{
    public abstract void Property();
    public string GetImage();
    public void ChangeIsBeingPlacedFlag(bool val);
    public bool IsBeingPlaced();

}

public abstract class DominoPart : MonoBehaviour, IDominoPart
{
    [SerializeField] string image;
    [SerializeField] bool isBeingPlaced = false;

    [SerializeField] public List<DominoPart> neighbours = new List<DominoPart>();

    public abstract void Property();
    public string GetImage()
    {
        return image;
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
