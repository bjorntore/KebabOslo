﻿using UnityEngine;
using System.Collections;
using System;

public class Tile
{
    public int x;
    public int z;
    public TileType type;

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
    Buildable = 3
}