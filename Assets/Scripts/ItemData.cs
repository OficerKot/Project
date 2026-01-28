using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// ScriptableObject содержащий данные о предмете для системы инвентаря и крафта.
/// </summary>
[CreateAssetMenu(fileName = "New item", menuName = "Inventory/ItemData")]
public class ItemData : ScriptableObject
{
    public string Id;
    public GameObject prefab;
    public GameObject UIprefab;

    /// <summary>
    /// Компоненты для крафта данного предмета
    /// </summary>
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