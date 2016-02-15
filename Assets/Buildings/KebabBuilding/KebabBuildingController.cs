using UnityEngine;
using System.Collections;
using System;

public class KebabBuildingController : BuildingController, IClickable
{

    public KebabBuilding building;

    public override void SetBuilding(Building building)
    {
        this.building = (KebabBuilding)building;
    }

    public void Click()
    {
        throw new NotImplementedException();
    }

}
