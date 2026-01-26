using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Breakable : MonoBehaviour
{
    [SerializeField] float health = 100;
    float startHealth;
    [SerializeField] List<DominoProtectionSource> protectors = new List<DominoProtectionSource> ();
    [SerializeField] SpriteRenderer healthStateObject;
    [SerializeField] List<Sprite> healhStates;
    private void Start()
    {
        startHealth = health;
    }

    public void AddProtection(DominoProtectionSource source)
    {
        protectors.Add(source);
    }
    public void RemoveProtection(DominoProtectionSource source)
    {
        protectors.Remove(source);
    }
    public void TryToBreak(float hp)
    {
        if (protectors.Count > 0)
        {
            foreach (var protector in protectors)
            {
                if(protector.TryToProtect())
                {
                    Debug.Log("Is under protection!");
                    return;
                }
            }
  
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
