using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

/// <summary>
/// Основной класс управления домино, обрабатывающий перетаскивание, размещение и взаимодействие с клетками.
/// </summary>
public class Domino : PauseBehaviour
{
    DominoPart part1Playable, part2Playable;
    [SerializeField] DominoData part1, part2;
    public GameObject pivot;
    bool isBeingGrabbed = false;
    Cell curCell1, curCell2;
    [SerializeField] float offsetY = 2f;

    /// <summary>
    /// Инициализирует домино с указанными данными частей.
    /// </summary>
    /// <param name="p1">Данные первой части домино.</param>
    /// <param name="p2">Данные второй части домино.</param>
    public void Initialize(DominoData p1, DominoData p2)
    {
        part1 = p1;
        part2 = p2;
        GenerateParts();
        SpawnPivot();
    }

    /// <summary>
    /// Поднимает домино для перетаскивания.
    /// </summary>
    public void PickUp()
    {
        isBeingGrabbed = true;
        LayerSorter.Instance.PutInFront(gameObject);

        if (curCell1 && curCell2)
        {
            part1Playable.ClearAllNeighbors();
            part2Playable.ClearAllNeighbors();

            ClearCellData();
        }
    }

    private void OnDestroy()
    {
        ClearCellData();
    }

    /// <summary>
    /// Проверяет, размещено ли домино на поле.
    /// </summary>
    /// <returns>True если домино размещено, false если находится в руке.</returns>
    public bool isPlaced()
    {
        return !isBeingGrabbed;
    }

