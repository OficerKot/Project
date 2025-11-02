using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PerlinNoiseMap : MonoBehaviour
{
    Dictionary<int, GameObject> tileset;
    public GameObject emptinnes;
    public GameObject tree;
    public GameObject bush_berries;
    public GameObject bush_empty;
    public GameObject pond;

    int map_width = 17;
    int map_height = 9;

    List<List<int>> noise_grid = new List<List<int>>();
    List<List<GameObject>> tile_grid = new List<List<GameObject>>();

    //any value between 4 and 20
    float magnification = 7f;

    int x_offset = 0, y_offset = 0; //<- +>, v- +^

    private TilesPlacer secTilesPlacer;

    void Start()
    {
        this.transform.position = new Vector2(-map_width / 2, -map_height / 2);
        CreateTileSet();
        
        magnification = Random.Range(4, 20);
        x_offset = Random.Range(0, 1000);
        y_offset = Random.Range(0, 1000);
        GenerateMap();

    }

    void CreateTileSet()
    {
        tileset = new Dictionary<int, GameObject>();
        tileset.Add(0, emptinnes);
        tileset.Add(1, emptinnes);
        tileset.Add(2, tree);
        tileset.Add(3, pond);
    }

    void GenerateMap()
    {
        for (int x = 0; x < map_width; ++x)
        {
            noise_grid.Add(new List<int>());
            tile_grid.Add(new List<GameObject>());
            for (int y = 0; y < map_height; ++y)
            {
                int tile_id = GetIdUsingPerlin(x, y);
                noise_grid[x].Add(tile_id);
                Debug.Log($"Tile is {tileset[tile_id]}");
                if (tile_id != 0 && tile_id != 1)
                    CreateTile(tile_id, x, y);
                else
                    continue;

            }
        }
    }

    int GetIdUsingPerlin(int x, int y)
    {
        float raw_perlin = Mathf.PerlinNoise(
            (x - x_offset) / magnification,
            (y - y_offset) / magnification
        );
        float clamp_perlin = Mathf.Clamp(raw_perlin, 0.0f, 1.0f);
        float scaled_perlin = clamp_perlin * tileset.Count;
        if (scaled_perlin == 4)
        {
            scaled_perlin = 3;
        }
        return Mathf.FloorToInt(scaled_perlin);
    }
    void CreateTile(int tile_id, int x, int y)
    {
        GameObject tile_prefab = tileset[tile_id];
        GameObject tile = Instantiate(tile_prefab, this.transform);

        tile.name = string.Format("tile_x{0}_y{1}", x, y);
        tile.transform.localPosition = new Vector3Int(x, y, 0);
    }
}
