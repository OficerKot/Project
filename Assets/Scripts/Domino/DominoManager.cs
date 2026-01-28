using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Хранит в себе информацию о каждой части домино и связывает id с их соответствующими префабами.
/// Осуществляет поиск по номеру, изображению или id.
/// </summary>
[CreateAssetMenu(fileName = "DominoManager", menuName = "Domino/DominoManager")]
public class DominoManager : ScriptableObject
{
    public List<DominoData> allDomino;
    public List<DominoData> available, basic;

    public Dictionary<ImageEnumerator, int> order = new Dictionary<ImageEnumerator, int>()
    {
        {ImageEnumerator.bone, 1} , {ImageEnumerator.fireflies, 2}, {ImageEnumerator.leaves, 3 }, {ImageEnumerator.flowers, 4 }, {ImageEnumerator.axe, 5}, {ImageEnumerator.pickaxe, 6}
    };

    private static DominoManager _instance;

    /// <summary>
    /// Singleton instance менеджера домино. Автоматически загружается из Resources.
    /// </summary>
    public static DominoManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<DominoManager>("DominoManager");

                if (_instance == null)
                {
                    _instance = CreateInstance<DominoManager>();
                    Debug.LogWarning("Created new DominoManager instance. Consider creating it as an asset.");
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
    /// Возвращает данные части домино по её идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор домино.</param>
    /// <returns>Данные домино или null, если не найдено.</returns>
    public DominoData GetDominoByID(string id)
    {
        return allDomino.Find(domino => domino.dominoId == id);
    }

    /// <summary>
    /// Возвращает случайную часть домино из доступных и базовых.
    /// </summary>
    /// <returns>Случайная часть домино.</returns>
    public DominoData GetRandomDomino()
    {
        List<DominoData> allAvailable = basic.ToList();
        allAvailable.AddRange(available);
        int indx = Random.Range(0, allAvailable.Count);
        return allAvailable[indx];
    }

    /// <summary>
    /// Находит информацию о части домино по изображению и числу (0 - любое число).
    /// </summary>
    /// <param name="image">Требуемое изображение.</param>
    /// <param name="number">Требуемое число (0 для любого).</param>
    /// <returns>Данные домино или null, если не найдено.</returns>
    public DominoData GetDomino(ImageEnumerator image, int number)
    {
        return allDomino.Find(d => d.image == image && (d.number == 0 || d.number == number));
    }

    /// <summary>
    /// Проверяет, есть ли доступные сигилы (кроме базовых).
    /// </summary>
    /// <returns>True если есть доступные сигилы.</returns>
    public bool HasAvailable()
    {
        return available.Count > 0;
    }
}

/// <summary>
/// Все сигилы, реализованные в игре.
/// </summary>
public enum ImageEnumerator
{
    any, bone, fireflies, leaves, flowers, axe, pickaxe
}