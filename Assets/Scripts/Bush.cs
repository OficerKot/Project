using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bush : MonoBehaviour
{
    public float minDistanceToInteract;
    private SpriteAnimator sprAnim;
    private string itemID = "berry";

    private void Awake()
    {
        sprAnim = GetComponent<SpriteAnimator>();
    }
    private void OnMouseDown()
    {
        if (!Inventory.Instance.isFull() && IsNearbyPlayer())
        {
            Interact();
        }
    }
    bool IsNearbyPlayer()
    {
        Vector2 playerPos = GameObject.FindWithTag("Player").transform.position;
        Vector2 thisPos = transform.position;

        float dist = Mathf.Pow(playerPos.x - thisPos.x, 2) + Mathf.Pow(playerPos.y - thisPos.y, 2);
        return dist <= Mathf.Pow(minDistanceToInteract, 2);
    }

    void Interact()
    {
        Inventory.Instance.AddItem(ItemManager.Instance.GetItemByID(itemID));
        sprAnim.ForcePlay("BushEmpty");
        Destroy(this);
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Handles.DrawWireDisc(transform.position, Vector3.forward, minDistanceToInteract); 
        GUIStyle style = new GUIStyle();
        Handles.Label(transform.position + Vector3.right * (minDistanceToInteract + 0.5f),
                     $"Radius: {minDistanceToInteract}", style);
    }
#endif
}
