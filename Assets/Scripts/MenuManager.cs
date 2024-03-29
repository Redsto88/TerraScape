using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject currentMenu;

    public void ChangeMenu(GameObject newMenu)
    {
        currentMenu.SetActive(false);
        currentMenu = newMenu;
        currentMenu.SetActive(true);
    }

    public void ToolMenu()
    {
        ChangeMenu(GameManager.Tool.ParamMenu);
    }

    public void ToggleVisibility(GameObject obj)
    {
        obj.SetActive(!obj.activeSelf);
    }
}
