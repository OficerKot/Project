
using UnityEngine;

public class DominoBreaker : MonoBehaviour
{
    public float searchRadius = 20, minDistanceToHit = 5;
    [SerializeField] private LayerMask targetLayers;
    Domino dominoToHit;
    Collider2D[] colliders;
    public void BeatTheFurthestOnes()
    {
        colliders = Physics2D.OverlapCircleAll(transform.position, searchRadius, targetLayers);
        Debug.Log($"=== Поиск по слоям ===");
        Debug.Log($"Маска слоев: {targetLayers.value}");
        Debug.Log($"Найдено объектов: {colliders.Length}");
        if (colliders.Length > 0)
        {
            dominoToHit = GetRandDominoFromRadius();
            if (GetDist(dominoToHit.transform.position, transform.position) < minDistanceToHit)
            {
                TryToFindAny();
            }
            else
            {
                dominoToHit.TryToBreak(GetDist(dominoToHit.transform.position, transform.position));
            }
        }

    }

    Domino GetRandDominoFromRadius()
    {
        int indx = Random.Range(0, colliders.Length);
        return colliders[indx].GetComponent<Domino>();
    }
    float GetDist(Vector3 a, Vector3 b)
    {
        return Mathf.Sqrt(Mathf.Pow(a.x - b.x, 2) + Mathf.Pow(a.y - b.y, 2) + Mathf.Pow(a.z - b.z, 2));
    }

    void TryToFindAny()
    {
        foreach (Collider2D d in colliders)
        {
            dominoToHit = d.GetComponent<Domino>();
            if (GetDist(dominoToHit.transform.position, transform.position) >= minDistanceToHit)
            {
                dominoToHit.TryToBreak(GetDist(dominoToHit.transform.position, transform.position));
                return;
            }
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, searchRadius);
    }
#endif

}

