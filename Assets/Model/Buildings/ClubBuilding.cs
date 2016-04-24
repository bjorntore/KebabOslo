using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class ClubBuilding : Building
{

    float spawnCooldown = 1;
    public override float CustomerSpawnCooldown { get { return spawnCooldown; } }

    public ClubBuilding(Tile tile) : base(tile) { }

    public override int Cost()
    {
        return tile.propertyValue + Settings.ClubBuilding_DestroyCost;
    }

}
