using UnityEngine;
using UnityEngine.EventSystems;

public class Bush : MonoBehaviour
{
    private SpriteAnimator sprAnim;
    private string itemID = "berry";

    private void Awake()
    {
        sprAnim = GetComponent<SpriteAnimator>();
    }
    private void OnMouseDown()
    {
        if (!Inventory.Instance.isFull())
        {
            Inventory.Instance.AddItem(ItemManager.Instance.GetItemByID(itemID));
            sprAnim.ForcePlay("BushEmpty");
            Destroy(this);
        }
        else Debug.Log("FULL INVENTORY!!!");
    }

}
