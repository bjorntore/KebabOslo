using System;
using System.Collections.Generic;

[Serializable]
public class KebabMenu
{
    public readonly List<MenuItem> menuItems = new List<MenuItem>() { new MenuItem() };

    public void AddMenuItem(MenuItem newItem)
    {
        /* IF OK, add item. */
        menuItems.Add(newItem);
    }

    public void RemoveMenuItem(MenuItem menuItem)
    {
        if(menuItem != null)
            menuItems.Remove(menuItem);
    }
}
