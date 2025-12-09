using UnityEngine;

[CreateAssetMenu(fileName = "ItemManager", menuName = "Inventory/ItemManager")]
public class ItemManager : ScriptableObject
{
    public ItemData[] allItems;

    private static ItemManager _instance;
    public static ItemManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<ItemManager>("ItemManager");

                if (_instance == null)
                {
                    _instance = CreateInstance<ItemManager>();
                    Debug.LogWarning("Created new ItemManager instance. Consider creating it as an asset.");
                }
            }
            return _instance;
        }
    }

    private void OnEnable()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }
    public ItemData GetItemByID(string id)
    {
        return System.Array.Find(allItems, item => item.Id == id);
    }

}
