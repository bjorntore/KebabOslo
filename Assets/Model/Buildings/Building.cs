using UnityEngine;
using System.Collections;
using System;

public abstract class Building
{

    public Tile tile;

    public override string ToString()
    {
        return "Building_" + GetType() + "_" + tile.x + "_" + tile.z;
    }

}
