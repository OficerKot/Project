using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

public class SigilsMenu : Menu
{
    public static SigilsMenu Instance;

    [SerializeField] float scaleKoefficient = 2.5f;
    [SerializeField] List<Transform> cells;
    [SerializeField] GameObject menu;
    List<GameObject> spawnedIcons = new List<GameObject>();
    List<DominoData> sortedDominoList = new List<DominoData>();
    HashSet<ImageEnumerator> imageFilters = new HashSet<ImageEnumerator>();
    HashSet<int> numberFilters = new HashSet<int>();
    int prevAvailableCount;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            prevAvailableCount = DominoManager.Instance.available.Count;
            FillAvailable();
        }
        else
        {
            Destroy(this);
        }
    }
    public override void Open()
    {
        menu.SetActive(true);
        if (spawnedIcons.Count == 0) FillAvailable();
    }
    public override void Close()
    {
        menu.SetActive(false);
    }
    private void Update()
    {
        if (prevAvailableCount != DominoManager.Instance.available.Count) 
        {
            Debug.Log("Update");
            UpdateAvailable();
            CellsPlacer.Instance.UpdateButtons();
            prevAvailableCount = DominoManager.Instance.available.Count;
        }

    }

    public void ApplyFilter(int num)
    {
        if (numberFilters.Contains(num))
        {
            numberFilters.Remove(num);
        }
        else
        {
            numberFilters.Add(num);
        }
        UpdateAvailable();
    }
    public void ApplyFilter(ImageEnumerator im)
    {
        if (imageFilters.Contains(im))
        {
            imageFilters.Remove(im);
        }
        else
        {
            imageFilters.Add(im);
        }
        UpdateAvailable();
    }

    void FillAvailable() 
    {
        if (DominoManager.Instance.HasAvailable())
        {
            int curIndx = 0;
            sortedDominoList = DominoManager.Instance.available.ToList();
            sortedDominoList.Sort((a, b) => DominoManager.Instance.order[a.image].CompareTo(DominoManager.Instance.order[b.image]));
            sortedDominoList.Sort((a, b) => a.number.CompareTo(b.number));

            foreach (DominoData d in sortedDominoList)
            {
                bool isImageOk = imageFilters.Count == 0 || imageFilters.Contains(d.image);
                bool isNumberOk = numberFilters.Count == 0 || numberFilters.Contains(d.number);
                if (isImageOk && isNumberOk)
                {
                    cells[curIndx].gameObject.SetActive(true);
                    GameObject icon = Instantiate(d.UIprefab, menu.transform);
                    icon.transform.position = cells[curIndx++].position;
                    icon.transform.localScale *= scaleKoefficient;
                    spawnedIcons.Add(icon);
                }
            }
        }

    }

    void ClearAvailable()
    {
        int curIndx = 0;
        foreach(GameObject icon in spawnedIcons)
        {
            Destroy(icon);
            cells[curIndx++].gameObject.SetActive(false);
        }
        spawnedIcons.Clear();
    }

    void UpdateAvailable()
    {
        ClearAvailable();
        FillAvailable();
    }
 

}
