using UnityEngine;

public class CellsGridGenerator : MonoBehaviour
{
    [SerializeField] public GameObject cell;
    [SerializeField] public int width = 100;
    [SerializeField] public int height = 100;

    void Start()
    {
        this.transform.position = new Vector2(-width / 2, -height / 2);
        for (int x = 0; x < width; ++x) 
        {
            for (int y = 0; y < height; ++y)
            {
                GameObject PlacedCell = Instantiate(cell, this.transform);
                PlacedCell.name = string.Format("cell_x{0}_y{1}", x, y);
                PlacedCell.transform.localPosition = new Vector3Int(x, y, 0);
            }
        }
    }

}
