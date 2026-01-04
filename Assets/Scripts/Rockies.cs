using UnityEngine;

public class Rockies : ResourceSource
{
    RockiesNumberGenerator generator;

    private void Awake()
    {
        generator = GetComponent<RockiesNumberGenerator>();
        generator.Generate();
    }
    public override void Pick()
    {
        Inventory.Instance.AddItem(resource, generator.GetCount());
        curCell.SetFree();
        Destroy(gameObject);
    }

    public override void PutInCell(Cell cell)
    {
        base.PutInCell(cell);
        curCell.SetNumber(generator.GetCount());
    }

    public override bool CanBreak(DominoPart cur, DominoPart other)
    {
        return cur.data.number == generator.GetCount();
    }
}
