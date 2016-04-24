using System;
using System.Collections.Generic;

[Serializable]
public class KebabMenu
{
    public readonly List<MenuItem> menuItems = new List<MenuItem>() { new MenuItem() };

    public MenuItem AddMenuItem(MenuItem newItem)
    {
        if (!CanCreateMoreMenuItems()) { 
            return null;
        }

        menuItems.Add(newItem);
        return newItem;
    }

    public void RemoveMenuItem(MenuItem menuItem)
    {
        if(menuItem != null)
            menuItems.Remove(menuItem);
    }

    public bool CanCreateMoreMenuItems()
    {
        return menuItems.Count < 5;
    }
}
