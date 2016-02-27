using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Tile
{
    public int x;
    public int z;
    public TileType type;
    public bool isWorldEdge = false;

    public int propertyValue = Settings.Tile_BasePropertyValue;

    public Tile adjacentRoadTile;

    public Tile(int x, int z, TileType type)
    {
        this.x = x;
        this.z = z;
        this.type = type;
    }

    public override string ToString()
    {
        return "Tile_" + Enum.GetName(typeof(TileType), type) + "_" + x + "_" + z;
    }

}

public enum TileType
{
    Empty = 1,
    Road = 2,
    Buildable = 3,
    Occupied = 4
}