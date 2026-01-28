
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Блокирует распространение дублируемых предметов на клетки в радиусе доступности
/// </summary>
public class DuplicationBlocker : MonoBehaviour
{
    /// <summary>
    /// Радиус блокировки
    /// </summary>
    public Vector2 areaSize;
    Item thisItem;
    [SerializeField] List<Cell> affectedCells = new List<Cell>();

    private void OnEnable()
    {
        thisItem = GetComponent<Item>();
        thisItem.OnItemPlaced += OnItemPlaced;
    }

    /// <summary>
    /// Начало блокировки клеток после установки текущего объекта.
    /// </summary>
    private void OnItemPlaced()
    {
        var hits = Physics2D.OverlapBoxAll(transform.position, areaSize, 0, LayerMask.GetMask("Cell"));
        foreach (var cell in hits)
        {
            Cell c = cell.GetComponent<Cell>();
            affectedCells.Add(c);
            c.AddDuplicationBlocker();
        }
    }

    /// <summary>
    /// Снятие блокировки со все клеток, на которых она установлена.
    /// </summary>
    private void OnDestroy()
    {
        if (thisItem)
        {
            thisItem.OnItemPlaced -= OnItemPlaced;
            foreach (var cell in affectedCells)
            {
                cell.RemoveDuplicationBlocker();
            }
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, areaSize);
    }
#endif
}

