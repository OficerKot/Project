using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Tree : MonoBehaviour
{
    private ObstacleSnap obs;
    private bool choppable = false;

    private void Awake()
    {
        obs = this.GetComponent<ObstacleSnap>(); 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 9 && CheckAxeInInventory())
        {
            obs.curCell.SetFree(true);
            choppable = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == 9 && CheckAxeInInventory())
        {
            obs.curCell.SetFree(false);
            choppable = false;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (choppable)
            {
                Destroy(this.gameObject);
                return;
            }
        }
    }

    //private void OnMouseDown()
    //{
    //    Debug.Log("it was supposed to destroy it... Hmm");
    //    if (choppable)
    //    {
    //        Destroy(this.gameObject);
    //    }
    //}

    bool CheckAxeInInventory()
    {
        Dictionary<string, int> inv = Inventory.Instance.GetCurItems();
        if (inv.ContainsKey("axe"))
        {
            return true;
        }
        return false;
    }
}