    private void Update()
    {
        if (isBeingGrabbed)
        {
            Interact();
        }

        if (part1Playable && part2Playable)
        {
            CheckPartRotation();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isBeingGrabbed && collision.gameObject.layer == 6 && !EventSystem.current.IsPointerOverGameObject())
        {
            if (!collision.GetComponent<Cell>().IsFreeForDomino())
            {
                return;
            }

            ClearCellData();
            curCell1 = collision.GetComponent<Cell>();
            Cell minDistCell = FindMinDistCell();

            if (minDistCell == null)
            {
                return;
            }
            else
            {
                curCell2 = minDistCell;
                curCell1.Highlight();
                curCell2.Highlight();
                return;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isBeingGrabbed) ClearCellData();
    }

    /// <summary>
    /// Находит ближайшую подходящую клетку для размещения второй части домино.
    /// </summary>
    Cell FindMinDistCell()
    {
        float minDist = float.MaxValue;
        Cell minDistCell = null;
        foreach (Cell nearCell in curCell1.neighbourCells)
        {
            if (nearCell && CellIsOK(curCell1, nearCell))
            {
                if (minDist > GetDistance2(pivot.transform, nearCell.transform))
                {
                    minDist = GetDistance2(pivot.transform, nearCell.transform);
                    minDistCell = nearCell;
                }
            }
        }
        return minDistCell;
    }

    /// <summary>
    /// Проверяет, подходит ли клетка для размещения домино.
    /// </summary>
    bool CellIsOK(Cell cell1, Cell cell2)
    {
        if (IsSameRotationAngle(cell2.transform.position, curCell1.transform.position))
        {
            return cell2.IsFreeForDomino() && CheckCells(cell1, cell2);
        }
        return false;
    }

    /// <summary>
    /// Проверяет условия размещения домино в двух клетках.
    /// </summary>
    bool CheckCells(Cell cell1, Cell cell2)
    {
        IBreakableObject itemInCell;
        Cell[] sortedCells = GetCellsInOrder(cell2);
        Cell cellWithBiggerCoords = sortedCells[0];
        Cell cellWithLowerCoords = sortedCells[1];

        bool part1CoordsAreBigger = part1Playable.GetLocation() == Location.up || part1Playable.GetLocation() == Location.right;
        DominoPart partWithBiggerCoords = part1CoordsAreBigger ? part1Playable : part2Playable;
        DominoPart partWithLowerCoords = part1CoordsAreBigger ? part2Playable : part1Playable;

        bool image1IsOK = cellWithBiggerCoords.GetImage() == ImageEnumerator.any ||
            (partWithBiggerCoords.data.neighboursImage == cellWithBiggerCoords.GetImage() || partWithBiggerCoords.data.neighboursImage == ImageEnumerator.any);
        bool image2IsOK = cellWithLowerCoords.GetImage() == ImageEnumerator.any ||
            (partWithLowerCoords.data.neighboursImage == cellWithLowerCoords.GetImage() || partWithLowerCoords.data.neighboursImage == ImageEnumerator.any);

        bool number1IsOK = cellWithBiggerCoords.GetNumber() == 0 || (partWithBiggerCoords.data.neighboursNumber == cellWithBiggerCoords.GetNumber());
        bool number2IsOK = cellWithLowerCoords.GetNumber() == 0 || (partWithLowerCoords.data.neighboursNumber == cellWithLowerCoords.GetNumber());

        bool item1IsOK = cellWithBiggerCoords.GetCurItem() == null;
        bool item2IsOK = cellWithLowerCoords.GetCurItem() == null;

        if (!item1IsOK)
        {
            itemInCell = cellWithBiggerCoords.GetCurItem().GetComponent<IBreakableObject>();
            if (itemInCell != null)
            {
                item1IsOK = itemInCell.CanBreak(partWithBiggerCoords, partWithLowerCoords);
            }
        }
        if (!item2IsOK)
        {
            itemInCell = cellWithLowerCoords.GetCurItem().GetComponent<IBreakableObject>();
            if (itemInCell != null)
            {
                item2IsOK = itemInCell.CanBreak(partWithLowerCoords, partWithBiggerCoords);
            }
        }

        return (image1IsOK || number1IsOK) && (image2IsOK || number2IsOK) && item1IsOK && item2IsOK;
    }

    /// <summary>
    /// Сортирует две клетки по координатам.
    /// </summary>
    Cell[] GetCellsInOrder(Cell cell)
    {
        Cell[] output = new Cell[2];
        bool horizontal = Mathf.Abs(curCell1.transform.position.x - cell.transform.position.x) > 0.5f;
        bool cell1CoordsAreBigger = horizontal && (curCell1.transform.position.x > cell.transform.position.x) || !horizontal && (curCell1.transform.position.y > cell.transform.position.y);

        if (cell1CoordsAreBigger)
        {
            output[0] = curCell1;
            output[1] = cell;
        }
        else
        {
            output[0] = cell;
            output[1] = curCell1;
        }
        return output;
    }

    /// <summary>
    /// Очищает данные о текущих клетках и снимает выделение.
    /// </summary>
    void ClearCellData()
    {
        if (curCell1)
        {
            curCell1.NoHighlight();
            curCell1.SetFree(true);
            if (curCell1.GetCurDomino()) curCell1.SetFree();
            curCell1 = null;
        }
        if (curCell2)
        {
            curCell2.NoHighlight();
            curCell2.SetFree(true);
            if (curCell2.GetCurDomino()) curCell2.SetFree();
            curCell2 = null;
        }

        part1Playable.ChangeIsBeingPlacedFlag(false);
        part2Playable.ChangeIsBeingPlacedFlag(false);
    }

    /// <summary>
    /// Вычисляет квадрат расстояния между двумя трансформами.
    /// </summary>
    float GetDistance2(Transform pos1, Transform pos2)
    {
        float dist = Mathf.Pow(pos1.position.x - pos2.position.x, 2) + Mathf.Pow(pos1.position.y - pos2.position.y, 2);
        return dist;
    }

    /// <summary>
    /// Размещает домино в выбранных клетках.
    /// </summary>
    void PutInTheCells()
    {
        isBeingGrabbed = false;
        GameManager.Instance.PutInHand(null);

        BreakItemsInCells();
        TakeFreeSpace();
        LayerSorter.Instance.PutBack(gameObject, SortingOrder.domino);

        Collider2D collider1 = curCell1.GetComponent<BoxCollider2D>();
        Collider2D collider2 = curCell2.GetComponent<BoxCollider2D>();

        TeleportToCells(collider1.transform, collider2.transform);
        AddToCells(part1Playable, part2Playable);

        AudioManager.Instance.Boneplace();
        EnemyManager.Instance.MakeStep();
    }

    /// <summary>
    /// Привязывает части домино к соответствующим клеткам.
    /// </summary>
    
    void AddToCells(DominoPart d1, DominoPart d2)
    {
        if (GetDistance2(d1.transform, curCell1.transform) > GetDistance2(d2.transform, curCell1.transform))
        {
            curCell1.SetCurDomino(d2);
            curCell2.SetCurDomino(d1);
        }
        else
        {
            curCell1.SetCurDomino(d1);
            curCell2.SetCurDomino(d2);
        }
    }

    /// <summary>
    /// Телепортирует домино в центр между двумя клетками.
    /// </summary>
    void TeleportToCells(Transform pos1, Transform pos2)
    {
        Vector2 targetPos = (pos1.position + pos2.transform.position) / 2f;
        pivot.transform.position = targetPos;
    }

    /// <summary>
    /// Занимает клетки.
    /// </summary>
    void TakeFreeSpace()
    {
        curCell1.SetFree(false);
        curCell2.SetFree(false);
        part1Playable.ChangeIsBeingPlacedFlag(true);
        part2Playable.ChangeIsBeingPlacedFlag(true);
    }

    /// <summary>
    /// Уничтожает объекты в клетках при размещении домино.
    /// </summary>
    void BreakItemsInCells()
    {
        if (curCell1.GetCurItem())
        {
            IBreakableObject item1 = curCell1.GetCurItem().GetComponent<IBreakableObject>();
            if (item1 != null) item1.Break();
        }

        if (curCell2.GetCurItem())
        {
            IBreakableObject item2 = curCell2.GetCurItem().GetComponent<IBreakableObject>();
            if (item2 != null) item2.Break();
        }
    }

    /// <summary>
    /// Проверяет совпадение направления домино с направлением 2х клеток.
    /// </summary>
    bool IsSameRotationAngle(Vector3 pos1, Vector3 pos2)
    {
        bool cellsAreHorizontal = Mathf.Abs(pos1.x - pos2.x) > Mathf.Abs(pos1.y - pos2.y);
        float dominoAngle = NormalizeAngle(pivot.transform.eulerAngles.z);
        bool dominoIsHorizontal = (dominoAngle >= 45f && dominoAngle <= 135f);
        return cellsAreHorizontal == dominoIsHorizontal;
    }

    /// <summary>
    /// Нормализует угол в диапазон 0-180 градусов.
    /// </summary>
    float NormalizeAngle(float angle)
    {
        angle %= 360f;
        if (angle < 0) angle += 360f;

        if (angle > 180f) angle = 360f - angle;
        return angle;
    }

    /// <summary>
    /// Обрабатывает взаимодействие с домино (перемещение, вращение, размещение).
    /// </summary>
    void Interact()
    {
        Move();
        if (Input.GetKeyDown(KeyCode.A))
        {
            Rotate(90);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Rotate(-90);
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (curCell1 && curCell2 && !EventSystem.current.IsPointerOverGameObject())
            {
                curCell1.NoHighlight();
                curCell2.NoHighlight();
                PutInTheCells();
            }
        }
    }

    /// <summary>
    /// Вращает домино на указанный угол.
    /// </summary>
    /// <param name="degree">Угол вращения в градусах.</param>
    void Rotate(float degree = 90)
    {
        if (!part1Playable || !part2Playable)
        {
            Debug.Log("Generate first");
            return;
        }
        pivot.transform.Rotate(0, 0, degree);
    }

    /// <summary>
    /// Перемещает домино к позиции курсора мыши.
    /// </summary>
    void Move()
    {
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetPos.z = 0;

        pivot.transform.position = Vector3.Lerp(pivot.transform.position, targetPos, 1);
    }

    /// <summary>
    /// Генерирует части домино на сцене.
    /// </summary>
    void GenerateParts()
    {
        SpawnParts();
        SpawnPivot();
    }

    /// <summary>
    /// Создает игровые объекты для частей домино.
    /// </summary>
    void SpawnParts()
    {
        if (part2Playable != null) Destroy(part1Playable);
        if (part1Playable != null) Destroy(part2Playable);

        part1Playable = Instantiate(part1.prefab, gameObject.transform.position + new Vector3(0, offsetY), gameObject.transform.rotation, transform).GetComponent<DominoPart>();
        part2Playable = Instantiate(part2.prefab, gameObject.transform.position, gameObject.transform.rotation, transform).GetComponent<DominoPart>();
        part1Playable.GetComponent<DominoPart>().data = part1;
        part2Playable.GetComponent<DominoPart>().data = part2;
        part1Playable.ChangeIsBeingPlacedFlag(false);
        part2Playable.ChangeIsBeingPlacedFlag(false);

        CheckPartRotation();
    }

    /// <summary>
    /// Создает точку вращения (pivot) для домино.
    /// </summary>
    void SpawnPivot()
    {
        if (pivot != null) return;

        BoxCollider2D collider1 = part1Playable.GetComponent<BoxCollider2D>();
        BoxCollider2D collider2 = part2Playable.GetComponent<BoxCollider2D>();

        Vector2 centerPosition = (part1Playable.transform.position + part2Playable.transform.position) / 2f;

        pivot = new GameObject("Pivot");
        pivot.transform.position = centerPosition;
        pivot.transform.rotation = transform.rotation;

        transform.SetParent(pivot.transform);
    }

    /// <summary>
    /// Обновляет расположение частей домино (верх/низ/лево/право).
    /// </summary>
    void CheckPartRotation()
    {
        bool areHorizontal = Mathf.Abs(part1Playable.transform.position.y - part2Playable.transform.position.y) < 0.5f;

        if (areHorizontal)
        {
            if (part1Playable.transform.position.x > part2Playable.transform.position.x)
            {
                part1Playable.ChangeLocation(Location.right);
                part2Playable.ChangeLocation(Location.left);
            }
            if (part1Playable.transform.position.x < part2Playable.transform.position.x)
            {
                part1Playable.ChangeLocation(Location.left);
                part2Playable.ChangeLocation(Location.right);
            }
        }
        else
        {
            if (part1Playable.transform.position.y > part2Playable.transform.position.y)
            {
                part1Playable.ChangeLocation(Location.up);
                part2Playable.ChangeLocation(Location.down);
            }
            if (part1Playable.transform.position.y < part2Playable.transform.position.y)
            {
                part1Playable.ChangeLocation(Location.down);
                part2Playable.ChangeLocation(Location.up);
            }
        }
    }
}