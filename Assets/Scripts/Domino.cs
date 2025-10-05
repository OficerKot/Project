using TMPro;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Domino : MonoBehaviour
{
    [SerializeField] private GameObject[] Parts;
    private GameObject part1Prefab, part2Prefab;
    private GameObject spawnedPart1, spawnedPart2;
    [SerializeField] private Cell curCell1, curCell2;
    public GameObject pivot;


    [SerializeField] Button generateButton;


    [SerializeField] private float offsetY = 2f;
    [SerializeField] private float cellsDetectionRadius = 2f;

    [SerializeField] private bool isBeingGrabbed = false;

    private void Start()
    {
        generateButton.onClick.AddListener(Generate);
    }

    private void Update()
    {
        if (isBeingGrabbed)
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
                    TeleportToCells();
                }
            }
        }

        else if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            CheckDominoClick();
        }
    }

    void CheckDominoClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null)
        {
            if (hit.collider.transform.IsChildOf(transform))
            {
                Interact();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isBeingGrabbed && collision.gameObject.layer == 6)
        {
            if (!collision.GetComponent<Cell>().CheckIfFree()) return;

            float minDist = float.MaxValue;
            Cell minDistCell = null;

            ClearCellData();

            curCell1 = collision.GetComponent<Cell>();
            for(int i = 0; i < curCell1.GetNeighboursCount(); i++ )
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
            if (minDistCell == null || minDist == float.MaxValue)
            {
                Debug.Log("Нет подходящих клеток");
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
    }
    float GetDistance2(Transform pos1, Transform pos2)
    {
        float dist = Mathf.Pow(pos1.position.x - pos2.position.x, 2) + Mathf.Pow(pos1.position.y - pos2.position.y, 2);
        return dist;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (curCell1 && curCell2)
        {
            curCell1.NoHighlight();
            curCell2.NoHighlight();
            curCell1.SetFree(true);
            curCell2.SetFree(true);

        }
        curCell1 = null;
        curCell2 = null;
    }
    //void CheckForFreeCells()
    //{
    //    Collider2D[] nearCells = Physics2D.OverlapCircleAll(pivot.transform.position, cellsDetectionRadius);
    //    GameObject[] cells = new GameObject[5];
    //    int size = 0;
    //    foreach (Collider2D collider in nearCells)
    //    {
    //        if(collider.gameObject.layer == 6 && collider.gameObject.GetComponent<Cell>().CheckIfFree())
    //        {
    //            cells[size++] = collider.gameObject;
    //        }
    //    }
    //    if (size == 0) return;

    //    GameObject[] goodCells = FindTwoNeighbours(cells, size);
    //    if(goodCells.Length != 0)
    //    {
    //        Debug.Log(goodCells[0] + " " + goodCells[1]);
    //        TeleportToCells(goodCells[0], goodCells[1]);
    //    }

    //}
    //GameObject[] FindTwoNeighbours(GameObject[] cells, int size)
    //{
    //    for (int i = 0; i < size; i++)
    //    {
    //        for (int j = i + 1; j < size; j++)
    //        {
    //            Cell cell1 = cells[i].GetComponent<Cell>();
    //            BoxCollider2D col1= cells[i].GetComponent<BoxCollider2D>();
    //            BoxCollider2D col2 = cells[j].GetComponent<BoxCollider2D>();

    //            if (cell1.IsNeighbour(cells[j]) && IsSameRotationAngle(col1.transform.position, col2.transform.position))
    //            {
    //                return new GameObject[2] { cells[i], cells[j] };
    //            }
    //            else Debug.Log(cells[i] + " and " + cells[j] + " are not good");
    //        }
    //    }
    //    return new GameObject[0] { };
    //}
    void TeleportToCells()
    {
        isBeingGrabbed = false;

        curCell1.SetFree(false);
        curCell2.SetFree(false);
        Collider2D collider1 = curCell1.GetComponent<BoxCollider2D>();
        Collider2D collider2 = curCell2.GetComponent<BoxCollider2D>();


        Vector2 targetPos = (collider1.transform.position + collider2.transform.position) / 2f;
        pivot.transform.position = targetPos;

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
    void Rotate(float degree = 90)
    {
        if (!spawnedPart1 || !spawnedPart2)
        {
            Debug.Log("Generate first");
            return;
        }
        pivot.transform.Rotate(0, 0, degree);

    }

    void Interact()
    {
        if (!isBeingGrabbed)
        {
            PickUp();
        }
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
            Cell cell1 = curCell1.GetComponent<Cell>();
            Cell cell2 = curCell2.GetComponent<Cell>();

            cell1.SetFree(true);
            cell2.SetFree(true);

            curCell1 = null;
            curCell2 = null;
        }
    }

    void PutDown()
    {
        Debug.Log("Put down");
        isBeingGrabbed = false;
    }
    void Generate()
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
