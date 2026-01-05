using System;
using UnityEngine;

public class ResourceSource : MonoBehaviour, Interactable, IBreakableObject
{
    [SerializeField] public ItemData resource;
    [SerializeField] ImageEnumerator toolToDestroy;
    [SerializeField] public Cell curCell;


    public virtual void Pick()
    {
        if (resource)
        {
            Inventory.Instance.AddItem(resource);
        }
        curCell.SetCurItem(null);
        Destroy(gameObject);
    }
    public virtual void PutInCell(Cell cell)
    {
        curCell = cell;
        PutInCell();
    }

    public virtual bool CanBreak(DominoPart cur, DominoPart other)
    {
        return cur.data.image == toolToDestroy || other.data.image == toolToDestroy || toolToDestroy == ImageEnumerator.any;
    }
    public void Break()
    {
        Pick();
    }
    public virtual void PutInCell()
    {
        curCell.SetNumber(0);
        curCell.SetImage(toolToDestroy);
        curCell.SetCurItem(gameObject);
        transform.position = curCell.transform.position;
        transform.Translate(0, 0, -curCell.transform.position.z);
    }
}
