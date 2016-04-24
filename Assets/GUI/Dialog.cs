using UnityEngine;
using System.Collections;
using System.Linq;

public abstract class Dialog : MonoBehaviour {

    public GameObject panel;
    public static bool KeyboardLock { get; set; }

    public void Display(bool lockKeyboard = false)
    {
        if (IsAnyDialogOpen())
            return;

        panel.SetActive(true);
        KeyboardLock = lockKeyboard;
    }

    public bool IsAnyDialogOpen()
    {
        return GameObject.FindGameObjectsWithTag("Dialog").Any(g => g.activeSelf);
    }

    public void ClosePanel()
    {
        KeyboardLock = false;
        panel.SetActive(false);
    }
}
