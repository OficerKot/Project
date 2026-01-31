using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Останавливает игрока при входе в триггер и возобновляет движение после попытки перемещения.
/// Вынуждает делать более одного шага для выхода с триггера.
/// </summary>
public class PlayerStopper : MonoBehaviour
{
    bool started = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !started)
        {
            started = true;
            CharacterMovement character = collision.gameObject.GetComponent<CharacterMovement>();
            StartCoroutine(StopPlayer(character));
        }
    }

    /// <summary>
    /// Корутина для остановки и последующего возобновления управления игроком.
    /// </summary>
    /// <param name="character">Компонент движения персонажа.</param>
    IEnumerator StopPlayer(CharacterMovement character)
    {
        character.ControlMovement(false);
        yield return StartCoroutine(WaitForMovement());
        character.ControlMovement(true);
        if (GetComponent<ResourceSource>() != null)
        {
            GetComponent<ResourceSource>().Pick();
        }
    }

    /// <summary>
    /// Корутина ожидания попытки движения игрока.
    /// </summary>
    IEnumerator WaitForMovement()
    {
        bool moved = false;
        Action<bool> handler = (val) => moved = true;
        CharacterMovement.OnMovementAttempted += handler;
        yield return new WaitUntil(() => moved);
    }
}