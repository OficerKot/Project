using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public interface Interactable
{
    public void Pick();
    public void PutInCell(Cell cell);
    public void PutInCell();
}

public class Item : PauseBehaviour, Interactable
{
    [SerializeField] string ID;
    [SerializeField] Cell curCell;
    bool isPlaced = true;
    public event Action OnItemPlaced;

    public void OnMouseDown()
    {
        if (!Inventory.Instance.isFull() && isPlaced)
        {
            Pick();
        }

        else if (Input.GetKeyDown(KeyCode.Mouse0) && curCell)
        {
            curCell.NoHighlight();
            PutInCell();
        }

    }
    public virtual bool CanPutInCell(Cell c) // у разных предметов могут быть разные условия для их установки
    {
        return c.IsFree();
    }

    private void OnDestroy()
    {
        if(curCell)
        {
            curCell.SetFree();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isPlaced && collision.gameObject.layer == 6 && !curCell)
        {
            Cell cell = collision.GetComponent<Cell>();
            if (!CanPutInCell(cell)) return;

            curCell = cell;
            curCell.Highlight();
            return;

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (curCell && !isPlaced)
        {
            curCell.NoHighlight();
            curCell = null;
        }
    }
    private void Update()
    {
        if (!isPlaced) Move();
    }
    public bool GetIsPlaced()
    {
        return isPlaced;
    }
    public void SetIsPlaced(bool val)
    {
        isPlaced = val;
    }
    public string GetID()
    {
        return ID;
    }
    public void Pick()
    {
        if (!Inventory.Instance.isFull())
        {
            Inventory.Instance.AddItem(ItemManager.Instance.GetItemByID(ID));
            AudioManager.Instance.Pickup();
            Destroy(gameObject);
        }
    }
    public void PutInCell(Cell cell)
    {
        curCell = cell;
        PutInCell();
    }
    public virtual void PutInCell()
    {
        curCell.SetCurItem(gameObject);
        curCell.SetFree(false);
        isPlaced = true;
        OnItemPlaced?.Invoke();
        transform.position = curCell.transform.position;
        transform.Translate(0, 0, -curCell.transform.position.z);
        Inventory.Instance.RemoveItem(ItemManager.Instance.GetItemByID(ID));
        GameManager.Instance.PutInHand(null);
    }
    void Move() 
    {
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetPos.z = 0;
        transform.position = Vector3.Lerp(transform.position, targetPos, 1);
    }
}
