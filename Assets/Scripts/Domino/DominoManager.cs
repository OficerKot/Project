using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(fileName = "DominoManager", menuName = "Domino/DominoManager")]

public class DominoManager : ScriptableObject
{
    public List<DominoData> allDomino;
    public List<DominoData> available, basic;

    public Dictionary<ImageEnumerator, int> order = new Dictionary<ImageEnumerator, int>()
    {
        {ImageEnumerator.bone, 1} , {ImageEnumerator.fireflies, 2}, {ImageEnumerator.leaves, 3 }, {ImageEnumerator.flowers, 4 }, {ImageEnumerator.axe, 5}, {ImageEnumerator.pickaxe, 6}
    };

    private static DominoManager _instance;
    public static DominoManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<DominoManager>("DominoManager");

                if (_instance == null)
                {
                    _instance = CreateInstance<DominoManager>();
                    Debug.LogWarning("Created new DominoManager instance. Consider creating it as an asset.");
                }
            }
            return _instance;
        }
    }

    private void OnEnable()
    {
        if (_instance == null)
        {
            _instance = this;
        }
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
       return allDomino.Find(d => d.image == image && (d.number == 0 || d.number == number));
    }
    public bool HasAvailable()
    {
        return available.Count > 0;
    }
}
public enum ImageEnumerator
{
    any, bone, fireflies, leaves, flowers, axe, pickaxe
}
