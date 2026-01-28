using UnityEngine;

/// <summary>
///  олодец, который можно разместить только р€дом с домино. ќбеспечивает защиту.
/// </summary>
public class Well : Item
{
    public Vector2 placeAreaSize;

    /// <summary>
    /// ѕровер€ет, можно ли разместить колодец в указанной клетке.
    /// </summary>
    /// <param name="c"> летка дл€ проверки.</param>
    /// <returns>True если р€дом есть домино и клетка свободна.</returns>
    public override bool CanPutInCell(Cell c)
    {
        return HasDominoNearby(c) && c.IsFree();
    }

    /// <summary>
    /// ѕровер€ет, есть ли р€дом с клеткой домино.
    /// </summary>
    /// <param name="c"> летка дл€ проверки.</param>
    /// <returns>True если в зоне размещени€ есть домино.</returns>
    bool HasDominoNearby(Cell c)
    {
        var hits = Physics2D.OverlapBoxAll(c.transform.position, placeAreaSize, 0, LayerMask.GetMask("DominoBase"));
        if (hits.Length == 0) return false;
        return true;
    }

    /// <summary>
    /// –азмещает колодец в клетке и активирует защиту.
    /// </summary>
    public override void PutInCell()
    {
        base.PutInCell();
        GetComponent<DominoProtectionSource>().StartProtecting();
    }

#if UNITY_EDITOR
    /// <summary>
    /// ќтрисовывает зону размещени€ колодца в редакторе Unity.
    /// </summary>
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, placeAreaSize);
    }
#endif
}