using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ѕазовый класс дл€ зданий, которые можно размещать на поле и которые производ€т ресурсы.
/// </summary>
public class Building : Item
{
    public int proizvoditelnost;
    public int health;
    Vector2 cornerPoint;
    [SerializeField] ItemData resource;
    [SerializeField] List<Cell> curCells = new List<Cell>();
    int count = 0;

    /// <summary>
    /// ѕровер€ет клетки дл€ размещени€ здани€ и подсвечивает доступные.
    /// </summary>
    /// <param name="c"> летка, от которой начинаетс€ проверка.</param>
    public override void CheckCells(Cell c)
    {
        if (curCells.Count > 0) return;
        BoxCollider2D cellCollider = c.GetComponent<BoxCollider2D>();
        Vector2 midPoint = c.transform.position;
        cornerPoint = new Vector2(midPoint.x + cellCollider.size.x / 2, midPoint.y - cellCollider.size.y / 2);
        Collider2D[] hits = Physics2D.OverlapBoxAll(cornerPoint, cellCollider.size / 2, 0, LayerMask.GetMask("Cell"));
        if (hits.Length > 0)
        {
            foreach (Collider2D hit in hits)
            {
                Cell cellComp = hit.GetComponent<Cell>();
                if (!cellComp.IsFree() || !IsSettlementArea(cellComp))
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

    /// <summary>
    /// ѕровер€ет, находитс€ ли клетка в зоне поселени€.
    /// </summary>
    private bool IsSettlementArea(Cell c)
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(c.transform.position, new Vector2(1, 1), 0, LayerMask.GetMask("Settlement"));
        return hits.Length > 0;
    }

    /// <summary>
    /// —нимает подсветку с проверенных клеток.
    /// </summary>
    public override void TurnOffHighlightedCells()
    {
        if (curCells.Count > 0 && !GetIsPlaced())
        {
            foreach (Cell cell in curCells)
            {
                cell.NoHighlight();
            }
            curCells.Clear();
        }
    }

    /// <summary>
    /// ќбрабатывает нажатие мыши на здание.
    /// </summary>
    public override void OnMouseDown()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !GetIsPlaced() && curCells.Count > 0)
        {
            PutInCell();
        }
        if (GetIsPlaced() && resource != null)
        {
            PickResource();
        }
    }

    private void OnDestroy()
    {
        StopProducing();
    }

    /// <summary>
    /// –азмещает здание в выбранных клетках.
    /// </summary>
    public override void PutInCell()
    {
        foreach (Cell cell in curCells)
        {
            cell.NoHighlight();
            cell.SetCurItem(gameObject);
            cell.SetFree(false);
        }
        transform.position = cornerPoint;
        SetIsPlaced(true);
        InvokeAction();
        Inventory.Instance.RemoveItem(ItemManager.Instance.GetItemByID(GetID()));
        GameManager.Instance.PutInHand(null);

        StartProducing();
    }

    /// <summary>
    /// Ќачинает производство ресурсов (подписываетс€ на событие смены ночи).
    /// </summary>
    public virtual void StartProducing()
    {
        Clock.NightPassed += MakeResource;
    }

    /// <summary>
    /// ќстанавливает производство ресурсов (отписываетс€ от событи€ смены ночи).
    /// </summary>
    public virtual void StopProducing()
    {
        Clock.NightPassed -= MakeResource;
    }

    /// <summary>
    /// ѕроизводит ресурсы при наступлении ночи.
    /// </summary>
    public void MakeResource()
    {
        count += proizvoditelnost;
    }

    /// <summary>
    /// «абирает накопленные ресурсы из здани€.
    /// </summary>
    void PickResource()
    {
        if (Inventory.Instance.Contains(resource) || !Inventory.Instance.IsFull())
        {
            if (count > 0)
            {
                Inventory.Instance.AddItem(resource, count);
                Debug.Log("You've picked " + resource.name + "x" + count);
                count = 0;
            }
            else
            {
                Debug.Log("No food yet. Come back later after 6am.");
            }
        }
        else
        {
            Debug.Log("Full inventory");
        }
    }
    
}