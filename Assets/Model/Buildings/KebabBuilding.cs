using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class KebabBuilding : Building
{

    float spawnCooldown = 0;
    public override float SpawnCooldown { get { return spawnCooldown; } }

    public override float LastSpawnTimed { get; set; }

    public KebabBuilding(Tile tile) : base(tile) { }

}
