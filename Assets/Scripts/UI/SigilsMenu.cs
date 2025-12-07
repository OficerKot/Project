using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SigilsMenu : MonoBehaviour
{
    public static SigilsMenu Instance;

    bool isMenuOpen = false;
    [SerializeField] float scaleKoefficient = 2.5f;
    [SerializeField] GameObject menu;
    [SerializeField] List<Transform> cells;
    List<GameObject> spawnedIcons = new List<GameObject>();
    List<DominoData> sortedDominoList = new List<DominoData>();
    HashSet<ImageEnumerator> imageFilters = new HashSet<ImageEnumerator>();
    HashSet<int> numberFilters = new HashSet<int>();
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (isMenuOpen)
            {
                CloseMenu();
            }
            else
            {
                OpenMenu();
            }
        }

        if (spawnedIcons.Count != DominoManager.Instance.available.Count)
        {
            UpdateAvailable();
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

    void FillAvailable() // DominoManager.Instance.GetDomino(d.image, 1).UIprefab для фильтров 
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
    public void CloseMenu()
    {
        menu.SetActive(false);
        isMenuOpen = false;
    }

    public void OpenMenu()
    {
        menu.SetActive(true);
        isMenuOpen = true;
        if(spawnedIcons.Count == 0)
        FillAvailable();
    }

}
