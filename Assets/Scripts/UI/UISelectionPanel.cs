using System.Collections.Generic;
using UnityEngine;

public class UISelectionPanel : MonoBehaviour 
{
    [SerializeField] List<GameObject> spawnedUIDomino = new List<GameObject>();
    public static UISelectionPanel Instance;
    [SerializeField] GameObject UIDominoPrefab;
    float YPos = -170;
    float XPos = 3;
    float XOffset = 50;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        SpawnNewUIDomino(new Vector3(XPos, YPos, 0));
        SpawnNewUIDomino(new Vector3(XPos + XOffset, YPos, 0));
        SpawnNewUIDomino(new Vector3(XPos - XOffset, YPos, 0));
        SpawnNewUIDomino(new Vector3(XPos - 2*XOffset, YPos, 0));
    }
    public void SpawnNewUIDomino(Vector3 pos)
    {
        GameObject newDomino = Instantiate(UIDominoPrefab, transform);
        RectTransform newDominoRect = newDomino.GetComponent<RectTransform>();
        newDominoRect.localPosition = pos;

        spawnedUIDomino.Add(newDomino);
    }

    public void GiveDomino()
    {
        // если есть свободное место, то на одном из них спавнится новое домино)
    }
    public void RemoveDomino(GameObject d)
    {
        spawnedUIDomino.Remove(d);
        Destroy(d);
    }

}
