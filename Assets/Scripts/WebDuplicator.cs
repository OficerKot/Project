using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor;
using UnityEngine;

public class WebDuplicator : MonoBehaviour
{
    public Vector2 colliderSize;
    public float duplicateDelay; // in hours
    [SerializeField] Collider2D[] colliders;
    private void OnDisable()
    {
        foreach (Collider2D col in colliders)
        {
            if (col && col.GetComponent<Cell>() != null)
            {
                col.GetComponent<Cell>().OnDominoPlaced -= StartCountDown;
            }
        }
    }
    void OnEnable()
    {
        Start();
    }

    private void Start()
    {
        LayerMask cellsMask = LayerMask.GetMask("Cell");
        colliders = Physics2D.OverlapBoxAll(transform.position, colliderSize, cellsMask);
        ListenNeighbours();
    }
    void ListenNeighbours()
    {
        foreach (Collider2D col in colliders)
        {
            if (col.GetComponent<Cell>() != null)
            {
                Cell cell = col.GetComponent<Cell>();
                cell.OnDominoPlaced += StartCountDown;
                if (cell.GetCurDomino())
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
        int hoursPassed = 0;
        while (hoursPassed < duplicateDelay)
        {
            yield return StartCoroutine(WaitForNextHour());
            hoursPassed++;
        }
        if(cell.GetCurDomino())
        {
            DuplicateOn(cell);
        }
    }
    IEnumerator WaitForNextHour()
    {
        bool hourPassed = false;

        Action handler = () => hourPassed = true;
        Clock.OnHourPassed += handler;

        yield return new WaitUntil(() => hourPassed);

        Clock.OnHourPassed -= handler;
    }

    void DuplicateOn(Cell targetCell)
    {
        if (targetCell && targetCell.GetCurItem() == null && targetCell.GetCurDomino() != null)
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
