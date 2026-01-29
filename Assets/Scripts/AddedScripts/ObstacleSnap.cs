using UnityEngine;

/// <summary>
/// Скрипт, фиксирующий создаваемые на старте предметы/препятствия в клетках.
/// При появлении предмета/препятствия в мире, скрипт проверяет, чем является появившийся объект. 
/// Если предмет - кладёт его в клетку методом PutInCell() класса interactableInterface, если препятствие - занимает клетку методом SetFree(false) класса curCell
/// 
/// Дополнительная настройка: не требуется
/// </summary>
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
