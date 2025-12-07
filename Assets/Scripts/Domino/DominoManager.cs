using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "DominoManager", menuName = "Domino/DominoManager")]

public class DominoManager : ScriptableObject
{
    public List<DominoData> allDomino;
    public List<DominoData> available, basic;

    public Dictionary<ImageEnumerator, int> order = new Dictionary<ImageEnumerator, int>()
    {
        {ImageEnumerator.bone, 1} , {ImageEnumerator.fireflies, 2}, {ImageEnumerator.leaves, 3 }
    };

    public static DominoManager Instance;

    private void OnEnable()
    {
        Instance = this;
    }
    public DominoData GetDominoByID(string id)
    {
        return allDomino.Find(domino => domino.dominoId == id);
    }

    public DominoData GetRandomDomino()
    {
        List<DominoData> allAvailable = basic.ToList();
        allAvailable.AddRange(available);
        int indx = Random.Range(0, allAvailable.Count);
        return allAvailable[indx];
    }

    public DominoData GetDomino(ImageEnumerator image, int number)
    {
       return allDomino.Find(d => d.image == image && d.number == number);
    }
    public bool HasAvailable()
    {
        return available.Count > 0;
    }
}
public enum ImageEnumerator
{
    any, bone, fireflies, leaves
}
