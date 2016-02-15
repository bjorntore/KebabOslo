using UnityEngine;
using System.Collections;
using System;

public class TileController : MonoBehaviour, IClickable {

    public Tile tile;

    public void Click()
    {
        Debug.Log("Clicked " + tile.ToString());
        if (tile.CanBuildOn())
        {
            WorldController wc = GameObject.FindObjectOfType<WorldController>();
            wc.AddAndSpawnBuilding(new KebabBuilding(tile.x, tile.z), tile);
        }
    }

}
