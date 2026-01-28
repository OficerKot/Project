using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Building : Item
{
    public int proizvoditelnost;
    public int health;
    [SerializeField] ItemData resource;
    [SerializeField] List<Cell> curCells = new List<Cell>();
    int count = 0;
    public void MakeResource()
    {
        count += proizvoditelnost;
    }

    public override void CheckCells(Cell c)
    {
        if (curCells.Count > 0) return;
        BoxCollider2D cellCollider = c.GetComponent<BoxCollider2D>();
        Vector2 midPoint = c.transform.position;
        Vector2 cornerPoint = new Vector2(midPoint.x + cellCollider.size.x / 2, midPoint.y - cellCollider.size.y / 2);
        Collider2D[] hits = Physics2D.OverlapBoxAll(cornerPoint, cellCollider.size/2, 0, LayerMask.GetMask("Cell"));
        if (hits.Length > 0)
        {
            foreach (Collider2D hit in hits)
            {
                Cell cellComp = hit.GetComponent<Cell>();
                if(!cellComp.IsFree() || !IsSettlementArea(cellComp)) 
                {
                    return;
                }
            }

            foreach (Collider2D hit in hits)
            {
                Cell cellComp = hit.GetComponent<Cell>();
                cellComp.Highlight();
                curCells.Add(cellComp);
            }

        }

    }
    private bool IsSettlementArea(Cell c)
    {
       Collider2D[] hits = Physics2D.OverlapBoxAll(c.transform.position, new Vector2(1, 1), 0, LayerMask.GetMask("Settlement"));
       return hits.Length > 0;
    }
    public override void TurnOffHighlightedCells()
    {
        if (curCells.Count > 0 && !GetIsPlaced())
        {
            foreach(Cell cell in curCells)
            {
                cell.NoHighlight();
            }
            curCells.Clear();
        }
    }
    public override void OnMouseDown()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !GetIsPlaced() && curCells.Count>0)
        { 
            PutInCell();
        }
        if (resource != null && count > 0)
        {
            PickResource();
        }
    }
    private void OnDestroy()
    {
        StopProducing();
    }
    public override void PutInCell()
    {
        foreach (Cell cell in curCells)
        {
            cell.NoHighlight();
            cell.SetCurItem(gameObject);
            cell.SetFree(false);
        }
        Cell curCell = curCells[0];
        SetIsPlaced(true);
        InvokeAction();
        transform.position = curCell.transform.position;
        transform.Translate(0, 0, -curCell.transform.position.z);
        Inventory.Instance.RemoveItem(ItemManager.Instance.GetItemByID(GetID()));
        GameManager.Instance.PutInHand(null);

        StartProducing();
    }
    public virtual void StartProducing()
    {
        Clock.NightPassed += MakeResource;
    }
    public virtual void StopProducing()
    {
        Clock.NightPassed -= MakeResource;
    }

    void PickResource()
    {
        if(!Inventory.Instance.isFull())
        {
            Inventory.Instance.AddItem(resource,count);
            count = 0;
        }
    }

}
