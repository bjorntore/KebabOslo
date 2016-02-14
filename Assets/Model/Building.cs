using UnityEngine;
using System.Collections;

public abstract class Building
{

    public int x;
    public int z;

    public Building(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public override string ToString()
    {
        return "Building_" + GetType() + "_" + x + "_" + z;
    }

}
