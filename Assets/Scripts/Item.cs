using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour
{
    [SerializeField] string ID;
    [SerializeField] Cell curCell;
    bool isPlaced = true;

    void OnEnable()
    {
        GameManager.OnGameStateChanged += OnGameStateChanged;

    }

    void OnDisable()
    {
        GameManager.OnGameStateChanged -= OnGameStateChanged;
    }

    void OnGameStateChanged(bool isGamePaused)
    {
        GetComponent<BoxCollider2D>().enabled = !isGamePaused;
    }
    private void Start()
    {
        if(curCell) // сами поставили вручную перед запуском на какую то клетку
        {
            PutInCell();
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
        if (!Inventory.Instance.isFull())
        {
            Inventory.Instance.AddItem(ItemManager.Instance.GetItemByID(ID));
            Destroy(gameObject);
        }
        else Debug.Log("FULL INVENTORY!!!");
    }

    public void PutInCellOnSpawn(Cell cell)
    {
        curCell = cell;
        PutInCell();
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

    public void PutInCell(Cell cell)
    {
        curCell = cell;
        PutInCell();
    }
    void Move() 
    {
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetPos.z = 0;
        transform.position = Vector3.Lerp(transform.position, targetPos, 1);
    }
}
