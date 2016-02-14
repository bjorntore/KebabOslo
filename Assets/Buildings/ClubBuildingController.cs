using UnityEngine;
using System.Collections;
using System;

public class ClubBuildingController : BuildingController, IClickable
{

    public ClubBuilding building;

    public void Click()
    {
        Debug.Log("FUCKING YEA, ITS CLALLED");
    }

    public override void SetBuilding(Building building)
    {
        this.building = (ClubBuilding)building;
    }

}
