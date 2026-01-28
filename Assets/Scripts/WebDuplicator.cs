using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.Text;

/// <summary>
/// Дубликатор паутины, который создает копии предмета на соседних клетках при наличии доступных домино.
/// </summary>
public class WebDuplicator : MonoBehaviour
{
    public Vector2 colliderSize;
    /// <summary>
    /// Интервал распространения паутины (в игровых часах)
    /// </summary>
    public float duplicateDelay; 
    [SerializeField] Collider2D[] colliders;

    /// <summary>
    /// Очищает все подписки на события.
    /// </summary>
    void CleanupSubscriptions()
    {
        StopAllCoroutines();
        foreach (Collider2D col in colliders)
        {
            if (col && col.GetComponent<Cell>() != null)
            {
                Cell cell = col.GetComponent<Cell>();
                cell.OnDuplicationAllowed -= StartCountDown;
            }
        }
    }

    void OnDestroy() => CleanupSubscriptions();
    void OnDisable() => CleanupSubscriptions();

    private void Start()
    {
        LayerMask cellsMask = LayerMask.GetMask("Cell");
        colliders = Physics2D.OverlapBoxAll(transform.position, colliderSize, cellsMask);
        CheckNeighbours();
        ListenNeighbours();
    }

    /// <summary>
    /// Подписывается на события разрешения дублирования в соседних клетках.
    /// </summary>
    void ListenNeighbours()
    {
        foreach (Collider2D col in colliders)
        {
            if (col.GetComponent<Cell>() != null)
            {
                Cell cell = col.GetComponent<Cell>();
                if (!IsCurCell(cell))
                {
                    cell.OnDuplicationAllowed += StartCountDown;
                }
            }
        }
    }

    /// <summary>
    /// Проверяет, находится ли дубликатор на клетке. Исключает повторное дублирование в одну и ту же клетку.
    /// </summary>
    bool IsCurCell(Cell cell)
    {
        return Vector2.Distance(transform.position, cell.transform.position) <= 0.1f;
    }

    /// <summary>
    /// Проверяет соседние клетки для запуска процесса дублирования.
    /// </summary>
    private void CheckNeighbours()
    {
        foreach (Collider2D col in colliders)
        {
            if (col.GetComponent<Cell>() != null)
            {
                Cell cell = col.GetComponent<Cell>();
                if (!IsCurCell(cell) && cell.GetCurDomino())
                {
                    StartCountDown(cell);
                }
            }
        }
    }

    /// <summary>
    /// Запускает отсчет времени до дублирования на указанной клетке.
    /// </summary>
    /// <param name="cell">Клетка для дублирования.</param>
    void StartCountDown(Cell cell)
    {
        StartCoroutine(CountDown(cell));
    }

    /// <summary>
    /// Корутина отсчета времени до дублирования.
    /// </summary>
    /// <param name="cell">Клетка для дублирования.</param>
    IEnumerator CountDown(Cell cell)
    {
        if (!cell.CheckDuplicationAllowed())
        {
            yield break;
        }
        else
        {
            Debug.Log("Started countdown");
        }
        int hoursPassed = 0;
        while (hoursPassed < duplicateDelay)
        {
            yield return StartCoroutine(WaitForNextHour());
            hoursPassed++;
        }
        if (cell.CheckDuplicationAllowed())
        {
            DuplicateOn(cell);
        }
    }

    /// <summary>
    /// Корутина ожидания следующего игрового часа.
    /// </summary>
    IEnumerator WaitForNextHour()
    {
        bool hourPassed = false;

        Action handler = () => hourPassed = true;
        Clock.OnHourPassed += handler;

        yield return new WaitUntil(() => hourPassed || this == null);
        if (handler != null)
        {
            Clock.OnHourPassed -= handler;
        }
    }

    /// <summary>
    /// Создает копию предмета на указанной клетке.
    /// </summary>
    /// <param name="targetCell">Целевая клетка для создания копии.</param>
    void DuplicateOn(Cell targetCell)
    {
        if (!targetCell.GetCurItem() && targetCell.GetCurDomino() != null)
        {
            ItemsPlacer.CreateItem(gameObject, (int)targetCell.transform.position.x, (int)targetCell.transform.position.y);
        }
    }

#if UNITY_EDITOR
    /// <summary>
    /// Отрисовывает зону действия дубликатора в редакторе Unity.
    /// </summary>
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, colliderSize);
    }
#endif
}