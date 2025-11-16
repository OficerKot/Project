using UnityEngine;
[CreateAssetMenu(fileName = "New domino type", menuName = "Domino/DominoData")]
public class DominoData : ScriptableObject
{
    public string dominoId;
    public int number;
    public ImageEnumerator image;
    public GameObject prefab;
    public GameObject UIprefab;
}
