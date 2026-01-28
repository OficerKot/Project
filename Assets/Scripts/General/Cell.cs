using System;
using System.Collections.Generic;
using UnityEngine;


public interface ICell
{
    public void Highlight();
    public void NoHighlight();
    public bool IsNeighbour(Cell obj);
    public bool IsFreeForDomino();
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
    [SerializeField] bool isFreeForDomino = true;
    SpriteRenderer cellSprite;
    Color previousColor;
    [SerializeField] DominoPart curDomino;
    [SerializeField] GameObject curItem;
    [SerializeField] ImageEnumerator image = ImageEnumerator.any;
    [SerializeField] int number = 0;
    [SerializeField] public List<Cell> neighbourCells = new List<Cell>();
    public event Action<Cell> OnDuplicationAllowed;
    public int duplicationBlockers = 0;

    void Start()
    {
        CheckForDuplications();
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
        CheckForDuplications();
        ClearThePond();
    }

    public void SetCurItem(GameObject i)
    {
        CheckForDuplications();
        curItem = i;
        if (i == null)
        {
            UnsetImageToAllNeighbours();
            image = ImageEnumerator.any;
            number = 0;
            return;
        }

    }

    public void SetFree()
    {
        isFreeForDomino = true;
        curDomino = null;
        curItem = null;
        UnsetImageToAllNeighbours();
        image = ImageEnumerator.any;
        number = 0;
        CheckForDuplications();
        SmudgeThePond();
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

    public bool IsFreeForDomino()
    {
        return isFreeForDomino;
    }

    public bool IsFree()
    {
        return curDomino == null && curItem == null;
    }
    public void SetFree(bool val)
    {
        isFreeForDomino = val;
    }

    public bool CheckDuplicationAllowed()
    {
        if (curItem == null && curDomino != null && duplicationBlockers == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void CheckForDuplications()
    {
        if(CheckDuplicationAllowed()) OnDuplicationAllowed?.Invoke(this);
    }
    public void AddDuplicationBlocker()
    {
        duplicationBlockers++;
        CheckForDuplications();
    }

    public void RemoveDuplicationBlocker()
    {
        duplicationBlockers = Mathf.Max(0, duplicationBlockers - 1);
        CheckForDuplications();
    }
    void ClearThePond()
    {
        if (Physics2D.OverlapCircle(this.transform.position, .01f, LayerMask.GetMask("Pond")))
        {
            Collider2D pondCol = Physics2D.OverlapCircle(this.transform.position, .01f, LayerMask.GetMask("Pond"));

            Color alpha = pondCol.gameObject.GetComponent<SpriteRenderer>().color;
            Debug.Log(pondCol.gameObject);
            alpha.a = 0.5f;
            pondCol.gameObject.GetComponent<SpriteRenderer>().color = alpha;
        }
    }
    void SmudgeThePond()
    {
        if (Physics2D.OverlapCircle(this.transform.position, .01f, LayerMask.GetMask("Pond")))
        {
            Collider2D pondCol = Physics2D.OverlapCircle(this.transform.position, .01f, LayerMask.GetMask("Pond"));

            Color alpha = pondCol.gameObject.GetComponent<SpriteRenderer>().color;
            Debug.Log(pondCol.gameObject);
            alpha.a = 1f;
            pondCol.gameObject.GetComponent<SpriteRenderer>().color = alpha;
        }
    }
}
