using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New item", menuName = "Inventory/ItemData")]
public class ItemData : ScriptableObject
{
    public string Id;
    public GameObject prefab;
    public GameObject UIprefab;
    public ItemData[] itemsForCraft;

    public HashSet<ItemData> craftSet = new HashSet<ItemData>();
    private void OnEnable()
    {
        if (itemsForCraft.Length != 0)
        {
            foreach (ItemData item in itemsForCraft)
            {
                craftSet.Add(item);
            }
        }
    }

}
