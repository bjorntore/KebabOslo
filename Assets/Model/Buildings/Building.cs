using UnityEngine;
using System.Collections;
using System;

public abstract class Building
{

    public abstract float SpawnCooldown { get; }
    public abstract float LastSpawnTimed { get; set; }
    public abstract int Cost();

    public Tile tile;

    public Building(Tile tile)
    {
        this.tile = tile;
        LastSpawnTimed = Time.time;
    }

    public override string ToString()
    {
        return "Building_" + GetType() + "_" + tile.x + "_" + tile.z;
    }

    public bool SpawnRoll()
    {
        float timeSinceLastSpawn = Time.time - LastSpawnTimed;
        timeSinceLastSpawn += UnityEngine.Random.Range(-SpawnCooldown, SpawnCooldown);

        if (timeSinceLastSpawn >= SpawnCooldown)
        {
            LastSpawnTimed += SpawnCooldown;
            return true;
        }
        else
            return false;
    }

}
