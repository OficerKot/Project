using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    [SerializeField] GameObject inventory;
    [SerializeField] UIInventoryCell[] cells = new UIInventoryCell[6];
    
    [SerializeField] Button inventoryButton;

    public static UIInventory Instance;
    bool isOpened = false;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

    }

  
    void Start()
    {
        inventoryButton.onClick.AddListener(Interact);
    }

    void Interact()
    {
        if(!isOpened)
        {
            isOpened = true;
            Open();
        }
        else
        {
            isOpened = false;
            Close();
        }
    }

    public void AddNewItem(ItemData i, int cellIndx)
    {
        if(!isOpened) inventory.SetActive(true);
        cells[cellIndx].PutItem(Instantiate(i.UIprefab, GameObject.Find("InventoryInterface").transform));
        if(!isOpened) inventory.SetActive(false);
    }

    public void AddOneMoreItem(int cellIndx)
    {
        cells[cellIndx].AddToCounter(1);
    }

    public void RemoveOneItem(int cellIndx)
    {
        cells[cellIndx].AddToCounter(-1);
    }
    public void RemoveItemIcon(int cellIndx)
    {
        cells[cellIndx].RemoveItem();
    }
    void Open()
    {
        inventory.SetActive(true);
    }

    void Close()
    {
        inventory.SetActive(false);
    }
}
