using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Инвентарь игрока.
/// Хранит предметы по ID и их количество,
/// ограничен максимальным размером и автоматически обновляет UI.
/// </summary>
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

        }
        else
        {
            Destroy(this);
        }
    }

    /// <summary>
    /// Добавление предмета в ивентарь
    /// </summary>
    /// <param name="i">Данные добавляемого предмета.</param>
    public void AddItem(ItemData i)
    {
        if (Contains(i) || itemsID.Count < MAX_SIZE)
        {
            AudioManager.Play(SoundType.Pickup);
            if (!itemsID.ContainsKey(i.Id))
            {
                itemsID.Add(i.Id, 0);
                UIInventory.Instance.AddNewItem(i);
            }
            itemsID[i.Id]++;
            UIInventory.Instance.AddOneMoreItem(i);
            UICraftWindow.Instance.CheckInventory(i);
        }
        else
        {
            AudioManager.Play(SoundType.FullInventory);
        }
    }
    /// <summary>
    /// Добавление нескольких предметов в инвентарь
    /// </summary>
    /// <param name="i">Данные предмета</param>
    /// <param name="count">Количество</param>
    public void AddItem(ItemData i, int count)
    {
        if (Contains(i) || itemsID.Count < MAX_SIZE)
        {
            AudioManager.Play(SoundType.Pickup);
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

    /// <summary>
    /// Удаление предмета из инвентаря
    /// </summary>
    /// <param name="i">Данные предмета</param>
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

    /// <summary>
    /// Проверка на наличие предмета в инвентаре
    /// </summary>
    /// <param name="i">Данные предмета</param>
    /// <returns></returns>
    public bool Contains(ItemData i)
    {
        return itemsID.ContainsKey(i.Id);
    }

    /// <summary>
    /// Возвращает текущие предметы инвентаря.
    /// </summary>
    /// <returns>Словарь ID предметов и их количества.</returns>
    public Dictionary<string, int> GetCurItems()
    {
        return itemsID;
    }

    /// <summary>
    /// Проверяет наличие свободного места в инвентаре
    /// </summary>
    /// <returns></returns>
    public bool IsFull()
    {
        if (itemsID.Count >= MAX_SIZE) AudioManager.Play(SoundType.FullInventory);
        return itemsID.Count >= MAX_SIZE;
    }
}
