using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class WebDuplicator : MonoBehaviour
{
    public Vector2 colliderSize;
    public float duplicateDelay; // in hours
    [SerializeField] Collider2D[] colliders;

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

    bool IsCurCell(Cell cell)
    {
        return Vector2.Distance(transform.position, cell.transform.position) <= 0.1f;
    }
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
    void StartCountDown(Cell cell)
    {
        StartCoroutine(CountDown(cell));
    }
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

    void DuplicateOn(Cell targetCell)
    {
        if (!targetCell.GetCurItem() && targetCell.GetCurDomino() != null)
        {
            ItemsPlacer.CreateItem(gameObject, (int)targetCell.transform.position.x, (int)targetCell.transform.position.y);
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, colliderSize);
    }
#endif
}
