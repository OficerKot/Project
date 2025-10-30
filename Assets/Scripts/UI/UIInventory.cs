using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    [SerializeField] GameObject inventory;
    [SerializeField] GameObject[] itemsInCells = new GameObject[18];
    [SerializeField] GameObject[] cells = new GameObject[18];
    
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

    public void AddItemIcon(ItemData i, int cellIndx)
    {
        if(!isOpened) inventory.SetActive(true);
        itemsInCells[cellIndx] = Instantiate(i.UIprefab, GameObject.Find("InventoryInterface").transform);
        itemsInCells[cellIndx].transform.position = cells[cellIndx].transform.position;
        if(!isOpened) inventory.SetActive(false);
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
