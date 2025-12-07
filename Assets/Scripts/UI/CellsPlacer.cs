using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellsPlacer : MonoBehaviour
{
    public GameObject prefab;
    public float midOffsetKoef;
    public List<GameObject> spawnedCells = new List<GameObject>();
    public BoxCollider2D windowCollider;

    [SerializeField] float defaultOffset;
    public Vector3 startPos;
    Vector3 curStartPos;
    float offsetX;
    float midX;

    [SerializeField] int maxDefaultElements = 9;
    int elementsCnt;

    //Чем больше количество элементов, тем меньше отступ
    //При этом если элементов <= maxDefaultElements, отступ будет не больше defaultOffset
    //

    private void Update()
    {
        if(DominoManager.Instance.available.Count != spawnedCells.Count)
        {
            ClearElements();
            PlaceElements();
        }
    }
    private void Start()
    {
        PlaceElements();
    }
    void CountOffset()
    {

        if (elementsCnt <= maxDefaultElements)
        {
            offsetX = defaultOffset;
        }
        else
        {
            float k = (float)maxDefaultElements / elementsCnt;
            offsetX = defaultOffset * k;
            Debug.Log("offset x is " + offsetX);
        }
    }

    void PlaceElements()
    {
        elementsCnt = DominoManager.Instance.available.Count;

        if (elementsCnt < maxDefaultElements)
        {
            float cellWidth = prefab.GetComponent<BoxCollider2D>().size.x * prefab.transform.lossyScale.x;
            midX = windowCollider.transform.localPosition.x;

            curStartPos = new Vector3(midX - cellWidth * (elementsCnt - 1) / 2, startPos.y, startPos.z);

            Debug.Log("cellWidth is " + cellWidth + " window mid is " + midX);
        }
        else
        {
            curStartPos = startPos;
        }

        CountOffset();
        for (int i = 0; i < elementsCnt; i++)
        {
            GameObject newObj = Instantiate(prefab, transform);
            newObj.transform.localPosition = curStartPos + new Vector3(offsetX * i, 0, 0);
            spawnedCells.Add(newObj);
        }
    }

    void ClearElements()
    {
        for(int i = 0; i < spawnedCells.Count; i++)
        {
            Destroy(spawnedCells[i]);
        }
        spawnedCells.Clear();
    }

}
