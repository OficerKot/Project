using UnityEngine;
using UnityEngine.EventSystems;

public class UIPickableIcon : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] string itemId;
    GameObject spawnedPlayable;
    
    public string GetID()
    {
        return itemId;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (spawnedPlayable == null)
        {
            if (GameManager.Instance.WhatInHand() == null)
            {
                GameManager.Instance.PutInHand(gameObject);
                Grab();
            }
        }
        else
        {
            GameManager.Instance.PutInHand(null);
            Destroy(spawnedPlayable);
        }
    }

    void Grab()
    {
        spawnedPlayable = Instantiate(ItemManager.Instance.GetItemByID(itemId).prefab, transform.position, transform.rotation);
        spawnedPlayable.GetComponent<Item>().SetIsPlaced(false);

        // спавн предмета
    }

}
