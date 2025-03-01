using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{

    public Tilemap groundTileMap;

    public int width;
    public int height;
    [Range(0.4f, 0.68f)]
    public float posibility;
    public float[,] mapData;
    public float z = 0.06f;
    private float noiseValue;

    public TileBase grassTile;
    public TileBase groundTile;
    public TileBase edgeTile;
    public GameObject grass;
    public GameObject tree;
    public GameObject rock;

    private void Start()
    {
        GenerateMap();
    }
    public void GenerateMap()
    {

        ClearTileMap();
        mapData = new float[width, height];
        float minValue = float.MaxValue;
        float maxValue = float.MinValue;

        float randomOffset = UnityEngine.Random.Range(-10000, 10000);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
               noiseValue = Mathf.PerlinNoise(x*z+randomOffset, y*z+randomOffset);
                mapData[x, y] = noiseValue ;
                if (noiseValue < minValue) minValue = noiseValue ;
                if(noiseValue > maxValue) maxValue = noiseValue ;

            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                mapData[x, y] = Mathf.InverseLerp(minValue, maxValue, mapData[x, y]);
            }
        }
        
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                TileBase tile;
                // ¼ì²éÊÇ·ñÊÇ±ßÔµ
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    tile = edgeTile;
                }
                else
                {
                    tile = mapData[x, y] > posibility ? groundTile : grassTile;
                }
                groundTileMap.SetTile(new Vector3Int(x, y), tile);
            }
        }

   SpawnEnvironment();

    }



    public void ClearTileMap()
    {
        groundTileMap.ClearAllTiles();
    }


    private void SpawnEnvironment()
    {
        for (int x = 1; x < width-5; x++)
        {
            for (int y = 1; y < height-5; y++)
            {
                Vector3Int cell = new Vector3Int(x, y, 0);
                if (groundTileMap.GetTile(cell) == grassTile)
                {
                    if (Random.value < 0.05f) 
                    {
                        Instantiate(tree, groundTileMap.GetCellCenterWorld(cell), Quaternion.identity);
                    }
                    else if (Random.value < 0.03f) 
                    {
                        Instantiate(grass, groundTileMap.GetCellCenterWorld(cell), Quaternion.identity);
                    }
                }
                else
                {
                    if (Random.value < 0.02f)
                    {
                        Instantiate(rock, groundTileMap.GetCellCenterWorld(cell), Quaternion.identity);
                    }
                }
            }
        }
    }
}
