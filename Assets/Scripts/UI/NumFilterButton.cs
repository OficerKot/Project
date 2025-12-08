using UnityEngine;
using UnityEngine.UI;

public class NumFilterButton : MonoBehaviour
{
    Image imageComponent;
    bool clicked;
    public int number;
    Button b;
    void Start()
    {
        clicked = false;
        b = GetComponent<Button>();
        imageComponent = GetComponent<Image>();
        b.onClick.AddListener(ApplyFilter);
    }
    void ApplyFilter()
    {
        SigilsMenu.Instance.ApplyFilter(number);
        if (clicked)
        {
            clicked = false;
            imageComponent.color = Color.white;
        }
        else
        {
            clicked = true;
            imageComponent.color = Color.gray6;
        }

    }
}


