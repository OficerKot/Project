using System.Collections.Generic;
using System.Linq;
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

public class Cell : MonoBehaviour, ICell
{
    [SerializeField] bool isFree = true;

    SpriteRenderer cellSprite;
    Color previousColor;

    // попробовать заменить на GameObject curObject, домино через ScriptableObject!!!
    [SerializeField] public DominoPart curDomino;
    [SerializeField] public Item curItem;

    [SerializeField] Image image = Image.any;
    [SerializeField] int number = 0;
    // ------------------------------------------------------------------------------
    [SerializeField] Cell curCell;
    [SerializeField] public  List<Cell> neighbourCells = new List<Cell>();

    void Start()
    {
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

    public Item GetCurItem()
    {
        return curItem;
    }
    public void SetCurDomino(DominoPart domino)
    {
        curDomino = domino;
        image = domino.GetImage();
        number = domino.GetNumber();
    }

    public void SetCurItem(Item i)
    {
        curItem = i;
    }

    public void SetFree()
    {
        isFree = true;
        curDomino = null;
        curItem = null;
        UnsetUmageToAllNeighbours();

        image = Image.any;
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
            if (neighbour && neighbour.GetImage() == Image.any)
            {
                neighbour.SetImage(image);
                neighbour.SetNumber(number);
            }
        }
    }

    void UnsetUmageToAllNeighbours()
    {
        foreach (Cell neighbour in neighbourCells)
        {
            if (neighbour && neighbour.GetImage() == image && !neighbour.GetCurDomino())
            {
                neighbour.SetImage(Image.any);
                neighbour.SetNumber(0);
            }
        }
    }
    public void SetImage(Image i)
    {
        image = i;
    }

    public Image GetImage()
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
        cellSprite.color = previousColor;
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
