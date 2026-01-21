
using System.Collections.Generic;
using UnityEngine;

public class DuplicationBlocker : MonoBehaviour
{
    public Vector2 areaSize;
    Item thisItem;
    [SerializeField] List<Cell> affectedCells = new List<Cell>();

    private void Start()
    {
        thisItem = GetComponent<Item>();
        thisItem.OnItemPlaced += OnItemPlaced;
    }
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

    private void OnDestroy()
    {
        thisItem.OnItemPlaced -= OnItemPlaced;
        foreach (var cell in affectedCells)
        {
            cell.RemoveDuplicationBlocker();
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

