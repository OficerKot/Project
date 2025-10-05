using System.Collections;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] bool isFree = true;
    SpriteRenderer cellSprite;
    Color previousColor;

    [SerializeField] public Cell[] neighbourCells = new Cell[10];
    int size = 0;
    
    void Start()
    {
        cellSprite = gameObject.GetComponent<SpriteRenderer>();
        previousColor = cellSprite.color;

    }

    public int GetNeighboursCount()
    {
        return size;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == 6)
        {
            neighbourCells[size++] = other.GetComponent<Cell>();
        }
    }

    public void Highlight()
    {
        cellSprite.color = Color.aliceBlue;
    }

    public void NoHighlight()
    {
        cellSprite.color = previousColor;
    }
    public bool IsNeighbour(Cell obj)
    {
        if (size == 0) return false;
        foreach(Cell cell in neighbourCells)
        {
            if (cell == obj) return true;
        }
        return false;
    }
    

    public bool CheckIfFree()
    {
        return isFree;
    }

    public void SetFree(bool val)
    {
        isFree = val;
    }



}
