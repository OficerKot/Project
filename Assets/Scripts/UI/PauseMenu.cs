using UnityEngine;

public class PauseMenu : Menu
{
    [SerializeField] GameObject menu;
    public override void Open()
    {
        menu.SetActive(true);
        GameManager.Instance.Pause();
    }
    public override void Close()
    {
        menu.SetActive(false);
        GameManager.Instance.Pause();
    }
}
