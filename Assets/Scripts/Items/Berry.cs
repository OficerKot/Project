using UnityEngine;
using UnityEngine.EventSystems;

public class Berry : MonoBehaviour, IPointerClickHandler
{
    private UIPickableIcon berryUIPI;
    public ItemData berry;

    void Awake()
    {
        berryUIPI = this.GetComponent<UIPickableIcon>();
        //Debug.Log("Found!");
        berryUIPI.SetDisposable();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log("Berry eaten");
        Hunger.Instance.CallHungerUp();
        Inventory.Instance.RemoveItem(berry);
    }
}
