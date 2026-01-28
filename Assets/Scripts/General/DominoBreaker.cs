using UnityEngine;

/// <summary>
/// Компонент для разрушения домино в радиусе с вероятностной системой попадания.
/// </summary>
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

    /// <summary>
    /// Обработчик события начала/окончания защиты домино.
    /// </summary>
    /// <param name="b">Флаг активности защиты.</param>
    void OnProtectionStarted(bool b)
    {
        canHit = !b;
    }

    /// <summary>
    /// Находит все коллайдеры домино в радиусе поражения.
    /// </summary>
    public void GetColliders()
    {
        searchRadius = Physics2D.OverlapCircleAll(transform.position, maxDistanceToHit, targetLayers);
    }

    /// <summary>
    /// Рассчитывает вероятность нанесения урона домино на основе расстояния.
    /// </summary>
    /// <param name="dist">Расстояние от игрока</param>
    /// <returns>Вероятность попадания от 0 до 1.</returns>
    float CountProbability(float dist)
    {
        if (dist > maxDistanceToHit)
        {
            return 1;
        }
        if (dist < minDistanceToHit)
        {
            return 0;
        }
        else return dist / maxDistanceToHit;
    }

    /// <summary>
    /// Попытка нанести урон случайным домино в радиусе с учетом вероятности.
    /// </summary>
    public void HitSomeDomino()
    {
        if (!canHit)
        {
            Debug.Log("Can't hit");
            return;
        }

        GetColliders();
        foreach (Collider2D domino in searchRadius)
        {
            Transform pos = domino.GetComponent<Transform>();
            float probability = CountProbability(GetDist(pos.position, transform.position));
            Debug.Log("For " + domino.name + " probability is: " + probability);
            float randFloat = Random.Range(0, 1f);
            if (probability >= randFloat)
            {
                domino.GetComponent<Breakable>().TryToBreak(damage);
            }
        }
    }

    /// <summary>
    /// Вычисляет расстояние между двумя точками
    /// </summary>
    float GetDist(Vector3 a, Vector3 b)
    {
        return Mathf.Sqrt(Mathf.Pow(a.x - b.x, 2) + Mathf.Pow(a.y - b.y, 2) + Mathf.Pow(a.z - b.z, 2));
    }

#if UNITY_EDITOR
    /// <summary>
    /// Отрисовка радиусов поражения в редакторе Unity.
    /// </summary>
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxDistanceToHit);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, minDistanceToHit);
    }
#endif
}