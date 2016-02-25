using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class KebabBuildingController : BuildingController, IClickable
{
    public KebabBuilding building;

    public override void SetBuilding(Building building)
    {
        this.building = (KebabBuilding)building;
    }

    public void Click()
    {
        //Make me a dialog.
    }

}
