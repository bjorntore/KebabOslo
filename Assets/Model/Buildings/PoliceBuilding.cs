using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class PoliceBuilding : Building
{

    float spawnCooldown = 1;
    public override float SpawnCooldown { get { return spawnCooldown; } }

    public override float LastSpawnTimed { get; set; }

    public PoliceBuilding(Tile tile) : base(tile) { }

}
