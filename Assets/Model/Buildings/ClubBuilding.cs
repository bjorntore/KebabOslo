using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class ClubBuilding : Building
{

    float spawnCooldown = 60;
    public override float SpawnCooldown { get { return spawnCooldown; } }

    float lastSpawnTime;
    public override float LastSpawnTimed { get; set; }

    public ClubBuilding(Tile tile) : base(tile) { }

}
