using UnityEngine;

public class Pond : MonoBehaviour
{
    private ObstacleSnap ObsSnap;

    void Awake()
    {
        ObsSnap = GetComponent<ObstacleSnap>();
        //ObsSnap.curCell.SetFree();
    }
    
}
