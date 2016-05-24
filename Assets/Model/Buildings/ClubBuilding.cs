using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class ClubBuilding : Building
{

    float spawnCooldown = 1;
    public override float CustomerSpawnCooldown { get { return spawnCooldown; } }

    public override int ReplaceCost()
    {
        return tile.propertyValue + Settings.ClubBuilding_DestroyCost;
    }

}
