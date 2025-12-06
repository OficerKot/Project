using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoronoiDiagram : MonoBehaviour
{
    [SerializeField] private Color[] possibleColors;

    private int imgSize;
    private int gridSize = 5;
    private int pixelsPerCell;
    private RawImage image;
    private Vector2Int[,] pointsPositions;
    private Color[,] colors;

    private void Awake()
    {
        image = GetComponent<RawImage>();
        imgSize = Mathf.RoundToInt(image.GetComponent<RectTransform>().sizeDelta.x);
        GenerateDiagram();
    }

    private void GenerateDiagram()
    {
        Texture2D texture = new Texture2D(imgSize, imgSize);
        texture.filterMode = 0;
        pixelsPerCell = imgSize / gridSize;
        GeneratePoints();
        for (int i = 0; i< imgSize; ++i)
        {
            for (int j = 0; j<imgSize; ++j)
            {
                texture.SetPixel(i, j, Color.white);
            }
        }
        for (int i = 0; i < gridSize; ++i)
        {
            for (int j = 0; j < gridSize; ++j)
            {
                texture.SetPixel(pointsPositions[i,j].x, pointsPositions[i,j].y, Color.black);
            }
        }
        for (int i = 0; i < imgSize; ++i)
        {
            for (int j = 0; j < imgSize; ++j)
            {
                int gridX = i / pixelsPerCell;
                int gridY = j / pixelsPerCell;

                float nearestDistance = Mathf.Infinity;
                Vector2Int nearestPoint = new Vector2Int();

                for (int a = -1; a < 2; ++a)
                {
                    for (int b = -1; b < 2; ++b)
                    {
                        int X = gridX + a;
                        int Y = gridY + b;
                        if (X < 0 || Y < 0 || X >= gridSize || Y >= gridSize) continue;

                        float distance = Vector2Int.Distance(new Vector2Int(i, j), pointsPositions[X, Y]);
                        if (distance < nearestDistance)
                        {
                            nearestDistance = distance;
                            nearestPoint = new Vector2Int(X,Y);
                        }

                    }
                }
                texture.SetPixel(i, j, colors[nearestPoint.x, nearestPoint.y]);
            }
        }
        texture.Apply();
        image.texture = texture;
        

    }

    private void GeneratePoints()
    {
        pointsPositions = new Vector2Int[gridSize, gridSize];
        colors = new Color[gridSize, gridSize];
        for (int i = 0; i < gridSize; ++i)
        {
            for (int j = 0; j < gridSize; ++j)
            {
                pointsPositions[i, j] = new Vector2Int(i * pixelsPerCell + Random.Range(0, pixelsPerCell), j * pixelsPerCell + Random.Range(0, pixelsPerCell));
                colors[i, j] = possibleColors[Random.Range(0, possibleColors.Length)];
            }
        }

    }
}
