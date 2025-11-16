using UnityEngine;

[CreateAssetMenu(fileName = "DominoManager", menuName = "Domino/DominoManager")]

public class DominoManager : ScriptableObject
{
    public DominoData[] allDomino;
    public static DominoManager Instance;

    private void OnEnable()
    {
        Instance = this;
    }
    public DominoData GetDominoByID(string id)
    {
        return System.Array.Find(allDomino, domino => domino.dominoId == id);
    }

    public DominoData GetRandomDomino()
    {
        int indx = Random.Range(0, allDomino.Length);
        return allDomino[indx];
    }

}
public enum ImageEnumerator
{
    any, bone, fireflies
}
