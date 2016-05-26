using System.Collections;
using UnityEngine;

public abstract class NeutralBuilding : Building
{

    public abstract double[,] DayHourPercentSpawnChanceTable { get; }
    public abstract int ReplaceCost();

    private int lastCustomerSpawnRollDay = -1;
    private int lastCustomerSpawnRollHour = -1;


    protected WorldTimeController worldTimeController;

    public NeutralBuilding()
    {
        worldTimeController = WorldTimeController.Instance();
    }

    public bool CustomerSpawnRoll()
    {
        if (lastCustomerSpawnRollDay == worldTimeController.day && lastCustomerSpawnRollHour == worldTimeController.hour)
            return false;

        double spawnChance = 0;
        for(int d = lastCustomerSpawnRollDay + 1; d <= worldTimeController.day; d++)
            for (int h = lastCustomerSpawnRollHour + 1; h <= worldTimeController.hour; h++)
                spawnChance += DayHourPercentSpawnChanceTable[d % 7, h];

        lastCustomerSpawnRollDay = worldTimeController.day;
        lastCustomerSpawnRollHour = worldTimeController.hour;

        return Utils.PercentageRoll(spawnChance);
    }

}
