
using UnityEngine.EventSystems;
using UnityEngine;
using Unity.VisualScripting;


public class UIDomino : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] GameObject[] PartsUI;
    [SerializeField] GameObject dominoPlayablePrefab;
    [SerializeField] GameObject blurImage;

    GameObject spawnedPlayableDomino;
    GameObject part1, part2;

    int part1Indx, part2Indx; // для передачи в функцию Initialize при спавне playableDomino


   

    [SerializeField] float offsetY = 2f;

    [SerializeField] bool clicked = false;
    void Start()
    {
        blurImage.SetActive(false);
        GenerateParts();
        blurImage.transform.SetAsLastSibling();
    }


    private void Update()
    {
        if (spawnedPlayableDomino != null && spawnedPlayableDomino.GetComponent<Domino>().isPlaced())
        {
            spawnedPlayableDomino = null;
            UISelectionPanel.Instance.RemoveDomino(gameObject);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        if (GameManager.Instance.WhatInHand() == null || GameManager.Instance.WhatInHand() == gameObject)
        {
            clicked = !clicked;
        }

        if (clicked)
        {
            GameManager.Instance.PutInHand(gameObject);
            SpawnPlayable();
            blurImage.SetActive(true);
        }
        else
        {
            GameManager.Instance.PutInHand(null);
            Destroy(spawnedPlayableDomino);
            spawnedPlayableDomino = null;
            blurImage.SetActive(false);
        }
    }

    void SpawnPlayable()
    {
        spawnedPlayableDomino = Instantiate(dominoPlayablePrefab, transform.position, transform.rotation);
       
        spawnedPlayableDomino.GetComponent<Domino>().Initialize(part1Indx, part2Indx);
        spawnedPlayableDomino.GetComponent<Domino>().PickUp();
    }
    void GenerateParts()
    {
        ChooseParts();
        SpawnParts();
  
    }
    void ChooseParts()
    {
        part1Indx = Random.Range(0, PartsUI.Length);
        part2Indx = Random.Range(0, PartsUI.Length);
        part1 = PartsUI[part1Indx];
        part2 = PartsUI[part2Indx];
    }
    void SpawnParts()
    {
        RectTransform thisRectT = GetComponent<RectTransform>();

        part1 = Instantiate(part1, transform);
        part2 = Instantiate(part2, transform);

        RectTransform part1RectT = part1.GetComponent<RectTransform>();
        RectTransform part2RectT = part2.GetComponent<RectTransform>();
       
        part1RectT.localPosition = new Vector3(0, offsetY, 0);
        part2RectT.localPosition = new Vector3(0, -offsetY, 0);
    }
}
