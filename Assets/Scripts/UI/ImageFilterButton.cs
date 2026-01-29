using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Кнопки фильтрации в меню сигилов. При нажатии в меню остаются только сигилы с соответствующим изображением
/// </summary>
public class ImageFilterButton : MonoBehaviour
{
    public GameObject blurObject;
    bool clicked;
    public ImageEnumerator image;
    Button b;
    void Start()
    {
        clicked = false;
        b = GetComponent<Button>();
        b.onClick.AddListener(ApplyFilter);
    }

    /// <summary>
    /// Нажатие кнопки фильтрации, активация/деактивация фильтра в SigilsMenu
    /// </summary>
    void ApplyFilter()
    {
        SigilsMenu.Instance.ApplyFilter(image);
        if (clicked)
        {
            clicked = false;
            Debug.Log("Unactivated");
            blurObject.SetActive(false);
        }
        else
        {
            clicked = true;
            Debug.Log("Activated");
            blurObject.SetActive(true);
        }

    }
}
