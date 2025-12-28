using System.Collections.Generic;
using UnityEngine;

public class UIManager : PauseBehaviour
{
    Menu openedMenu = null;
    bool isActive = true;
    [SerializeField] Menu sigilsMenu, pauseMenu, craftMenu;
    [SerializeField] KeyCode sigilsKey, pauseKey, craftKey;

    void Update()
    {
        if (Input.GetKeyDown(sigilsKey))
        {
            ToggleMenu(sigilsMenu);
        }
        if(Input.GetKeyDown(craftKey))
        {
            ToggleMenu(craftMenu);
        }
        if(Input.GetKeyDown(pauseKey))
        {
            ToggleMenu(pauseMenu);
        }
    }
    public override void OnGamePaused(bool isGamePaused)
    {
        isActive = !isGamePaused;
    }

    public void ToggleMenu(Menu menu)
    {
        if (!isActive && GameManager.Instance.gameEnd) return;
        if (openedMenu == menu)
        {
            menu.Close();
            openedMenu = null;
        }
        else if (openedMenu == null)
        {
            menu.Open();
            openedMenu = menu;
        }
    }
}