using System;
using System.Collections;
using UnityEngine;

public class PlayerStopper : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            CharacterMovement character = collision.gameObject.GetComponent<CharacterMovement>();
            StartCoroutine(StopPlayer(character));
        }
    }
    IEnumerator StopPlayer(CharacterMovement character)
    {
        character.ControlMovement(false);
        yield return StartCoroutine(WaitForMovement());
        character.ControlMovement(true);
        GetComponent<ResourceSource>().Pick();
    }
    IEnumerator WaitForMovement()
    {
        bool moved = false;
        Action<bool> handler = (val) => moved = true;
        CharacterMovement.OnMovementAttempted += handler;
        yield return new WaitUntil(() => moved);
    }
}
