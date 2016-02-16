using UnityEngine;
using System.Collections;
using System;

public class ClubBuildingController : NeutralBuildingController, IClickable
{

    ClubBuilding building;

    public override void SetBuilding(Building building)
    {
        this.building = (ClubBuilding)building;
    }

    protected override Building GetBuilding()
    {
        return building;
    }

    public void Click()
    {
        GenericDialogPanel panel = GenericDialogPanel.Instance();
        panel.SetPanel("Club", "Tear down the building and build a kebab shop!", DeleteBuildingAndBuildKebabBuilding, string.Format("Erase and build (-{0} cash)!", Cost()));
    }

    protected override int Cost()
    {
        return 3000;
    }

}
