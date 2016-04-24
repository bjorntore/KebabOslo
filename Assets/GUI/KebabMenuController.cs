using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class KebabMenuController : MonoBehaviour
{
    public GameObject MenuItemPanelPrefab;
    public Transform kebabMenuList;
    public Button AddMenuItemButton;
    public Button SaveButton;
    public Button DiscardButton;

    private KebabMenu kebabMenu;
    private List<MenuItemController> menuItemControls = new List<MenuItemController>();

    public void SetupKebabMenu(KebabMenu kebabMenu)
    {
        if (MenuItemPanelPrefab == null)
            throw new Exception("MenuItemPanelPrefab is missing! Configure for KebabMenuController.");

        this.kebabMenu = kebabMenu;
        foreach (MenuItem menuItem in this.kebabMenu.menuItems)
        {
            CreateMenuItemPanel(menuItem);
        }

        AddMenuItemButton.onClick.RemoveAllListeners();
        AddMenuItemButton.onClick.AddListener(AddMenuItem);

        SaveButton.onClick.RemoveAllListeners();
        SaveButton.onClick.AddListener(SaveMenu);

        DiscardButton.onClick.RemoveAllListeners();
        DiscardButton.onClick.AddListener(DiscardChanges);
    }

    private void CreateMenuItemPanel(MenuItem menuItem)
    {
        GameObject menuItemGO = (GameObject)Instantiate(MenuItemPanelPrefab);
        menuItemGO.transform.SetParent(kebabMenuList);

        MenuItemController menuItemController = menuItemGO.GetComponent<MenuItemController>();
        menuItemController.SetMenuItem(menuItem);
        menuItemControls.Add(menuItemController);
    }

    public void AddMenuItem()
    {
        if (kebabMenu.CanCreateMoreMenuItems())
        {
            var newMenuItem = kebabMenu.AddMenuItem(new MenuItem());
            if (newMenuItem == null)
                Debug.LogError("Should not try to create more menu items!");
            else {
                CreateMenuItemPanel(newMenuItem);
            }
        }

        if (!kebabMenu.CanCreateMoreMenuItems())
            AddMenuItemButton.interactable = false; 
    }

    public void SaveMenu()
    {
        menuItemControls.ForEach(x => x.SaveChanges());
    }

    public void DiscardChanges()
    {
        menuItemControls.ForEach(x => x.DiscardChanges());
    }
}
