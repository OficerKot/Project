using System.Collections.Generic;
using UnityEngine;


public class Inventory : MonoBehaviour
{
    public const int MAX_SIZE = 6;
    [SerializeField] Dictionary<string, int> itemsID = new Dictionary<string, int>();
    public static Inventory Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);

        }
        else
        {
            Destroy(this);
        }
    }

    public void AddItem(ItemData i)
    {
        if (itemsID.Count < MAX_SIZE)
        {
            if (!itemsID.ContainsKey(i.Id))
            {
                itemsID.Add(i.Id, 0);
                UIInventory.Instance.AddNewItem(i);
            }
            itemsID[i.Id]++;
            UIInventory.Instance.AddOneMoreItem(i);
            UICraftWindow.Instance.CheckInventory(i);
        }
    }
    public void AddItem(ItemData i, int count)
    {
        if (itemsID.Count < MAX_SIZE)
        {
            if (!itemsID.ContainsKey(i.Id))
            {
                itemsID.Add(i.Id, 0);
                UIInventory.Instance.AddNewItem(i);
            }
            for (int j = 0; j < count; j++)
            {
                itemsID[i.Id]++;
                UIInventory.Instance.AddOneMoreItem(i);
            }
            UICraftWindow.Instance.CheckInventory(i);
        }
    }

    public void RemoveItem(ItemData i)
    {
        if (itemsID.ContainsKey(i.Id))
        {
            itemsID[i.Id]--;
            UIInventory.Instance.RemoveOneItem(i);

            if (itemsID[i.Id] < 1)
            {
                UIInventory.Instance.RemoveItemIcon(i);
                itemsID.Remove(i.Id);
            }
            UICraftWindow.Instance.CheckInventory(i);
        }

    }

    public bool Contains(ItemData i)
    {
        return itemsID.ContainsKey(i.Id);
    }
    public Dictionary<string, int> GetCurItems()
    {
        return itemsID;
    }
    public bool isFull()
    {
        return itemsID.Count >= MAX_SIZE;
    }
}
