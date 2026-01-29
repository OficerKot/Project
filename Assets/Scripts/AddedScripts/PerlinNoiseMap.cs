using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Скрипт, создающий ландшафт игровой карты из префабов, данных ему, по карте шума Перлина и по PNG-карте биомов.
/// Запускается один раз в начале игры. 
/// Создаёт четыре словаря препятствий (по одному на каждый биом игрового поля), двумерный массив, содержащий индекс
/// биома для каждой клетки игрового поля и генерирует карту по шуму Перлина на основе этого двумерного массива с учётом безопасных зон
/// После - вызывает метод PlaceItems() класса itemsPlacer.
/// 
/// Дополнительная настройка:
/// Задать префабы препятствий для словарей препятствий; 
/// Задать объекты, вокруг которых в определённом радиусе нельзя ставить препятствия;
/// Установить длину и ширину карты (должна совпадать с разрешением карты биомов).
/// </summary>
public class PerlinNoiseMap : MonoBehaviour
{
    Dictionary<int, GameObject> tileset_grove;
    Dictionary<int, GameObject> tileset_desert;
    Dictionary<int, GameObject> tileset_swamp;
    Dictionary<int, GameObject> tileset_cemetery;

    [SerializeField] public Transform[] safezones;
    public GameObject emptinnes;
    public GameObject tree;
    public GameObject bush_berries;
    public GameObject bush_empty;
    public GameObject pond;
    public GameObject cactus;
    public GameObject planks;
    public GameObject quagmire;

    public Texture2D sourceTexture;

    int map_width = 100;
    int map_height = 100;

    List<List<int>> noise_grid = new List<List<int>>();

    //any value between 4 and 20
    [SerializeField] float magnification_primary = 5f;
    [SerializeField] float magnification_secondary = 1.5f;

    int x_offset1 = 0, y_offset1 = 0; //<- +>, v- +^

    private Vector3 safepoint;
    private ItemsPlacer itemsPlacer;

    public void Start()
    {
        this.transform.position = new Vector2(-map_width / 2, -map_height / 2);
        CreateTileSets();

        for (int x = 0; x < map_width; ++x)
        {
            noise_grid.Add(new List<int>());
            for (int y = 0; y < map_height; ++y)
                noise_grid[x].Add(0);
        }

        Color32[] pixels1D = sourceTexture.GetPixels32();
        int t_width = sourceTexture.width;
        int t_height = sourceTexture.height;
        int[,] pixelMatrix = new int[t_width, t_height];
        for (int y = 0; y < t_height; ++y)
        {
            for (int x = 0; x < t_width; ++x)
            {
                if (Mathf.Abs(pixels1D[y * t_width + x][1] - 73) < 5)
                {
                    pixelMatrix[x, y] = 73;
                    continue;
                }
                if (Mathf.Abs(pixels1D[y * t_width + x][1] - 207) < 5)
                {
                    pixelMatrix[x, y] = 207;
                    continue;
                }
                if (Mathf.Abs(pixels1D[y * t_width + x][1] - 191) < 5)
                {
                    pixelMatrix[x, y] = 191;
                    continue;
                }
                if (Mathf.Abs(pixels1D[y * t_width + x][1] - 119) < 5)
                {
                    pixelMatrix[x, y] = 119;
                    continue;
                }
            }
        }
        //magnification = Random.Range(4, 20);
        x_offset1 = Random.Range(0, 1000);
        y_offset1 = Random.Range(0, 1000);
        GenerateMap(pixelMatrix);

        safepoint = new Vector3(safezones[0].position.x, safezones[0].position.y, 0);
        itemsPlacer = this.GetComponent<ItemsPlacer>();
        itemsPlacer.PlaceItems(map_width, map_height, safepoint);
    }

    public List<List<int>> GetNoiseGrid()
    {
        return noise_grid;
    }

    void CreateTileSets()
    {
        tileset_grove = new Dictionary<int, GameObject>();
        tileset_grove.Add(0, emptinnes);
        tileset_grove.Add(1, emptinnes);
        tileset_grove.Add(2, tree);
        tileset_grove.Add(3, pond);

        tileset_desert = new Dictionary<int, GameObject>();
        tileset_desert.Add(0, emptinnes);
        tileset_desert.Add(1, emptinnes);
        tileset_desert.Add(2, emptinnes);
        tileset_desert.Add(3, cactus);

        tileset_swamp = new Dictionary<int, GameObject>();
        tileset_swamp.Add(0, emptinnes);
        tileset_swamp.Add(1, emptinnes);
        tileset_swamp.Add(2, quagmire);
        tileset_swamp.Add(3, tree);

        tileset_cemetery = new Dictionary<int, GameObject>();
        tileset_cemetery.Add(0, pond);
        tileset_cemetery.Add(1, pond);
        tileset_cemetery.Add(2, planks);
        tileset_cemetery.Add(3, planks);
    }

    void GenerateMap(int[,] pixelMatrix)
    {
        for (int x = 0; x < map_width; ++x)
        {
            for (int y = 0; y < map_height; ++y)
            {
                if (IntersectsSafezone(safezones[0], x, y) || IntersectsSafezone(safezones[1], x, y) ||
                    IntersectsSafezone(safezones[2], x, y) || IntersectsSafezone(safezones[3], x, y) ||
                    IntersectsSafezone(safezones[4], x, y))
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
                CreateTile(tile_id, x, y, pixelMatrix[x, y]);
                //if (tile_id != 0 && tile_id != 1)
                //    CreateTile(tile_id, x, y, pixelMatrix[x, y]);
                //else
                //    continue;
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
        float scaled_perlin = clamp_perlin * tileset_grove.Count;
        if (scaled_perlin == 4)
        {
            scaled_perlin = 3;
        }
        return Mathf.FloorToInt(scaled_perlin);
    }
    void CreateTile(int tile_id, int x, int y, int dict_id)
    {
        GameObject tile_prefab = tileset_grove[tile_id];
        switch (dict_id)
        {
            case (207): { tile_prefab = tileset_desert[tile_id]; break; }
            case (73): { tile_prefab = tileset_cemetery[tile_id]; break; }
            case (191): { tile_prefab = tileset_grove[tile_id]; break; }
            case (119): { tile_prefab = tileset_swamp[tile_id]; break; }
            default: { tile_prefab = tileset_grove[tile_id]; break; }
        }
        if (tile_prefab == emptinnes) return;
        GameObject tile = Instantiate(tile_prefab, this.transform);

        tile.name = string.Format("tile_x{0}_y{1}", x, y);
        tile.transform.localPosition = new Vector3Int(x, y, 0);
    }

    bool IntersectsSafezone(Transform tf, int x, int y)
    {
        safepoint = new Vector3(tf.position.x - this.transform.position.x, tf.position.y - this.transform.position.y, 0);
        if ((safepoint.x - x) * (safepoint.x - x) + (safepoint.y - y) * (safepoint.y - y) <= 8)
        {
            return true;
        }
        else 
            return false;
    }
}
