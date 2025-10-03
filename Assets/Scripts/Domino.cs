using Unity.VisualScripting;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using TreeEditor;

public class Domino : MonoBehaviour
{
    [SerializeField] private GameObject[] Parts;
    private GameObject part1Prefab, part2Prefab;
    private GameObject spawnedPart1, spawnedPart2;
    public GameObject pivot;

    [SerializeField] Button generateButton;
    [SerializeField] Button rotateButton;

    [SerializeField] private float offsetY = 2f;

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

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Debug.Log("3e3e3");
                CheckForFreeCells();
                Interact();
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                Rotate(90);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                Rotate(-90);
            }
        }

        else if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            CheckDominoClick();
        }
    }

    void CheckForFreeCells()
    {

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
        if (isBeingGrabbed)
        {
            PutDown();
        }
        else
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

    void PickUp()
    {
        Debug.Log("Pick up");
        isBeingGrabbed = true;
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
