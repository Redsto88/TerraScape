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
        currentMenu.SetActive(false);
        currentMenu = GameManager.Tool.ParamMenu;
        currentMenu.SetActive(true);
    }
}
