using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject DominoPrefab;

    public void SpawnDomino()
    {
        Instantiate(DominoPrefab);
    }
}
