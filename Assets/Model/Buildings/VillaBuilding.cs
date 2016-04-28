using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class VillaBuilding : Building
{

    float spawnCooldown = 2;
    public override float CustomerSpawnCooldown { get { return spawnCooldown; } }

    public override int Cost()
    {
        return tile.propertyValue + Settings.VillaBuilding_DestroyCost;
    }

}