using System.Collections;
using UnityEngine;

public abstract class NeutralBuilding : Building
{

    public abstract double[,] DayHourPercentSpawnChanceTable { get; }
    public abstract int ReplaceCost();

    protected WorldTimeController worldTimeController;

    public NeutralBuilding()
    {
        worldTimeController = WorldTimeController.Instance();
    }

}
