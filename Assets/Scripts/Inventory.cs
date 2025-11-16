using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour
{
    public const int MAX_SIZE = 6;
    [SerializeField] Dictionary<string, int> itemsID = new Dictionary<string, int>();
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
        if(itemsID.Count < MAX_SIZE)
        {
            ItemData item = ItemManager.Instance.GetItemByID(i.GetID());
            if (!itemsID.ContainsKey(i.GetID()))
            {
                itemsID.Add(item.itemId, 0);
                UIInventory.Instance.AddNewItem(item);
            }
            itemsID[i.GetID()]++;
            UIInventory.Instance.AddOneMoreItem(item);
        }
    }

    public void RemoveItem(Item i)
    {
        ItemData item = ItemManager.Instance.GetItemByID(i.GetID());
        if (itemsID.ContainsKey(i.GetID()))
        {
            itemsID[i.GetID()]--;
            UIInventory.Instance.RemoveOneItem(item);

            if (itemsID[i.GetID()]< 1)
            {
                UIInventory.Instance.RemoveItemIcon(item);
                itemsID.Remove(i.GetID());
            }
      
        }
    }

    public bool isFull()
    {
        return itemsID.Count >= MAX_SIZE;
    }
}
