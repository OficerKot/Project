using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Интерфейс для части домино с базовыми функциями.
/// </summary>
interface IDominoPart
{
    public void Property();
    public void ChangeIsBeingPlacedFlag(bool val);
    public bool IsBeingPlaced();
    public void ClearAllNeighbors();
}

/// <summary>
/// Расположение части домино относительно другой части. Необходимо для проверки доступных клеток при установке.
/// </summary>
public enum Location
{
    left, right, up, down
};

/// <summary>
/// Часть домино, управляющая связями с соседями, расположением и цикличностью дорог.
/// </summary>
public class DominoPart : MonoBehaviour, IDominoPart
{
    public DominoData data;
    int loopNumber = 0;

    [SerializeField] bool isBeingPlaced = false;

    [SerializeField] Location loc;

    [SerializeField] public List<DominoPart> neighbours = new List<DominoPart>();

    /// <summary>
    /// Виртуальный метод для особых свойств части домино.
    /// </summary>
    public virtual void Property()
    {
        //do nothing
    }

    /// <summary>
    /// Изменяет расположение части домино, относительно другой.
    /// </summary>
    /// <param name="l">Новое расположение.</param>
    public void ChangeLocation(Location l)
    {
        loc = l;
    }

    /// <summary>
    /// Возвращает текущее расположение части домино.
    /// </summary>
    public Location GetLocation()
    {
        return loc;
    }

    /// <summary>
    /// Возвращает номер цикла, в котором находится часть домино.
    /// </summary>
    public int GetLoopNumber()
    {
        return loopNumber;
    }

    /// <summary>
    /// Устанавливает номер цикла для части домино.
    /// </summary>
    /// <param name="n">Номер цикла.</param>
    public void SetLoopNumber(int n)
    {
        loopNumber = n;
    }

    /// <summary>
    /// Изменяет флаг размещения части домино.
    /// </summary>
    /// <param name="val">Новое значение флага.</param>
    public void ChangeIsBeingPlacedFlag(bool val)
    {
        isBeingPlaced = val;
    }

    /// <summary>
    /// Проверяет, размещена ли часть домино на поле.
    /// </summary>
    public bool IsBeingPlaced()
    {
        return isBeingPlaced;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == 7)
        {
            AddNeighbour(other.GetComponent<DominoPart>());
        }
    }

    /// <summary>
    /// Добавляет соседнюю часть домино при установке на поле.
    /// </summary>
    /// <param name="dominoPart">Соседняя часть домино.</param>
    void AddNeighbour(DominoPart dominoPart)
    {
        if (dominoPart.IsBeingPlaced() && isBeingPlaced && !IsAlreadyNeighbour(dominoPart) && NotAngular(dominoPart.transform))
        {
            neighbours.Add(dominoPart);
            RoadManager.Instance.CheckForLoop(this);
        }
    }

    /// <summary>
    /// Удаляет соседнюю часть домино.
    /// </summary>
    /// <param name="n">Соседняя часть для удаления.</param>
    public void RemoveNeighbor(DominoPart n)
    {
        neighbours.Remove(n);
    }

    /// <summary>
    /// Очищает все соседние связи.
    /// </summary>
    public void ClearAllNeighbors()
    {
        foreach (DominoPart n in neighbours)
        {
            if (n)
            {
                n.RemoveNeighbor(this);
                RoadManager.Instance.CheckForLoop(n);
            }
        }
        neighbours.Clear();
    }

    /// <summary>
    /// Проверяет, что объект не находится по диагонали.
    /// </summary>
    /// <param name="pos1">Трансформ для проверки.</param>
    bool NotAngular(Transform pos1)
    {
        return Mathf.Abs(pos1.position.x - transform.position.x) < 0.5f || Mathf.Abs(pos1.position.y - transform.position.y) < 0.5f;
    }

    /// <summary>
    /// Проверяет, является ли часть домино соседом.
    /// </summary>
    /// <param name="neighbour">Часть домино для проверки.</param>
    bool IsAlreadyNeighbour(DominoPart neighbour)
    {
        foreach (DominoPart n in neighbours)
        {
            if (n && n == neighbour) return true;
        }
        return false;
    }
}