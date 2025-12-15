using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    [SerializeField] GameObject inventory;
    public UIInventoryCell[] cellsList;
    Dictionary<UIInventoryCell, ItemData> cells = new Dictionary<UIInventoryCell, ItemData>();

    [SerializeField] Button inventoryButton;

    public static UIInventory Instance;
    bool isOpened = false;

    private void Awake()
    {
        if (Instance == null)
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
        for (int i = 0; i < Inventory.MAX_SIZE; i++)
        {
            cells.Add(cellsList[i], null);
        }

        inventoryButton.onClick.AddListener(Interact);
    }

    void Interact()
    {
        if (!isOpened)
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

    public void AddNewItem(ItemData item)
    {
        if (!isOpened) inventory.SetActive(true);
        for (int i = 0; i < cells.Count; i++)
        {
            UIInventoryCell cell = cellsList[i];
            if (cells[cell]) continue;
            else
            {
                cells[cell] = item;
                cell.PutItem(Instantiate(item.UIprefab, GameObject.Find("InventoryInterface").transform));
                break;
            }
        }

        if (!isOpened) inventory.SetActive(false);
    }

    public void AddOneMoreItem(ItemData item)
    {
        for (int i = 0; i < cells.Count; i++)
        {
            UIInventoryCell cell = cellsList[i];
            if (cells[cell] == item)
            {
                cell.AddToCounter(1);
            }
        }
    }

    public void RemoveOneItem(ItemData item)
    {
        for (int i = 0; i < cells.Count; i++)
        {
            UIInventoryCell cell = cellsList[i];
            if (cells[cell] == item)
            {
                cell.AddToCounter(-1);
            }
        }
    }
    public void RemoveItemIcon(ItemData item)
    {
        for (int i = 0; i < cells.Count; i++)
        {
            UIInventoryCell cell = cellsList[i];
            if (cells[cell] == item)
            {
                cells[cell] = null;
                cell.RemoveItem();
            }
        }
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
