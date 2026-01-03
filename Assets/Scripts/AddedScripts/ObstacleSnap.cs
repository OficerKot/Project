using UnityEngine;

public class ObstacleSnap : MonoBehaviour
{
    [SerializeField] public Cell curCell;
    [SerializeField] Interactable interactableInterface;
    [SerializeField] public bool spawned = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 6)
        { 
            curCell = other.GetComponent<Cell>();
            interactableInterface = GetComponent<Interactable>();
            if (interactableInterface != null)
            {
                if (spawned == true)
                {
                    interactableInterface.PutInCell(curCell);
                }
            }
            else
            {
                curCell.SetFree(false);
            }
        }
    }
}
