using System;
using Unity.VisualScripting;
using UnityEngine;

public enum ObeliskColor
{ 
    orange, green, purple, pink
}

public class Obelisk : PauseBehaviour
{
    [SerializeField] ObeliskColor color;
    [SerializeField] Sprite collectedSprite;
    bool isActive;

    private void Start()
    {
        isActive = true;
    }
    public void OnMouseDown()
    {
        if (!isActive) return;
        ObeliskManager.Instance.Pick(color);
        GetComponent<SpriteRenderer>().sprite = collectedSprite;
        isActive = false;
    }
}
