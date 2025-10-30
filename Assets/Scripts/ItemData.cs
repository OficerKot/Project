using UnityEngine;

[CreateAssetMenu(fileName = "New item", menuName = "Inventory/ItemData")]
public class ItemData : ScriptableObject
{
    public string itemId;
    public GameObject prefab;
    public GameObject UIprefab;
}
