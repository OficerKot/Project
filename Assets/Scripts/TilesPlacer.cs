using UnityEngine;
using UnityEngine.Tilemaps;

public class TilesPlacer : MonoBehaviour
{
    public int map_width = 10;
    public int map_height = 10;
    public int tiles_count = 10;
    public Tilemap targetTilemap;
    public Tile[] tileTypes;
    public int tileTypesCount = 5;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateTiles(tiles_count);
    }

    void GenerateTiles(int tiles_count)
    {
        for (int i  = 0; i<tiles_count; ++i)
        {
            int x = Random.Range(-map_width/2, map_width/2);
            int y = Random.Range(-map_height/2, map_height/2);
            int randomIndex = Random.Range(0, tileTypesCount);
            Tile selectedTile  = tileTypes[randomIndex];

            targetTilemap.SetTile(new Vector3Int(x, y, 0), selectedTile);
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
