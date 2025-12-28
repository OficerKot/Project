using UnityEngine;

public class DominoBreaker : MonoBehaviour
{
    public float maxDistanceToHit = 20, minDistanceToHit = 5;
    public int maxDominoToBreak = 5;
    public float damage = 10;
    [SerializeField] private LayerMask targetLayers;
    Collider2D[] searchRadius;
    bool canHit = true;

    private void Start()
    {
        DominoProtecter.OnProtectionStarted += OnProtectionStarted;
    }

    void OnProtectionStarted(bool b)
    {
        canHit = !b;
    }
    public void GetColliders()
    {
        searchRadius = Physics2D.OverlapCircleAll(transform.position, maxDistanceToHit, targetLayers);    
    }

    float CountProbability(float dist)
    {
        if(dist > maxDistanceToHit)
        {
            return 1;
        }
        if (dist < minDistanceToHit)
        {
            return 0;
        }
        else return dist / maxDistanceToHit;
    }

    public void HitSomeDomino()
    {
        if (!canHit)
        {
            Debug.Log("Can't hit");
            return;
        }

        GetColliders();
        foreach(Collider2D domino in searchRadius)
        {
            Transform pos = domino.GetComponent<Transform>();
            float probability = CountProbability(GetDist(pos.position, transform.position));
            Debug.Log("For " + domino.name + " probability is: " + probability);
            float randFloat = Random.Range(0, 1f);
            if(probability >= randFloat)
            {
                domino.GetComponent<Domino>().TryToBreak(damage);
            }
        }

    }
    float GetDist(Vector3 a, Vector3 b)
    {
        return Mathf.Sqrt(Mathf.Pow(a.x - b.x, 2) + Mathf.Pow(a.y - b.y, 2) + Mathf.Pow(a.z - b.z, 2));
    }


#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxDistanceToHit);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, minDistanceToHit);

    }
#endif
}

