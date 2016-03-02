using UnityEngine;
using System.Collections;
using System.Linq;

public abstract class Dialog : MonoBehaviour {

    public bool IsAnyDialogOpen()
    {
        return GameObject.FindGameObjectsWithTag("Dialog").Any(g => g.activeSelf);
    }

}
