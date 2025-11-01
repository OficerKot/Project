using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PerlinNoiseMap : MonoBehaviour
{
    Dictionary<int, AnimatedTile> tileset;
    public AnimatedTile emptinnes;
    public AnimatedTile tree;
    public AnimatedTile bush_berries;
    public AnimatedTile bush_empty;
    public AnimatedTile pond;

    int map_width = 64;
    int map_height = 36;

    List<List<int>> noise_grid = new List<List<int>>();
    List<List<AnimatedTile>> tile_grid = new List<List<AnimatedTile>>();

    Tilemap targetTilemap;

    //any value between 4 and 20
    float magnification = 7f;

    int x_offset = 0, y_offset = 0; //<- +>, v- +^

    private TilesPlacer secTilesPlacer;

    void Start()
    {
        this.transform.position = new Vector2(-map_width / 2, -map_height / 2);
        targetTilemap = this.transform.GetComponent<Tilemap>();
        CreateTileSet();
        
        magnification = Random.Range(4, 20);
        x_offset = Random.Range(0, 1000);
        y_offset = Random.Range(0, 1000);
        GenerateMap();

        secTilesPlacer = this.transform.GetComponent<TilesPlacer>();
        secTilesPlacer.SecondaryObs(map_width, map_height);
    }

    void CreateTileSet()
    {
        tileset = new Dictionary<int, AnimatedTile>();
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
            tile_grid.Add(new List<AnimatedTile>());
            for (int y = 0; y < map_height; ++y)
            {
                int tile_id = GetIdUsingPerlin(x, y);
                noise_grid[x].Add(tile_id);
                Debug.Log($"Tile is {tileset[tile_id]}");
                if (tile_id != 0 && tile_id != 1)
                    targetTilemap.SetTile(new Vector3Int(x, y, 0), tileset[tile_id]);
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
}
