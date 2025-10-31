using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

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
            int indx = itemsID.FindIndex(itemm => itemm == i.GetID());
            ItemData item = ItemManager.Instance.GetItemByID(i.GetID());
            if (indx == -1)
            {
                itemsID.Add(item.itemId);
                indx = itemsID.FindIndex(itemm => itemm == item.itemId);
                UIInventory.Instance.AddNewItem(item, indx);
            }
            itemsCount[indx]++;
            UIInventory.Instance.AddOneMoreItem(indx);
        }
    }

    public void RemoveItem(Item i)
    {
        int indx = itemsID.FindIndex(item => item == i.GetID());
        if (indx != -1)
        {
            itemsCount[indx]--;
            UIInventory.Instance.RemoveOneItem(indx);

            if (itemsCount[indx] < 1)
            {
                UIInventory.Instance.RemoveItemIcon(indx);
                itemsID.RemoveAt(indx);
            }
      
        }
    }

    public bool isFull()
    {
        return itemsID.Count >= MAX_SIZE;
    }
}
