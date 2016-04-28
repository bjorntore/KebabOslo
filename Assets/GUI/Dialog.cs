using UnityEngine;
using System.Collections;
using System.Linq;

public abstract class Dialog : MonoBehaviour {

    public GameObject panel;
    public static bool KeyboardLock { get; set; }

    public void Display()
    {
        if (IsAnyDialogOpen())
            return;

        panel.SetActive(true);
    }

    public bool IsAnyDialogOpen()
    {
        return GameObject.FindGameObjectsWithTag("Dialog").Any(g => g.activeSelf);
    }

    public void ClosePanel()
    {
        panel.SetActive(false);
    }

    public static void KeyboardLockOn(string  arg0 = "")
    {
        KeyboardLock = true;
    }

    public static void KeyboardLockOff(string arg0 = "")
    {
        KeyboardLock = false;
    }
}
