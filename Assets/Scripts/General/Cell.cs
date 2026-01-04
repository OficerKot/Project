using System;
using System.Collections.Generic;
using UnityEngine;


public interface ICell
{
    public void Highlight();
    public void NoHighlight();
    public bool IsNeighbour(Cell obj);
    public bool CheckIfFree();
    public DominoPart GetCurDomino();
    public void SetFree(bool val);
    public void SetFree();
}

public interface IBreakableObject
{
    public bool CanBreak(DominoPart cur, DominoPart other);
    public void Break();
}
public class Cell : MonoBehaviour, ICell
{
    [SerializeField] bool isFree = true;
    SpriteRenderer cellSprite;
    Color previousColor;
    [SerializeField] DominoPart curDomino;
    [SerializeField] GameObject curItem;
    [SerializeField] ImageEnumerator image = ImageEnumerator.any;
    [SerializeField] int number = 0;
    [SerializeField] public  List<Cell> neighbourCells = new List<Cell>();
    public event Action<Cell> OnDominoPlaced;

    void Start()
    {
        curDomino = null;
        number = 0;
        cellSprite = gameObject.GetComponent<SpriteRenderer>();
        previousColor = cellSprite.color;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 6 && NotAngular(other.transform))
        {
            neighbourCells.Add(other.GetComponent<Cell>());
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (curDomino)
        {
            SetImageToAllNeighbours();
        }

    }
    public DominoPart GetCurDomino()
    {
        return curDomino;
    }

    public GameObject GetCurItem()
    {
        return curItem;
    }
    public void SetCurDomino(DominoPart domino)
    {
        curDomino = domino;
        image = domino.data.image;
        number = domino.data.number;
        if(OnDominoPlaced != null) OnDominoPlaced.Invoke(this);
    }

    public void SetCurItem(GameObject i)
    {
        curItem = i;
    }

    public void SetFree()
    {
        isFree = true;
        curDomino = null;
        curItem = null;
        UnsetImageToAllNeighbours();

        image = ImageEnumerator.any;
        number = 0;

    }
    bool NotAngular(Transform pos1)
    {
        return Mathf.Abs(pos1.position.x - transform.position.x) < 0.5f || Mathf.Abs(pos1.position.y - transform.position.y) < 0.5f;
    }

    void SetImageToAllNeighbours()
    {
        foreach (Cell neighbour in neighbourCells)
        {
            if (neighbour && neighbour.GetImage() == ImageEnumerator.any)
            {
                neighbour.SetImage(curDomino.data.neighboursImage);
                neighbour.SetNumber(number);
            }
        }
    }

    void UnsetImageToAllNeighbours()
    {
        foreach (Cell neighbour in neighbourCells)
        {
            if (neighbour && neighbour.GetImage() == image && !neighbour.GetCurDomino())
            {
                neighbour.SetImage(ImageEnumerator.any);
                neighbour.SetNumber(0);
            }
        }
    }
    public void SetImage(ImageEnumerator i)
    {
        image = i;
    }

    public ImageEnumerator GetImage()
    {
        return image;
    }

    public int GetNumber()
    {
        return number;
    }

    public void SetNumber(int n)
    {
        number = n;
    }
    public void Highlight()
    {
        cellSprite.color = Color.aliceBlue;
    }

    public void NoHighlight()
    {
        if (cellSprite)
        {
            cellSprite.color = previousColor;
        }
    }
    public bool IsNeighbour(Cell obj)
    {
        foreach (Cell cell in neighbourCells)
        {
            if (cell && cell == obj) return true;
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
