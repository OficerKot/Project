using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Компонент для разрушаемых объектов с системой защиты и визуализацией состояния здоровья.
/// </summary>
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

    /// <summary>
    /// Добавляет источник защиты к объекту.
    /// </summary>
    /// <param name="source">Источник защиты для добавления.</param>
    public void AddProtection(DominoProtectionSource source)
    {
        protectors.Add(source);
    }

    /// <summary>
    /// Удаляет источник защиты из объекта.
    /// </summary>
    /// <param name="source">Источник защиты для удаления.</param>
    public void RemoveProtection(DominoProtectionSource source)
    {
        protectors.Remove(source);
    }

    /// <summary>
    /// Попытка нанести урон объекту с проверкой защиты.
    /// </summary>
    /// <param name="hp">Количество наносимого урона.</param>
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

    /// <summary>
    /// Уничтожает игровой объект.
    /// </summary>
    void RemoveFromGame()
    {
        Destroy(gameObject);
    }
}
