using UnityEngine;
using System.Collections;
using System;

public abstract class NeutralBuildingController : BuildingController
{
    //TODO: Diskuter med BT om det bør vær NeutralBuilding som returneres.
    protected abstract Building GetBuilding();

    protected void DeleteBuildingAndBuildKebabBuilding()
    {
        NeutralBuilding building = (NeutralBuilding)GetBuilding();

        WorldController wc = FindObjectOfType<WorldController>();
		wc.ReplaceBuilding(GetBuilding(), new KebabBuilding(wc.world));
        wc.world.player.ChangeCash(-building.ReplaceCost());

        Destroy(gameObject);
    }

}
