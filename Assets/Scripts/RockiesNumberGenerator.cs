using UnityEngine;

/// <summary>
/// Генерирует количество камней для объекта "Камешки"
/// </summary>
public class RockiesNumberGenerator : MonoBehaviour
{
    SpriteAnimator SprAnim; //Р—Р°РјРµРЅРёР» SpriteRenderer РЅР° SpriteAnimator, С‚Р°Рє С‡С‚Рѕ С‚РµРїРµСЂСЊ РјР°СЃСЃРёРІ СЃРїСЂР°Р№С‚РѕРІ РЅРµРјРЅРѕРіРѕ Р±РµСЃРїРѕР»РµР·РµРЅ...
    public Sprite[] sprites; // РїРѕ РїРѕСЂСЏРґРєСѓ РґР»СЏ С†РёС„СЂ РѕС‚ 1 РґРѕ 6 
    int count;

    public void Generate()
    {
        SprAnim = GetComponent<SpriteAnimator>();
        count = Random.Range(1, 7); 
        SprAnim.ForcePlay("Rockies" + count.ToString());
    }

    public int GetCount()
    {
        return count;
    }
}
