using UnityEngine;
using System.Collections;
using System;

public class PoliceBuildingController : NeutralBuildingController, IClickable {

	public PoliceBuilding building;

	public override void SetBuilding(Building building)
	{
		this.building = (PoliceBuilding)building;
	}

    protected override Building GetBuilding()
    {
        return building;
    }

    public void Click()
    {
        GenericDialog panel = GenericDialog.Instance();
        panel.SetPanel("Police station", "Tear down the building and build a kebab shop!", DeleteBuildingAndBuildKebabBuilding, string.Format("Erase and build (-{0} cash)!", building.Cost()));
    }

}
