using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Timeline;
using UnityEngine.UI;


public class UICraftWindow : Menu
{
    public static UICraftWindow Instance = null;
    [SerializeField] GameObject menu;
    [SerializeField] GameObject curWindow;
    [SerializeField] List<MenuButton> categoryButtons;
    [SerializeField] List<ItemSpawnerButton> spawnerButtons;
    public HashSet<ItemData> availableItems = new HashSet<ItemData>();
    public HashSet<ItemData> exploredRecipes = new HashSet<ItemData>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        foreach (MenuButton b in categoryButtons)
        {
            b.GetComponent<Button>().onClick.AddListener(() => OpenCategoryMenu(b.GetComponent<MenuButton>().window));
        }
        foreach (ItemSpawnerButton b in spawnerButtons)
        {
            b.GetComponent<Button>().onClick.AddListener(() => Craft(b.GetComponent<ItemSpawnerButton>().objectToSpawn));
        }
    }

    public override void Close()
    {
        if (curWindow)
        {
            curWindow.SetActive(false);
            curWindow = null;
        }
        menu.SetActive(false);
    }

    public override void Open()
    {
        if (curWindow)
        {
            menu.transform.position = curWindow.transform.position;
            curWindow.SetActive(false);
            curWindow = null;
        }
        else
        {
            menu.transform.position = Input.mousePosition;
        }
        menu.SetActive(true);
    }

    public void OpenCategoryMenu(GameObject g)
    {
        curWindow = g;
        menu.SetActive(false);
        curWindow.SetActive(true);
        curWindow.transform.position = menu.transform.position;
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
            CheckInventoryAfterRemove();
        }
        else Debug.Log("You don't have all items to craft it!");
    }

    public void CheckInventoryAfterRemove() // отличие в том, что не нужна проверка на новый рецепт
    {
        HashSet<ItemData> itemsInInventory = new HashSet<ItemData>();
        foreach (string id in Inventory.Instance.GetCurItems().Keys)
        {
            itemsInInventory.Add(ItemManager.Instance.GetItemByID(id));
        }
        foreach (ItemSpawnerButton b in spawnerButtons)
        {
            ItemData obj = b.objectToSpawn;
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
        foreach (ItemSpawnerButton b in spawnerButtons)
        {
            ItemData obj = b.objectToSpawn;
            if (obj.craftSet.Contains(added))
            {
                b.gameObject.SetActive(true);

                if (!exploredRecipes.Contains(obj))
                {
                    AddNewRecipe(obj, b);
                }

                if (obj.craftSet.IsSubsetOf(itemsInInventory))
                {
                    availableItems.Add(obj);
                }
                else
                {
                    availableItems.Remove(obj);
                }
            }
      
        }
    }

    void AddNewRecipe(ItemData item, ItemSpawnerButton b)
    {
        exploredRecipes.Add(item);
        b.GetComponent<Notificationable>().ShowNotification();
        MenuButton categoryButton = categoryButtons.Find(but => but.category == b.category);
        categoryButton.ShowNotification();
    }
}
