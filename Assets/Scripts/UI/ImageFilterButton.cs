using UnityEngine;
using UnityEngine.UI;

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
