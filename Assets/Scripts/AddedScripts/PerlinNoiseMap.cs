using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PerlinNoiseMap : MonoBehaviour
{
    Dictionary<int, GameObject> tileset;
    public GameObject Player;
    public GameObject emptinnes;
    public GameObject tree;
    public GameObject bush_berries;
    public GameObject bush_empty;
    public GameObject pond;

    int map_width = 100;
    int map_height = 50;

    List<List<int>> noise_grid = new List<List<int>>();

    //any value between 4 and 20
    float magnification_primary = 10f;
    float magnification_secondary = 5.5f;

    int x_offset1 = 0, y_offset1 = 0; //<- +>, v- +^

    private ItemsPlacer itemsPlacer;

    void Start()
    {
        this.transform.position = new Vector2(-map_width / 2, -map_height / 2);
        CreateTileSet();

        for (int x = 0; x < map_width; ++x)
        {
            noise_grid.Add(new List<int>());
            for (int y = 0; y < map_height; ++y)
                noise_grid[x].Add(0);
        }

        //magnification = Random.Range(4, 20);
        x_offset1 = Random.Range(0, 1000);
        y_offset1 = Random.Range(0, 1000);
        GenerateMap();

        itemsPlacer = this.GetComponent<ItemsPlacer>();
        itemsPlacer.PlaceItems(map_width, map_height);
    }

    public List<List<int>> GetNoiseGrid()
    {
        return noise_grid;
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
        Vector3 safepoint = new Vector3(Player.transform.position.x - this.transform.position.x, Player.transform.position.y - this.transform.position.y, 0);
        for (int x = 0; x < map_width; ++x)
        {
            for (int y = 0; y < map_height; ++y)
            {
                if ((safepoint.x - x) * (safepoint.x - x) + (safepoint.y - y) * (safepoint.y - y) <= 16)
                {
                    continue;
                }
                int tile_id = GetIdUsingPerlin(x, y, x_offset1, y_offset1, magnification_primary);
                noise_grid[x][y] = tile_id;
            }
        }
        int x_offset2 = Random.Range(0, 1000);
        int y_offset2 = Random.Range(0, 1000);
        for (int x = 0; x < map_width; ++x)
        {
            for (int y = 0; y < map_height; ++y)
            {
                int tile_id = GetIdUsingPerlin(x, y, x_offset2, y_offset2, magnification_secondary);
                if (tile_id != 0 && tile_id != 1)
                {
                    noise_grid[x][y] = 0;
                }
            }
        }
        for (int x = 0; x < map_width; ++x)
        {
            for (int y = 0; y < map_height; ++y)
            {
                int tile_id = noise_grid[x][y];
                if (tile_id != 0 && tile_id != 1)
                    CreateTile(tile_id, x, y);
                else
                    continue;
            }
        }
    }

    int GetIdUsingPerlin(int x, int y, int x_off, int y_off, float magn)
    {
        float raw_perlin = Mathf.PerlinNoise(
            (x - x_off) / magn,
            (y - y_off) / magn
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
