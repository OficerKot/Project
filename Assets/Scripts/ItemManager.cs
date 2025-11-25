using UnityEngine;

[CreateAssetMenu(fileName = "ItemManager", menuName = "Inventory/ItemManager")]
public class ItemManager : ScriptableObject
{
    public ItemData[] allItems;

    public static ItemManager Instance;
 

    private void OnEnable()
    {
        Instance = this;
    }
    public ItemData GetItemByID(string id)
    {
        return System.Array.Find(allItems, item => item.Id == id);
    }

}
