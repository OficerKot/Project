using UnityEngine;
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
