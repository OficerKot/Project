
using UnityEngine.EventSystems;
using UnityEngine;
using Unity.VisualScripting;


public class UIDomino : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] GameObject dominoPlayablePrefab;
    GameObject spawnedPlayableDomino;
    [SerializeField] GameObject blurImage;
    
    DominoData part1, part2;
    GameObject part1UI, part2UI;
    
 
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
       
        spawnedPlayableDomino.GetComponent<Domino>().Initialize(part1, part2);
        spawnedPlayableDomino.GetComponent<Domino>().PickUp();
    }
    void GenerateParts()
    {
        ChooseParts();
        SpawnParts();
  
    }
    void ChooseParts()
    {
        part1 = DominoManager.Instance.GetRandomDomino();
        part2 = DominoManager.Instance.GetRandomDomino();
    }
    void SpawnParts()
    {
        RectTransform thisRectT = GetComponent<RectTransform>();

        part1UI = Instantiate(part1.UIprefab, transform);
        part2UI = Instantiate(part2.UIprefab, transform);

        RectTransform part1RectT = part1UI.GetComponent<RectTransform>();
        RectTransform part2RectT = part2UI.GetComponent<RectTransform>();
       
        part1RectT.localPosition = new Vector3(0, offsetY, 0);
        part2RectT.localPosition = new Vector3(0, -offsetY, 0);
    }
}
