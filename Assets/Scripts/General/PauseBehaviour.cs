using UnityEngine;

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
