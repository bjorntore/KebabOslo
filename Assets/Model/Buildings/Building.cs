using UnityEngine;
using System.Collections;
using System;

public abstract class Building
{

    public abstract float CustomerSpawnCooldown { get; }
    public abstract int ReplaceCost();

    public Tile tile;
    public float lastSpawnTime;

    public Building()
    {
        lastSpawnTime = Time.time;
    }

    public override string ToString()
    {
        return "Building_" + GetType() + "_" + tile.x + "_" + tile.z;
    }

    public bool CustomerSpawnRoll()
    {
        float timeSinceLastSpawn = Time.time - lastSpawnTime;
        timeSinceLastSpawn += UnityEngine.Random.Range(-CustomerSpawnCooldown, CustomerSpawnCooldown);

        if (timeSinceLastSpawn >= CustomerSpawnCooldown)
        {
            lastSpawnTime += CustomerSpawnCooldown;
            return true;
        }
        else
            return false;
    }

}
