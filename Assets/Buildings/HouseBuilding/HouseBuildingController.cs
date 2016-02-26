using UnityEngine;
using System.Collections;
using System;

public class HouseBuildingController : NeutralBuildingController, IClickable
{
    public HouseBuilding building;

    public override void SetBuilding(Building building)
    {
        this.building = (HouseBuilding)building;
    }

    protected override Building GetBuilding()
    {
        return building;
    }

    public void Click()
    {
        GenericDialog panel = GenericDialog.Instance();
        panel.SetPanel("House", "Tear down the building and build a kebab shop!", DeleteBuildingAndBuildKebabBuilding, string.Format("Erase and build (-{0} cash)!", building.Cost()));
    }

}
