using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public class KebabBuilding : Building
{

    float spawnCooldown = 0;
    public override float SpawnCooldown { get { return spawnCooldown; } }

    public override float LastSpawnTimed { get; set; }

    public int capacity = 1;
    public List<Customer> customers = new List<Customer>();

    public KebabBuilding(Tile tile) : base(tile) { }

    public bool IsFull()
    {
        return customers.Count >= capacity;
    }

}
