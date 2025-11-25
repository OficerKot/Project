
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum Position
{
    left_down, left_up, right_down, right_up
}



public class ItemSpawnerButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Position pos;
    public ItemData objectToSpawn;
    public GameObject craftPanelPrefab;
    public string description;
    Vector3 offset;
    GameObject craftPanel;
    [SerializeField] GameObject AvailableImage;
    [SerializeField] GameObject notAvailableImage;

    private void Start()
    {
        switch (pos)
        {
            case Position.left_down:
                offset = new Vector3(-40, -45, 0);
                break;
            case Position.left_up:
                offset = new Vector3(-40, 45, 0);
                break;
            case Position.right_down:
                offset = new Vector3(40, -45, 0);
                break;
            case Position.right_up:
                offset = new Vector3(40, 45, 0);
                break;
        }

    }

    private void Update()
    {
        if (UICraftWindow.Instance.availableItems.Contains(objectToSpawn))
        {
            notAvailableImage.SetActive(false);
            AvailableImage.SetActive(true);
        }
        else
        {
            AvailableImage.SetActive(false);
            notAvailableImage.SetActive(true);
        }

    }
    public void OnPointerEnter(PointerEventData eventData)
    {

        craftPanel = Instantiate(craftPanelPrefab, transform.position, transform.rotation, (GameObject.Find("CraftWindow").transform));
        RectTransform rect = craftPanel.GetComponent<RectTransform>();
        rect.localPosition += offset;
        craftPanel.GetComponent<CraftPanel>().text.text = description;
        AddItemsForCraft();

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Destroy(craftPanel);
        craftPanel = null;
    }

    private void OnDisable()
    {
        if (craftPanel)
        {
            Destroy(craftPanel);
            craftPanel = null;
        }
    }

    void AddItemsForCraft()
    {
        int cellIndx = 0;
        List<GameObject> cells = craftPanel.GetComponent<CraftPanel>().cells;
        foreach (ItemData obj in objectToSpawn.itemsForCraft)
        {
            GameObject icon = Instantiate(obj.UIprefab, cells[cellIndx].transform);
            icon.transform.position = cells[cellIndx].transform.position;
            Destroy(icon.GetComponent<UIPickableIcon>());

            cellIndx++;
        }
    }
}
