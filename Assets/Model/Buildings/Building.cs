using UnityEngine;
using System.Collections;

public abstract class Building
{

    public Tile tile;

    public Building(Tile tile)
    {
        this.tile = tile;
    }

    public override string ToString()
    {
        return "Building_" + GetType() + "_" + tile.x + "_" + tile.z;
    }

}
