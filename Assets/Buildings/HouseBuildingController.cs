using UnityEngine;
using System.Collections;
using System;

public class HouseBuildingController : BuildingController, IClickable
{

    public HouseBuilding building;

    public override void SetBuilding(Building building)
    {
        this.building = (HouseBuilding)building;
    }

    public void Click()
    {
        throw new NotImplementedException();
    }
}
