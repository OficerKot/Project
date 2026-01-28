using System;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Цвет обелиска. Все обелиски на сцене должны иметь уникальный цвет.
/// </summary>
public enum ObeliskColor
{
    orange, green, purple, pink
}

/// <summary>
/// Обелиски можно собирать. При сборе всех обелисков на сцене игрок побеждает.
/// </summary>
public class Obelisk : PauseBehaviour
{
    [SerializeField] ObeliskColor color;
    [SerializeField] Sprite collectedSprite;
    bool isActive;
    SpriteAnimator sprAnim;

    private void Start()
    {
        isActive = true;
        sprAnim = GetComponent<SpriteAnimator>();
    }

    /// <summary>
    /// Обрабатывает клик мыши по обелиску, деактивируя его и сообщая менеджеру.
    /// </summary>
    public void OnMouseDown()
    {
        if (!isActive) return;
        ObeliskManager.Instance.Pick(color);
        sprAnim.StopAllCoroutines();
        sprAnim.enabled = false;
        GetComponent<SpriteRenderer>().sprite = collectedSprite;
        isActive = false;
    }
}