using UnityEngine;

/// <summary>
/// Менеджер предметов, предоставляющий доступ ко всем игровым предметам через ScriptableObject.
/// </summary>
[CreateAssetMenu(fileName = "ItemManager", menuName = "Inventory/ItemManager")]
public class ItemManager : ScriptableObject
{
    public ItemData[] allItems;

    private static ItemManager _instance;

    /// <summary>
    /// Singleton instance менеджера предметов. Автоматически загружается из Resources.
    /// </summary>
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

    /// <summary>
    /// Возвращает данные предмета по его идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор предмета.</param>
    /// <returns>Данные предмета или null, если предмет не найден.</returns>
    public ItemData GetItemByID(string id)
    {
        return System.Array.Find(allItems, item => item.Id == id);
    }
}