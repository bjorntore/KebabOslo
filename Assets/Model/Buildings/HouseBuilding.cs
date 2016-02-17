using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class HouseBuilding : Building
{

    float spawnCooldown = 300;
    public override float SpawnCooldown { get { return spawnCooldown; } }

    float lastSpawnTime;
    public override float LastSpawnTimed { get; set; }

    public HouseBuilding(Tile tile) : base(tile) { }

}
