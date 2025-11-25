using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Timeline;
using UnityEngine.UI;


public class UICraftWindow : MonoBehaviour
{
    public static UICraftWindow Instance = null;
    [SerializeField] GameObject menuObject;
    [SerializeField] GameObject curWindow;
    [SerializeField] List<Button> categoryButtons, spawnerButtons;
    public HashSet<ItemData> availableItems = new HashSet<ItemData>();
    bool isMenuOpen = false;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        foreach (Button b in categoryButtons)
        {
            b.onClick.AddListener(() => OpenCategoryMenu(b.GetComponent<MenuButton>().window));
        }
        foreach (Button b in spawnerButtons)
        {
            b.onClick.AddListener(() => Craft(b.GetComponent<ItemSpawnerButton>().objectToSpawn));
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Menu();
        }
    }

    void Menu()
    {
        if (isMenuOpen)
        {
            CloseMenu();
        }
        else
        {
            OpenMenu();
        }

    }

    public void OpenCategoryMenu(GameObject g)
    {
        curWindow = g;
        menuObject.SetActive(false);
        curWindow.SetActive(true);
        curWindow.transform.position = menuObject.transform.position;
    }

    public void CloseMenu()
    {
        if (curWindow)
        {
            curWindow.SetActive(false);
            curWindow = null;
        }
        menuObject.SetActive(false);
        isMenuOpen = false;

    }

    public void OpenMenu()
    {
        if (curWindow)
        {
            menuObject.transform.position = curWindow.transform.position;
            curWindow.SetActive(false);
            curWindow = null;
        }
        else
        {
            menuObject.transform.position = Input.mousePosition;
        }
        menuObject.SetActive(true);
        isMenuOpen = true;

    }

    public void Craft(ItemData obj)
    {
        if (availableItems.Contains(obj))
        {
            GameObject spawnedObj = Instantiate(obj.prefab, transform.position, transform.rotation);
            foreach (ItemData i in obj.itemsForCraft)
            {
                Inventory.Instance.RemoveItem(i);
            }
            spawnedObj.GetComponent<Item>().Pick();
            CheckInventory();
            CloseMenu();
        }
        else Debug.Log("You don't have all items to craft it!");
    }

    public void CheckInventory()
    {
        HashSet<ItemData> itemsInInventory = new HashSet<ItemData>();
        foreach (string id in Inventory.Instance.GetCurItems().Keys)
        {
            itemsInInventory.Add(ItemManager.Instance.GetItemByID(id));
        }
        foreach (Button b in spawnerButtons)
        {
            ItemData obj = b.GetComponent<ItemSpawnerButton>().objectToSpawn;
            if (obj.craftSet.IsSubsetOf(itemsInInventory))
            {
                availableItems.Add(obj);
                b.gameObject.SetActive(true);
            }
            else
            {
                availableItems.Remove(obj);
            }
        }
    }
    public void CheckInventory(ItemData added)
    {
        HashSet<ItemData> itemsInInventory = new HashSet<ItemData>();
        foreach (string id in Inventory.Instance.GetCurItems().Keys)
        {
            itemsInInventory.Add(ItemManager.Instance.GetItemByID(id));
        }
        foreach (Button b in spawnerButtons)
        {
            ItemData obj = b.GetComponent<ItemSpawnerButton>().objectToSpawn;
            if (obj.craftSet.IsSubsetOf(itemsInInventory))
            {
                availableItems.Add(obj);
                b.gameObject.SetActive(true);
            }
            else if (obj.craftSet.Contains(added))
            {
                b.gameObject.SetActive(true);
            }
            else
            {
                availableItems.Remove(obj);
            }
        }
    }

}
