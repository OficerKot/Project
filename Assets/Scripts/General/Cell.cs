using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Интерфейс для клетки игрового поля.
/// </summary>
public interface ICell
{
    public void Highlight();
    public void NoHighlight();
    public bool IsNeighbour(Cell obj);
    public bool IsFreeForDomino();
    public DominoPart GetCurDomino();
    public void SetFree(bool val);
    public void SetFree();
}

/// <summary>
/// Интерфейс для разрушаемых объектов на поле.
/// </summary>
public interface IBreakableObject
{
    public bool CanBreak(DominoPart cur, DominoPart other);
    public void Break();
}

/// <summary>
/// Класс клетки игрового поля, управляющей размещением домино и объектов.
/// </summary>
public class Cell : MonoBehaviour, ICell
{
    [SerializeField] bool isFreeForDomino = true;
    SpriteRenderer cellSprite;
    Color previousColor;
    [SerializeField] DominoPart curDomino;
    [SerializeField] GameObject curItem;
    [SerializeField] ImageEnumerator image = ImageEnumerator.any;
    [SerializeField] int number = 0;
    [SerializeField] public List<Cell> neighbourCells = new List<Cell>();
    public event Action<Cell> OnDuplicationAllowed;
    public int duplicationBlockers = 0;

    void Start()
    {
        CheckForDuplications();
        curDomino = null;
        number = 0;
        cellSprite = gameObject.GetComponent<SpriteRenderer>();
        previousColor = cellSprite.color;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 6 && NotAngular(other.transform))
        {
            neighbourCells.Add(other.GetComponent<Cell>());
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (curDomino)
        {
            SetImageToAllNeighbours();
        }
    }

    /// <summary>
    /// Возвращает текущее домино в клетке.
    /// </summary>
    public DominoPart GetCurDomino()
    {
        return curDomino;
    }

    /// <summary>
    /// Возвращает текущий предмет в клетке.
    /// </summary>
    public GameObject GetCurItem()
    {
        return curItem;
    }

    /// <summary>
    /// Устанавливает домино в клетку.
    /// </summary>
    /// <param name="domino">Домино для размещения.</param>
    public void SetCurDomino(DominoPart domino)
    {
        curDomino = domino;
        image = domino.data.image;
        number = domino.data.number;
        CheckForDuplications();
    }

    /// <summary>
    /// Устанавливает предмет в клетку.
    /// </summary>
    /// <param name="i">Предмет для размещения.</param>
    public void SetCurItem(GameObject i)
    {
        CheckForDuplications();
        curItem = i;
        if (i == null)
        {
            UnsetImageToAllNeighbours();
            image = ImageEnumerator.any;
            number = 0;
            return;
        }
    }

    /// <summary>
    /// Освобождает клетку, удаляя домино и предмет.
    /// </summary>
    public void SetFree()
    {
        isFreeForDomino = true;
        curDomino = null;
        curItem = null;
        UnsetImageToAllNeighbours();
        image = ImageEnumerator.any;
        number = 0;
        CheckForDuplications();
    }

    /// <summary>
    /// Проверяет, что объект не находится по диагонали.
    /// </summary>
    bool NotAngular(Transform pos1)
    {
        return Mathf.Abs(pos1.position.x - transform.position.x) < 0.5f || Mathf.Abs(pos1.position.y - transform.position.y) < 0.5f;
    }

    /// <summary>
    /// Устанавливает изображение сигила всем соседним клеткам.
    /// </summary>
    void SetImageToAllNeighbours()
    {
        foreach (Cell neighbour in neighbourCells)
        {
            if (neighbour && neighbour.GetImage() == ImageEnumerator.any)
            {
                neighbour.SetImage(curDomino.data.neighboursImage);
                neighbour.SetNumber(number);
            }
        }
    }

    /// <summary>
    /// Сбрасывает изображение сигила со всех соседних клеток.
    /// </summary>
    void UnsetImageToAllNeighbours()
    {
        foreach (Cell neighbour in neighbourCells)
        {
            if (neighbour && neighbour.GetImage() == image && !neighbour.GetCurDomino())
            {
                neighbour.SetImage(ImageEnumerator.any);
                neighbour.SetNumber(0);
            }
        }
    }

    /// <summary>
    /// Устанавливает изображение сигила для клетки.
    /// </summary>
    public void SetImage(ImageEnumerator i)
    {
        image = i;
    }

    /// <summary>
    /// Возвращает текущее изображение сигила для клетки.
    /// </summary>
    public ImageEnumerator GetImage()
    {
        return image;
    }

    /// <summary>
    /// Возвращает текущее число клетки.
    /// </summary>
    public int GetNumber()
    {
        return number;
    }

    /// <summary>
    /// Устанавливает число клетки.
    /// </summary>
    public void SetNumber(int n)
    {
        number = n;
    }

    /// <summary>
    /// Подсвечивает клетку.
    /// </summary>
    public void Highlight()
    {
        cellSprite.color = Color.aliceBlue;
    }

    /// <summary>
    /// Снимает подсветку с клетки.
    /// </summary>
    public void NoHighlight()
    {
        if (cellSprite)
        {
            cellSprite.color = previousColor;
        }
    }

    
    
    /// <summary>
    /// Проверяет, является ли текущая клетка соседней с проверяемой.
    /// </summary>
    /// <param name="obj">Клетка для проверки</param>
    /// <returns></returns>
    public bool IsNeighbour(Cell obj)
    {
        foreach (Cell cell in neighbourCells)
        {
            if (cell && cell == obj) return true;
        }
        return false;
    }

    /// <summary>
    /// Проверяет, свободна ли клетка для размещения домино.
    /// </summary>
    public bool IsFreeForDomino()
    {
        return isFreeForDomino;
    }

    /// <summary>
    /// Проверяет, полностью ли свободна клетка.
    /// </summary>
    public bool IsFree()
    {
        return curDomino == null && curItem == null;
    }

    /// <summary>
    /// Устанавливает статус доступности клетки для домино.
    /// </summary>
    public void SetFree(bool val)
    {
        isFreeForDomino = val;
    }

    /// <summary>
    /// Проверяет, разрешено ли дублирование объектов в этой клетке.
    /// </summary>
    public bool CheckDuplicationAllowed()
    {
        if (curItem == null && curDomino != null && duplicationBlockers == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Проверяет возможность дублирования и вызывает событие при разрешении.
    /// </summary>
    public void CheckForDuplications()
    {
        if (CheckDuplicationAllowed()) OnDuplicationAllowed?.Invoke(this);
    }

    /// <summary>
    /// Добавляет блокировщик дублирования.
    /// </summary>
    public void AddDuplicationBlocker()
    {
        duplicationBlockers++;
        CheckForDuplications();
    }

    /// <summary>
    /// Удаляет блокировщик дублирования.
    /// </summary>
    public void RemoveDuplicationBlocker()
    {
        duplicationBlockers = Mathf.Max(0, duplicationBlockers - 1);
        CheckForDuplications();
    }
}