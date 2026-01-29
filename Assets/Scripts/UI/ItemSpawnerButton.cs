
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Позиция кнопки на кольце крафта для корректного отображения кнопки и описания предмета при наведении курсора мыши.
/// </summary>
public enum Position
{
    left_down, left_up, right_down, right_up
}

/// <summary>
/// Категория меню крафта, в которой находится данная кнопка
/// </summary>
public enum Category
{
    blue, green, red
}

/// <summary>
/// Кнопка создания предмета в меню крафта. Отображает информацию о рецепте при наведении, позволяет создавать и добавлять
/// предметы в инвентарь.
/// </summary>
public class ItemSpawnerButton : Notificationable, IPointerEnterHandler, IPointerExitHandler
{
    public Position pos;
    public ItemData objectToSpawn;
    public GameObject craftPanelPrefab;
    public string description;
    public Category category;
    Vector3 offset;
    GameObject craftPanel;
    [SerializeField] GameObject AvailableImage; //в будущем нужно свести к одной переменной и работать с яркостью изображения
    [SerializeField] GameObject notAvailableImage;

  

    private void Start()
    {
        switch (pos)     //Подбор отступов для всплывающих окон при наведении на кнопку.
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

    /// <summary>
    /// При наведении курсора показывает панель с информацией о предмете.
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        craftPanel = Instantiate(craftPanelPrefab, transform.position, transform.rotation, (GameObject.Find("Craft").transform));
        RectTransform rect = craftPanel.GetComponent<RectTransform>();
        rect.localPosition += offset;
        craftPanel.GetComponent<CraftPanel>().text.text = description;
        AddItemsForCraft();
    }

    /// <summary>
    /// При уходе курсора скрывает панель с информацией о предмете.
    /// </summary>
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

    /// <summary>
    /// Добавляет иконки необходимых предметов для крафта предмета на панель.
    /// </summary>
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