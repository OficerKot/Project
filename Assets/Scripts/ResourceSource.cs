using System;
using UnityEngine;

public class ResourceSource : MonoBehaviour, Interactable
{
    [SerializeField] public ItemData resource;
    [SerializeField] ImageEnumerator toolToDestroy;
    [SerializeField] public Cell curCell;

    private void OnDisable()
    {
        if(curCell)
        {
            curCell.OnCellOccupied -= HandleCellOccupied;
        }
    }
    public virtual void Pick()
    {
        Inventory.Instance.AddItem(resource);
        curCell.SetFree();
        Destroy(gameObject);
    }
    public virtual void PutInCell(Cell cell)
    {
        curCell = cell;
        PutInCell();
        curCell.OnCellOccupied += HandleCellOccupied;
    }

    void HandleCellOccupied()
    {
        Pick();
    }
    public virtual void PutInCell()
    {
        curCell.SetImage(toolToDestroy);
        curCell.SetCurItem(gameObject);
        transform.position = curCell.transform.position;
        transform.Translate(0, 0, -curCell.transform.position.z);
    }
}
