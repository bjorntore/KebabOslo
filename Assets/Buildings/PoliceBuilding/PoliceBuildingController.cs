using UnityEngine;
using System.Collections;
using System;

public class PoliceBuildingController : BuildingController, IClickable {

	public PoliceBuilding building;

	public override void SetBuilding(Building building)
	{
		this.building = (PoliceBuilding)building;
	}

	public void Click()
	{
		throw new NotImplementedException();
	}
}
