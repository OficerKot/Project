using UnityEngine;
/// <summary>
/// Информация о части домино, хранит в себе
/// название, номер рисунка и номер рисунка соседних домино, изображение и изображение соседних домино,
/// игровой и ui префабы
/// </summary>
/// 
[CreateAssetMenu(fileName = "New domino type", menuName = "Domino/DominoData")]
public class DominoData : ScriptableObject
{
    public string dominoId;
    public int number;
    public int neighboursNumber;
    public ImageEnumerator image;
    public ImageEnumerator neighboursImage;
    public GameObject prefab;
    public GameObject UIprefab;
}
