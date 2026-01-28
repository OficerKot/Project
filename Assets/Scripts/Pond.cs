using UnityEngine;

public class Pond : MonoBehaviour
{
    private ObstacleSnap ObsSnap;
    private Cell curCell;

    void Start()
    {
        ObsSnap = GetComponent<ObstacleSnap>();
        curCell = ObsSnap.curCell;
        curCell.SetFree();
        enabled = false;
    }
}
