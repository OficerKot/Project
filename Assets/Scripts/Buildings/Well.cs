using UnityEngine;

public class Well : Item
{
    public Vector2 placeAreaSize;
    public override bool CanPutInCell(Cell c)
    {
        return HasDominoNearby(c) && c.IsFree();
    }
    bool HasDominoNearby(Cell c)
    {
        var hits = Physics2D.OverlapBoxAll(c.transform.position, placeAreaSize, 0, LayerMask.GetMask("DominoBase"));
        if (hits.Length == 0) return false;
        return true;
    }
    public override void PutInCell()
    {
        base.PutInCell();
        GetComponent<DominoProtectionSource>().StartProtecting();
    }

    //bool IsSettlementArea()
    //{
    //    var hits = Physics2D.OverlapBoxAll(transform.position, new Vector2(0.2f, 0.2f), 0, LayerMask.GetMask("Settlement"));
    //    return hits.Length > 0;
    //}
#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, placeAreaSize);
    }
#endif
}
