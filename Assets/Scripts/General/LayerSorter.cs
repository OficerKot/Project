using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Порядок сортировки слоёв по умолчанию.
/// </summary>
public enum SortingOrder
{
    domino = 4,
    item = 4,
    pond = 5,
    player = 6,
    web = 10
}

/// <summary>
/// Предназначен для смены порядка слоёв в процессе игры.
/// </summary>
public class LayerSorter : MonoBehaviour
{
    public static LayerSorter Instance;
    int topLayer = 10;
    /// <summary>
    /// Содержит объекты, перемещённые на передний план в порядке их добавления.
    /// </summary>
    List<GameObject> objectsOnTop = new List<GameObject>();

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
    public void PutInFront(GameObject obj)
    {
        SortingGroup sortingGroup = obj.GetComponent<SortingGroup>();
        objectsOnTop.Add(obj);
        sortingGroup.sortingOrder = topLayer;
        topLayer++;
    }

    public void PutBack(GameObject obj, SortingOrder ord)
    {
        if (objectsOnTop.Contains(obj))
        {
            obj.GetComponent<SortingGroup>().sortingOrder = (int)ord;
            if (objectsOnTop[objectsOnTop.Count-1] == obj)
            {
                topLayer--;
            }
            objectsOnTop.Remove(obj);
        }
    }
}
