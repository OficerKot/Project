using Unity.VisualScripting;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class DominoGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] Parts;
    private GameObject part1Prefab, part2Prefab;
    private GameObject spawnedPart1, spawnedPart2;
    [SerializeField] Button generateButton;

    [SerializeField] private float offsetY = 2f;

    private void Start()
    {
        generateButton.onClick.AddListener(Generate);
    }
    public void Generate()
    {
        ChooseParts();
        SpawnParts();
    }

    void ChooseParts()
    {
        int indx1 = Random.Range(0, Parts.Length);
        int indx2 = Random.Range(0, Parts.Length);
        part1Prefab = Parts[indx1];
        part2Prefab = Parts[indx2];
    }

    void SpawnParts()
    {
        if (spawnedPart2 != null) Destroy(spawnedPart1);
        if (spawnedPart1 != null) Destroy(spawnedPart2);
        spawnedPart1 = Instantiate(part1Prefab, gameObject.transform.position + new Vector3(0, offsetY), gameObject.transform.rotation);
        spawnedPart2 = Instantiate(part2Prefab, gameObject.transform.position, gameObject.transform.rotation);
    }
}
