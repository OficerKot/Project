using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Источник защиты домино.
/// С определённой вероятностью применяет защиту ко всем разрушаемым домино в области действия.
/// </summary>
public class DominoProtectionSource : MonoBehaviour
{
    /// <summary>
    /// Вероятность защиты
    /// </summary>
    [SerializeField] float probability;
    /// <summary>
    /// Область защиты
    /// </summary>
    public Vector2 protectAreaSize;
    /// <summary>
    /// Домино, находящиеся под защитой
    /// </summary>
    List<Breakable> dominoInProtection = new List<Breakable>();

    private void OnDestroy()
    {
        EndProtecting();
    }

    /// <summary>
    /// Попытка применить защиту с учётом вероятности.
    /// </summary>
    /// <returns>
    /// True — защита сработала,  
    /// False — защита не сработала.
    /// </returns>
    public bool TryToProtect()
    {
        float randNum = Random.Range(0, 1);
        return (randNum <= probability);
    }

    /// <summary>
    /// Начало защиты всех домино в области действия.
    /// </summary>
    public void StartProtecting()
    {
        var hits = Physics2D.OverlapBoxAll(transform.position, protectAreaSize, 0, LayerMask.GetMask("DominoBase"));
        if (hits.Length > 0)
        {
            foreach (var domino in hits)
            {
                Breakable breakableComp = domino.GetComponent<Breakable>();
                breakableComp.AddProtection(this);
                Debug.Log("Added protection");
                dominoInProtection.Add(breakableComp);
            }
        }
    }

    /// <summary>
    /// Окончание защиты всех домино в области действия.
    /// </summary>
    public void EndProtecting()
    {
        foreach (var domino in dominoInProtection)
        {
            domino.RemoveProtection(this);
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, protectAreaSize);
    }
#endif
}
