using TMPro;
using UnityEngine;

public class UIInventoryCell : MonoBehaviour
{
    [SerializeField] GameObject item;
    [SerializeField] int itemCount;
    [SerializeField] TextMeshProUGUI counterText;

    private void Start()
    {
        if (item == null)
        {
            ClearCounter();
        }
    }

    public void PutItem(GameObject i)
    {
        item = i;
        if (i == null)
        {
            ClearCounter();
            return;
        }
        i.transform.position = transform.position;
    }
    public void RemoveItem()
    {
        Destroy(item);
        item = null;
        ClearCounter();
    }
    public void AddToCounter(int i)
    {
        itemCount += i;
        counterText.text = itemCount.ToString();
    }

    void ClearCounter()
    {
        itemCount = 0;
        counterText.text = "";
       
    }


}
