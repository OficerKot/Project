using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Inventory : MonoBehaviour
{
    const int MAX_SIZE = 6;
    [SerializeField] List<string> itemsID = new List<string>();
    [SerializeField] int[] itemsCount = { 0, 0, 0, 0, 0, 0 };
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
            if (!itemsID.Contains(item.itemId))
            {
                itemsID.Add(item.itemId);
                UIInventory.Instance.AddItemIcon(item, itemsID.FindIndex(itemm => itemm == item.itemId));
            }
            itemsCount[itemsID.FindIndex(itemm => itemm == item.itemId)]++; 
        }
    }

    public void RemoveItem(Item i)
    {
        int indx = itemsID.FindIndex(item => item == i.GetID());
        if (indx != -1)
        {
            itemsCount[indx]--;
            if (itemsCount[indx] < 1)
            {
                UIInventory.Instance.RemoveItemIcon(indx);
                itemsID.RemoveAt(indx);
                itemsCount[indx] = 0;
            }
      
        }
    }

    public bool isFull()
    {
        return itemsID.Count >= MAX_SIZE;
    }
}
