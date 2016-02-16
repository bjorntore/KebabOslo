using UnityEngine;
using System.Collections;
using System;

public abstract class NeutralBuildingController : BuildingController
{
    protected abstract int Cost();
    protected abstract Building GetBuilding();

    protected void DeleteBuildingAndBuildKebabBuilding()
    {
        WorldController wc = FindObjectOfType<WorldController>();
        wc.ReplaceBuilding(GetBuilding(), new KebabBuilding(null));
        wc.player.ChangeCash(-Cost());
        Destroy(gameObject);
    }

}
