using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    const int MAX_SIZE = 18;
    [SerializeField] List<Item> items = new List<Item>();
    public static Inventory Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
     
        }
        else
        {
            Destroy(this);
        }
    }


    public void AddItem(Item i)
    {
        if(items.Count < MAX_SIZE)
        {
            items.Add(i);

            ItemData item = ItemManager.Instance.GetItemByID(i.GetID());
            UIInventory.Instance.AddItemIcon(item, items.FindIndex(item => item == i));
        }
    }

    public void RemoveItem(Item i)
    {
        if(items.Contains(i))
        {
            items.Remove(i);
        }
    }

    public bool isFull()
    {
        return items.Count >= MAX_SIZE;
    }
}
