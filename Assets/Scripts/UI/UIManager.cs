using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    Menu openedMenu = null;
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

    public void ToggleMenu(Menu menu)
    {

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