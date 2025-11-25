using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour
{
    [SerializeField] string ID;
    [SerializeField] Cell curCell;
    bool isPlaced = true;

    private void Start()
    {
        if(curCell) // сами поставили вручную перед запуском на какую то клетку
        {
            curCell.SetCurItem(this);
            curCell.SetFree(false);
            isPlaced = true;
            transform.position = curCell.transform.position;
            transform.Translate(0, 0, -curCell.transform.position.z);
        }
    }
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isPlaced && collision.gameObject.layer == 6 && !curCell)
        {
            if (!collision.GetComponent<Cell>().CheckIfFree()) return;

            curCell = collision.GetComponent<Cell>();

            curCell.Highlight();
            return;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (curCell)
        {
            curCell.NoHighlight();
            curCell = null;
        }
    }

    private void Update()
    {
        if (!isPlaced) Move();
    }
    public bool IsPlaced()
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
        Inventory.Instance.AddItem(this);
        Destroy(gameObject);
    }

    void PutInCell()
    {
        curCell.SetCurItem(this);
        curCell.SetFree(false);
        isPlaced = true;
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
