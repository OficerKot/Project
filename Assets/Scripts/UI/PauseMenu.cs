using UnityEngine;

/// <summary>
/// Меню паузы. Приостанавливает игру при активации.
/// </summary>
public class PauseMenu : Menu
{
    [SerializeField] GameObject menu;

    /// <summary>
    /// Открытие меню, игра устанавливается на паузу.
    /// </summary>
    public override void Open()
    {
        menu.SetActive(true);
        GameManager.Instance.Pause();
    }

    /// <summary>
    /// Закрытие меню, возообновление игры.
    /// </summary>
    public override void Close()
    {
        menu.SetActive(false);
        GameManager.Instance.Pause();
    }
}
