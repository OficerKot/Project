using UnityEngine;
using UnityEngine.EventSystems;

public class UIPickableIcon : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] string itemId;
    GameObject spawnedPlayable;
    [SerializeField] GameObject blurImage = null;


    private void Update()
    {
        if(spawnedPlayable && spawnedPlayable.GetComponent<Item>().IsPlaced())
        {
            spawnedPlayable = null;
            blurImage.SetActive(false);
        }
    }
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
                if(blurImage) blurImage.SetActive(true);
                Grab();
            }
        }
        else
        {
            GameManager.Instance.PutInHand(null);
            if(blurImage) blurImage.SetActive(false);
            Destroy(spawnedPlayable);
        }
    }

    void Grab()
    {
        spawnedPlayable = Instantiate(ItemManager.Instance.GetItemByID(itemId).prefab, transform.position, transform.rotation);
        spawnedPlayable.GetComponent<Item>().SetIsPlaced(false);
    }

  

}
