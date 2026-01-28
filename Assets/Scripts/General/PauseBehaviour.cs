using UnityEngine;

/// <summary>
/// Отвечает за поведение объекта при игровой паузе. По умолчанию деактивирует коллайдер для исключения взаимодействия с объектом.
/// </summary>
public class PauseBehaviour : MonoBehaviour
{
    void OnEnable()
    {
        GameManager.OnGamePaused += OnGamePaused;
    }

    void OnDisable()
    {
        GameManager.OnGamePaused -= OnGamePaused;
    }

    public virtual void OnGamePaused(bool isGamePaused)
    {
        GetComponent<Collider2D>().enabled = !isGamePaused;
    }
}
