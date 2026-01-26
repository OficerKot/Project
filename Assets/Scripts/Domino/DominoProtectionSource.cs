using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DominoProtectionSource : MonoBehaviour
{
    [SerializeField] float probability;
    public Vector2 protectAreaSize;
    List<Breakable> dominoInProtection = new List<Breakable>();

    private void OnDestroy()
    {
        EndProtecting();
    }

    public bool TryToProtect()
    {
        float randNum = Random.Range(0, probability);
        return (randNum <= probability);
    }
    public void StartProtecting()
    {
        Debug.Log("StartProtecting called");
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
