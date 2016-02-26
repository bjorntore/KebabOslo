﻿using UnityEngine;
using System.Collections;
using System;

public abstract class NeutralBuildingController : BuildingController
{
    protected abstract Building GetBuilding();

    protected void DeleteBuildingAndBuildKebabBuilding()
    {
        Building building = GetBuilding();

        WorldController wc = FindObjectOfType<WorldController>();
		wc.ReplaceBuilding(GetBuilding(), new KebabBuilding(null, wc.world));
        wc.world.player.ChangeCash(-building.Cost());

        Destroy(gameObject);
    }

}
