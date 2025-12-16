using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsPlacer : MonoBehaviour
{
    int map_width;
    int map_height;

    public int items_count = 10;
    
    public GameObject[] itemTypes;
    private int itemsCount;
    private PerlinNoiseMap perlin;
    private Cell cell;
    private ObstacleSnap obs_snap;
    private List<List<int>> noise_grid;

    void Awake()
    {
        perlin = this.GetComponent<PerlinNoiseMap>();
        noise_grid = perlin.GetNoiseGrid();
    }

    public void PlaceItems(int width, int height, Vector3 safepoint)
    {
        map_width = width;
        map_height = height;
        itemsCount = itemTypes.Length;
        GenerateTiles(items_count, safepoint);
    }

    void GenerateTiles(int items_count, Vector3 safepoint)
    {
        for (int i  = 0; i<items_count; ++i)
        {
            int x = Random.Range(0, map_width);
            int y = Random.Range(0, map_height);
            if ((safepoint.x - x) * (safepoint.x - x) + (safepoint.y - y) * (safepoint.y - y) <= 16)
            {
                continue;
            }

            if (noise_grid[x][y] == 0 || noise_grid[x][y] == 1)
            {
                CreateItem(x, y);
            }
            else
            {
                --i;
                continue;
            }
            
        }
    }
    void CreateItem(int x, int y)
    {
        int item_id = Random.Range(0, itemsCount-1);
        GameObject item_prefab = itemTypes[item_id];
        GameObject item = Instantiate(item_prefab, this.transform);

        item.name = string.Format("item_{0}_x{1}_y{2}", item_prefab.name, x, y);
        item.transform.localPosition = new Vector3Int(x, y, 0);

        obs_snap = item.GetComponent<ObstacleSnap>();
        obs_snap.spawned = true;
    }
}
