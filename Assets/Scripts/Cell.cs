using System.Collections;
using UnityEngine;


public interface ICell
{
    public int GetNeighboursCount();
    public void Highlight();
    public void NoHighlight();
    public bool IsNeighbour(Cell obj);
    public bool CheckIfFree();
    public DominoPart GetCurDomino();
    public void SetFree(bool val);
}

public class Cell : MonoBehaviour, ICell
{
    [SerializeField] bool isFree = true;

    SpriteRenderer cellSprite;
    Color previousColor;

    [SerializeField] public DominoPart curDomino;
    [SerializeField] public Cell[] neighbourCells = new Cell[10];

    int size = 0;
    
    void Start()
    {
        cellSprite = gameObject.GetComponent<SpriteRenderer>();
        previousColor = cellSprite.color;

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == 6 && NotAngular(other.transform))
        {
            neighbourCells[size++] = other.GetComponent<Cell>();
        }
    }

    public DominoPart GetCurDomino()
    {
        return curDomino;
    }
    bool NotAngular(Transform pos1)
    {
        return Mathf.Abs(pos1.position.x - transform.position.x) < 0.5f || Mathf.Abs(pos1.position.y - transform.position.y) < 0.5f;
    }
    public int GetNeighboursCount()
    {
        return size;
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
