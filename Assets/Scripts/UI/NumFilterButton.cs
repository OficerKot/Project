using UnityEngine;
using UnityEngine.UI;

public class NumFilterButton : MonoBehaviour
{
    Image imageComp;
    bool clicked;
    public int number;
    Button b;
    void Start()
    {
        clicked = false;
        b = GetComponent<Button>();
        imageComp = GetComponent<Image>();
        b.onClick.AddListener(ApplyFilter);
    }
    void ApplyFilter()
    {
        SigilsMenu.Instance.ApplyFilter(number);
        if (clicked)
        {
            clicked = false;
            imageComp.color = Color.white;
        }
        else
        {
            clicked = true;
            imageComp.color = Color.gray6;
        }

    }
}


