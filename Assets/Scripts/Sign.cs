using System.Collections;
using UnityEditor;
using UnityEngine;

public class Sign : MonoBehaviour
{
    enum Direction
    {
        Up, Down, Left, Right
    }

    [SerializeField] private Direction direction;
    private Vector3 VDirection;
    [SerializeField] private GameObject HolePrefab;
    private RaycastHit2D[] RayHitsArray;
    bool RayCrossed = false;
    float DestructionTime = 0.25f;

    enum RayhitsNature
    {
        Sign, DominoPart, Obstacle
    }

    private void Start()
    {
        SignsManager.Instance.PutInList(this);
        SpriteAnimator SprAnim = GetComponent<SpriteAnimator>();
        direction = (Direction)Random.Range(0, 4);

        switch (direction)
        {
            case Direction.Up:
                {
                    VDirection = new Vector3(0, 1, 0);
                    break;
                }
            case Direction.Down:
                {
                    VDirection = new Vector3(0, -1, 0);
                    break;
                }
            case Direction.Left:
                {
                    VDirection = new Vector3(-1, 0, 0);
                    break;
                }
            case Direction.Right:
                {
                    VDirection = new Vector3(1, 0, 0);
                    break;
                }

            default:
                break;
        }
        SprAnim.ForcePlay("Sign"+direction.ToString());

    }

    public void CastRay()
    {
        if (Physics2D.Raycast(transform.position, VDirection, 5f, LayerMask.GetMask("Player")))
        {
            RayCrossed = true;
            Debug.Log("Player found!");
            return;
        }
        else if (Physics2D.Raycast(transform.position, VDirection, 5f, LayerMask.GetMask("Default", "DominoPart")))
        {
            RayHitsArray = Physics2D.RaycastAll(transform.position, VDirection, 5f, LayerMask.GetMask("Default", "DominoPart"));
            Debug.Log($"Ray touched {RayHitsArray.Length} cell-objects");
            foreach (RaycastHit2D domino in RayHitsArray)
            {
                Debug.Log(domino.transform.gameObject);
            }
        }
        if (RayCrossed)
        {
            StartCoroutine(SpawnHoles());
            this.enabled = false;
        }
    }

    IEnumerator SpawnHoles()
    {
        int i = 0;
        foreach (RaycastHit2D rayhit in RayHitsArray)
        {
            if (rayhit)
            {
                RayhitsNature nature = CheckTheNature(rayhit);
                if (nature == RayhitsNature.DominoPart)
                {
                    rayhit.transform.parent.gameObject.GetComponent<Domino>().PickUp();
                    Destroy(rayhit.transform.parent.gameObject);
                }
                else if (nature == RayhitsNature.Obstacle && rayhit.transform.gameObject != this.gameObject)
                {
                    Cell curCell = rayhit.transform.gameObject.GetComponent<ObstacleSnap>().curCell;
                    curCell.SetFree();
                    Destroy(rayhit.transform.gameObject);
                }
                else continue;
            }
            Instantiate(HolePrefab, transform.position + (i + 1) * VDirection, Quaternion.identity);
            ++i;
            yield return new WaitForSeconds(DestructionTime);
        }
        while (i < 5)
        {
            Instantiate(HolePrefab, transform.position + (i + 1) * VDirection, Quaternion.identity);
            ++i;
            yield return new WaitForSeconds(DestructionTime);
        }
        StopAllCoroutines();
    }

    RayhitsNature CheckTheNature(RaycastHit2D rayhit) 
    {
        if (rayhit.transform.gameObject.layer == 7)
            return RayhitsNature.DominoPart;
        else if (rayhit.transform.gameObject.GetComponent<ObstacleSnap>() != null)
        {
            return RayhitsNature.Obstacle;
        }
        else 
            return RayhitsNature.Sign;
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Handles.DrawLine(transform.position, transform.position + 5*VDirection);
        GUIStyle style = new GUIStyle();
    }
#endif
}
