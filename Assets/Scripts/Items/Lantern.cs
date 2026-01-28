using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ќбъект фонарь, который можно разместить только р€дом с домино. ќбеспечивает защиту соседних домино от урона.
/// </summary>
public class Lantern : Item
{
    public Vector2 placeAreaSize;

    /// <summary>
    /// ѕровер€ет, можно ли разместить фонарь в указанной клетке.
    /// </summary>
    /// <param name="cell"> летка дл€ проверки.</param>
    /// <returns>True если р€дом есть домино и клетка свободна.</returns>
    public override bool CanPutInCell(Cell cell)
    {
        return HasDominoNearby(cell) && cell.IsFree();
    }

    /// <summary>
    /// –азмещает фонарь в клетке и активирует защиту соседних домино от урона.
    /// </summary>
    public override void PutInCell()
    {
        base.PutInCell();
        GetComponent<DominoProtectionSource>().StartProtecting();
    }

    /// <summary>
    /// ѕровер€ет, есть ли р€дом с клеткой домино.
    /// </summary>
    /// <param name="cell"> летка дл€ проверки.</param>
    /// <returns>True если в зоне размещени€ есть домино.</returns>
    bool HasDominoNearby(Cell cell)
    {
        var hits = Physics2D.OverlapBoxAll(cell.transform.position, placeAreaSize, 0, LayerMask.GetMask("DominoBase"));
        if (hits.Length == 0) return false;
        return true;
    }

#if UNITY_EDITOR
    /// <summary>
    /// ќтрисовывает зону размещени€ фонар€ в редакторе Unity.
    /// </summary>
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, placeAreaSize);
    }
#endif
}