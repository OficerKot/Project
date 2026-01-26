using System.Collections.Generic;
using UnityEngine;

public class Lantern : Item
{
    public Vector2 placeAreaSize;

    public override bool CanPutInCell(Cell cell)
    {
        return HasDominoNearby(cell) && cell.IsFree();
    }

    public override void PutInCell()
    {
        base.PutInCell();
        GetComponent<DominoProtectionSource>().StartProtecting();
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
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, placeAreaSize);
    }
#endif
}
