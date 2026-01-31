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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            CharacterStates statesComp = collision.GetComponent<CharacterStates>();
            statesComp.PutInPond();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            CharacterStates statesComp = collision.GetComponent<CharacterStates>();
            statesComp.DefaultAnim();
        }
    }
}
