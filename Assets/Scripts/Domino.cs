using TMPro;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class Domino : MonoBehaviour
{
    [SerializeField] GameObject[] Parts;
    GameObject part1Prefab, part2Prefab;
    GameObject spawnedPart1, spawnedPart2;
    public GameObject pivot;

    [SerializeField] Cell curCell1, curCell2;

    [SerializeField] float offsetY = 2f;

    [SerializeField] bool isBeingGrabbed = false;

    private void Start()
    {
        GenerateParts();
    }
    private void Update()
    {
        if (isBeingGrabbed)
        {
            Interact();
        }

        else if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            CheckDominoClick();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isBeingGrabbed && collision.gameObject.layer == 6)
        {
            if (!collision.GetComponent<Cell>().CheckIfFree()) return;

            ClearCellData();
            curCell1 = collision.GetComponent<Cell>();
            Cell minDistCell = FindMinDistCell();
           
            if (minDistCell == null)
            { 
                return;
            }
            else
            {
                curCell2 = minDistCell;
                curCell1.Highlight();
                curCell2.Highlight();
                return;
            }

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(isBeingGrabbed) ClearCellData();
    }
    void CheckDominoClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null)
        {
            if (hit.collider.transform.IsChildOf(transform))
            {
                PickUp();
            }
        }
    }
    Cell FindMinDistCell()
    {
        float minDist = float.MaxValue;
        Cell minDistCell = null;
        for (int i = 0; i < curCell1.GetNeighboursCount(); i++)
        {
            Cell nearCell = curCell1.neighbourCells[i];
            if (nearCell.GetComponent<Cell>().CheckIfFree() && IsSameRotationAngle(nearCell.transform.position, curCell1.transform.position))
            {
                if (minDist > GetDistance2(pivot.transform, nearCell.transform))
                {
                    minDist = GetDistance2(pivot.transform, nearCell.transform);
                    minDistCell = nearCell;
                }
            }
        }
        return minDistCell;
    }
    void ClearCellData()
    {
        if (curCell1)
        {
            curCell1.NoHighlight();
            curCell1.SetFree(true);
            curCell1 = null;
        }
        if (curCell2)
        {
            curCell2.NoHighlight();
            curCell2.SetFree(true);
            curCell2 = null;
        }

        spawnedPart1.GetComponent<DominoPart>().ChangeIsBeingPlacedFlag(false);
        spawnedPart2.GetComponent<DominoPart>().ChangeIsBeingPlacedFlag(false);
    }
    float GetDistance2(Transform pos1, Transform pos2)
    {
        float dist = Mathf.Pow(pos1.position.x - pos2.position.x, 2) + Mathf.Pow(pos1.position.y - pos2.position.y, 2);
        return dist;
    }
    void PutInTheCells()
    {
        isBeingGrabbed = false;
        TakeFreeSpace();

        Collider2D collider1 = curCell1.GetComponent<BoxCollider2D>();
        Collider2D collider2 = curCell2.GetComponent<BoxCollider2D>();

        TeleportToCells(collider1.transform, collider2.transform);
    }
    void TeleportToCells(Transform pos1, Transform pos2)
    {
        Vector2 targetPos = (pos1.position + pos2.transform.position) / 2f;
        pivot.transform.position = targetPos;
    }
    void TakeFreeSpace()
    {
        curCell1.SetFree(false);
        curCell2.SetFree(false);
        spawnedPart1.GetComponent<DominoPart>().ChangeIsBeingPlacedFlag(true);
        spawnedPart2.GetComponent<DominoPart>().ChangeIsBeingPlacedFlag(true);
    }
    bool IsSameRotationAngle(Vector3 pos1, Vector3 pos2)
    {
        bool cellsAreHorizontal = Mathf.Abs(pos1.x - pos2.x) > Mathf.Abs(pos1.y - pos2.y);
        float dominoAngle = NormalizeAngle(pivot.transform.eulerAngles.z);
        bool dominoIsHorizontal = (dominoAngle >= 45f && dominoAngle <= 135f);
        return cellsAreHorizontal == dominoIsHorizontal;
    }
    float NormalizeAngle(float angle)
    {
        // Приводим угол к диапазону 0-360
        angle %= 360f;
        if (angle < 0) angle += 360f;

        // Приводим к диапазону 0-180 
        if (angle > 180f) angle = 360f - angle;
        return angle;
    }
    void Interact()
    {
        Move();
        if (Input.GetKeyDown(KeyCode.A))
        {
            Rotate(90);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Rotate(-90);
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (curCell1 && curCell2)
            {
                curCell1.NoHighlight();
                curCell2.NoHighlight();
                PutInTheCells();
            }
        }
    }
    void Rotate(float degree = 90)
    {
        if (!spawnedPart1 || !spawnedPart2)
        {
            Debug.Log("Generate first");
            return;
        }
        pivot.transform.Rotate(0, 0, degree);

    }
    void Move()
    {
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetPos.z = 0;

        pivot.transform.position = Vector3.Lerp(pivot.transform.position, targetPos, 1);
    }
    void PickUp()
    {
        isBeingGrabbed = true;
        if (curCell1 && curCell2)
        {
            ClearCellData();
        }

    }
    void GenerateParts()
    {
        ChooseParts();
        SpawnParts();
        SpawnPivot();
    }
    void ChooseParts()
    {
        int indx1 = Random.Range(0, Parts.Length);
        int indx2 = Random.Range(0, Parts.Length);
        part1Prefab = Parts[indx1];
        part2Prefab = Parts[indx2];
    }
    void SpawnParts()
    {
        if (spawnedPart2 != null) Destroy(spawnedPart1);
        if (spawnedPart1 != null) Destroy(spawnedPart2);
        spawnedPart1 = Instantiate(part1Prefab, gameObject.transform.position + new Vector3(0, offsetY), gameObject.transform.rotation, transform);
        spawnedPart2 = Instantiate(part2Prefab, gameObject.transform.position, gameObject.transform.rotation, transform);
        spawnedPart1.GetComponent<DominoPart>().ChangeIsBeingPlacedFlag(false);
        spawnedPart2.GetComponent<DominoPart>().ChangeIsBeingPlacedFlag(false);

    }
    void SpawnPivot()
    {
        if (pivot != null) return;

        BoxCollider2D collider1 = spawnedPart1.GetComponent<BoxCollider2D>();
        BoxCollider2D collider2 = spawnedPart2.GetComponent<BoxCollider2D>();

        Vector2 centerPosition = (spawnedPart1.transform.position + spawnedPart2.transform.position) / 2f;

        pivot = new GameObject("Pivot");
        pivot.transform.position = centerPosition;
        pivot.transform.rotation = transform.rotation;

        transform.SetParent(pivot.transform);
    }

}
