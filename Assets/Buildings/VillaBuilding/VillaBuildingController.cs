using UnityEngine;
using System.Collections;
using System;

public class VillaBuildingController : NeutralBuildingController, IClickable
{
    public VillaBuilding building;

    public override void SetBuilding(Building building)
    {
        this.building = (VillaBuilding)building;
    }

    protected override Building GetBuilding()
    {
        return building;
    }

    public void Click()
    {
        GenericDialog panel = GenericDialog.Instance();
        panel.OpenDialog("Villa", "Tear down the building and build a kebab shop!", DeleteBuildingAndBuildKebabBuilding, string.Format("Erase and build (-{0} cash)!", building.ReplaceCost()));
    }

}
