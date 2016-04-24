using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class HouseBuilding : Building
{

    float spawnCooldown = 1;
    public override float CustomerSpawnCooldown { get { return spawnCooldown; } }

    public HouseBuilding(Tile tile) : base(tile) { }

    public override int Cost()
    {
        return tile.propertyValue + Settings.HouseBuilding_DestroyCost;
    }

}
