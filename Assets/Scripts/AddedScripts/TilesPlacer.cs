using UnityEngine;
using UnityEngine.Tilemaps;

public class TilesPlacer : MonoBehaviour
{
    int map_width;
    int map_height;

    public int tiles_count = 10;
    public Tilemap targetTilemap;
    public AnimatedTile[] tileTypes;
    private int tileTypesCount;

    public void SecondaryObs(int width, int height)
    {
        map_width = width;
        map_height = height;
        tileTypesCount = tileTypes.Length;
        targetTilemap = this.transform.GetComponent<Tilemap>();
        GenerateTiles(tiles_count);
    }

    void GenerateTiles(int tiles_count)
    {
        for (int i  = 0; i<tiles_count; ++i)
        {
            int x = Random.Range(0, map_width);
            int y = Random.Range(0, map_height);
            TileBase tile = targetTilemap.GetTile(new Vector3Int(x, y, 0));
            if (tile == null)
            {
                int randomIndex = Random.Range(0, tileTypesCount);
                AnimatedTile selectedTile = tileTypes[randomIndex];
                targetTilemap.SetTile(new Vector3Int(x, y, 0), selectedTile);
            }
            else
            {
                --i;
                continue;
            }
            
        }
    }
}
