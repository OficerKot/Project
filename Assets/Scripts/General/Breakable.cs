using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Breakable : MonoBehaviour
{
    [SerializeField] float health = 100;
    float startHealth;
    [SerializeField] int protectorsCount = 0;
    [SerializeField] SpriteRenderer healthStateObject;
    [SerializeField] List<Sprite> healhStates;
    private void Start()
    {
        startHealth = health;
    }

    public void AddProtection()
    {
        protectorsCount++;
    }
    public void RemoveProtection()
    {
        protectorsCount--;
    }
    public void TryToBreak(float hp)
    {
        if (protectorsCount > 0)
        {
            Debug.Log("Is under protection!");
            return;
        }

        health -= hp;
        if (health <= 0)
        {
            RemoveFromGame();
        }
        int k = (int)startHealth / healhStates.Count;
        healthStateObject.sprite = healhStates[(int)health / k];
    }
    void RemoveFromGame()
    {
        Destroy(gameObject);
    }
}
