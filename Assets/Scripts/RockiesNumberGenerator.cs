using UnityEngine;

/// <summary>
/// Генерирует количество камней для объекта "Камешки"
/// </summary>
public class RockiesNumberGenerator : MonoBehaviour
{
    SpriteRenderer spriteRend;
    public Sprite[] sprites; // по порядку для цифр от 1 до 6
    int count;

    public void Generate()
    {
        spriteRend = GetComponent<SpriteRenderer>();
        count = Random.Range(1, 6);
        spriteRend.sprite = sprites[count - 1];
    }

    public int GetCount()
    {
        return count;
    }
}
