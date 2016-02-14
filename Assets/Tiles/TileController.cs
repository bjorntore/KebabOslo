using UnityEngine;
using System.Collections;
using System;

public class TileController : MonoBehaviour, IClickable {

    public Tile tile;

    public void Click()
    {
        Debug.Log("Clicked " + tile.ToString());
    }
}
