using Unity.VisualScripting;
using UnityEngine;

public enum ObeliskColor
{ 
    orange, green, purple, pink
}

public class Obelisk : MonoBehaviour
{
    [SerializeField] ObeliskColor color;
    [SerializeField] Sprite collectedSprite;
    public void OnMouseDown()
    {
        ObeliskManager.Instance.Pick(color);
        GetComponent<SpriteRenderer>().sprite = collectedSprite;
        Destroy(this);
    }
}
