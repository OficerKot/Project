using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour
{
    [SerializeField] string itemID;
    bool isPlaced = true;
    public void OnMouseDown()
    {
        if(!Inventory.Instance.isFull() && isPlaced)
        {
            Pick();
        }

    }

    private void Update()
    {
        if(!isPlaced) Move();
    }
    public bool IsPlaced()
    {
        return isPlaced;
    }

    public void SetIsPlaced(bool val)
    {
        isPlaced = val;
    }
    public string GetID()
    {
        return itemID;
    }
    void Pick() // собрать с клетки
    {
        Inventory.Instance.AddItem(this);
        gameObject.SetActive(false);
    }

    void Move() // перемещать (когда нажали на иконку)
    {
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetPos.z = 0;
        transform.position = Vector3.Lerp(transform.position, targetPos, 1);
    }
}
