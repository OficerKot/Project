using UnityEngine;

public class ObstacleSnap : MonoBehaviour
{
    [SerializeField] public Cell curCell;
    [SerializeField] Item this_item;
    [SerializeField] public bool spawned = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 6)
        { 
            curCell = other.GetComponent<Cell>();
            this_item = this.GetComponent<Item>();
            if (this_item != null)
            {
                if (spawned == true)
                {
                    this_item.PutInCellOnSpawn(curCell);
                }
            }
            else
            {
                curCell.SetFree(false);
            }
        }
    }
}
