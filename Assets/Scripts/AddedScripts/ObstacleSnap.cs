using UnityEngine;

public class ObstacleSnap : MonoBehaviour
{
    [SerializeField] Cell curCell;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 6)
        {
            curCell = other.GetComponent<Cell>();
            curCell.SetFree(false);
        }
    }
}
