using UnityEngine;
using System.Collections;
using System;
using System.Linq;
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

        ResetMenuItemList();

        this.kebabMenu = kebabMenu;
        SetAddMenuItemButtonInteractability();

        foreach (MenuItem menuItem in this.kebabMenu.menuItems)
        {
            var menuItemController = CreateMenuItemPanel(menuItem);
            menuItemController.menuItemIsSaved = true;
        }

        AddMenuItemButton.onClick.RemoveAllListeners();
        AddMenuItemButton.onClick.AddListener(AddMenuItem);

        SaveButton.onClick.RemoveAllListeners();
        SaveButton.onClick.AddListener(SaveMenu);

        DiscardButton.onClick.RemoveAllListeners();
        DiscardButton.onClick.AddListener(DiscardChanges);
    }

    private void SetAddMenuItemButtonInteractability()
    {
        if (kebabMenu.CanCreateMoreMenuItems())
            AddMenuItemButton.interactable = true;
        else AddMenuItemButton.interactable = false;
    }

    private MenuItemController CreateMenuItemPanel(MenuItem menuItem)
    {
        GameObject menuItemGO = (GameObject)Instantiate(MenuItemPanelPrefab);
        menuItemGO.transform.SetParent(kebabMenuList);

        MenuItemController menuItemController = menuItemGO.GetComponent<MenuItemController>();
        menuItemController.SetMenuItem(menuItem);
        menuItemControls.Add(menuItemController);

        return menuItemController;
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

        SetAddMenuItemButtonInteractability();
    }

    public void SaveMenu()
    {
        menuItemControls.ForEach(x => x.SaveChanges());
        if (kebabMenu.CanCreateMoreMenuItems()) AddMenuItemButton.interactable = true;
    }

    public void DiscardChanges()
    {
        List<MenuItemController> menuItemControllersToDelete = new List<MenuItemController>();
        menuItemControls.ForEach(x =>
        {
            if (!x.menuItem.IsActive)
            {
                Destroy(x.gameObject);
                menuItemControllersToDelete.Add(x);
            }
            else
                x.DiscardChanges();
        });


        menuItemControllersToDelete.ForEach(x =>
        {
            kebabMenu.RemoveMenuItem(x.menuItem);
            menuItemControls.Remove(x);
        });

        if (kebabMenu.CanCreateMoreMenuItems()) AddMenuItemButton.interactable = true;
        Dialog.KeyboardLockOff();
    }

    private void ResetMenuItemList()
    {
        menuItemControls.ForEach(x => Destroy(x.gameObject));
        menuItemControls = new List<MenuItemController>();
    }
}
