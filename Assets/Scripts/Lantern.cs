using System.Collections.Generic;
using UnityEngine;

public class Lantern : Item
{
    public Vector2 placeAreaSize;
    public Vector2 protectAreaSize;
    List<Breakable> dominoInProtection = new List<Breakable>();
    public override bool CanPutInCell(Cell cell)
    {
        return HasDominoNearby(cell) && cell.IsFree();
    }

    public override void PutInCell()
    {
        base.PutInCell();
        Debug.Log("Put in cell called");
        StartProtecting();
    }

    private void OnDestroy()
    {
        EndProtecting();
    }
    void StartProtecting()
    {
        var hits = Physics2D.OverlapBoxAll(transform.position, protectAreaSize, 0, LayerMask.GetMask("DominoBase"));
        if (hits.Length > 0)
        {
            foreach (var domino in hits)
            {
                Breakable breakableComp = domino.GetComponent<Breakable>();
                breakableComp.AddProtection();
                Debug.Log("Added protection");
                dominoInProtection.Add(breakableComp);
            }
        }
    }
    void EndProtecting()
    {
        foreach(var domino in dominoInProtection)
        {
            domino.RemoveProtection();
        }
    }
    bool HasDominoNearby(Cell cell)
    {
        var hits = Physics2D.OverlapBoxAll(cell.transform.position, placeAreaSize, 0, LayerMask.GetMask("DominoBase"));
        if (hits.Length == 0) return false;
        return true;
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, protectAreaSize);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, placeAreaSize);
    }
#endif
}
