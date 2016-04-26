using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class KebabMenu
{
    public List<MenuItem> menuItems = new List<MenuItem>();

    public KebabMenu()
    {
        menuItems.Add(new MenuItem() { IsActive = true });
    }

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

    public void SetAllMenuItemsActive()
    {
        menuItems.ForEach(x => x.IsActive = true);
    }

    public void RemoveAllInactiveMenuItems()
    {
        menuItems.RemoveAll(x => !x.IsActive);
    }

    public bool CanCreateMoreMenuItems()
    {
        return menuItems.Count < 5;
    }
}
