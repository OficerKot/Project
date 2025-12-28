using System.Collections.Generic;
using UnityEngine;

public class UISelectionPanel : PauseBehaviour
{
    [SerializeField] List<GameObject> spawnedUIDomino = new List<GameObject>();
    public static UISelectionPanel Instance;
    [SerializeField] GameObject UIDominoPrefab;
    bool isActive = true;
    const int DOMINO_CNT = 5;
    public float YPos = -170;
    public float XPos = 5;
    public float XOffset = 50;

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
        GeneratePanel();
    }

    public override void OnGamePaused(bool isGamePaused)
    {
        isActive = !isGamePaused;
    }
    public void GeneratePanel()
    {
        if (!isActive) return;
        if(spawnedUIDomino.Count != 0)
        {
            foreach (var c in spawnedUIDomino)
            {
                Hunger.Instance.MakeStep();
                Destroy(c);
            }
            spawnedUIDomino.Clear();
        }
        for (int i = 0; i < DOMINO_CNT; i++)
        {
            SpawnNewUIDomino(new Vector3(XPos + i * XOffset, YPos, 0));
        }
    }
    public void SpawnNewUIDomino(Vector3 pos)
    {
        GameObject newDomino = Instantiate(UIDominoPrefab, transform);
        RectTransform newDominoRect = newDomino.GetComponent<RectTransform>();
        newDominoRect.localPosition = pos;

        spawnedUIDomino.Add(newDomino);
    }

    public void RemoveDomino(GameObject d)
    {
        spawnedUIDomino.Remove(d);
        Destroy(d);
    }

}
