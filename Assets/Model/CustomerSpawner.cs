using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner
{

    private WorldTimeController worldTimeController;

    private List<BuildingSpawnTracker> trackers = new List<BuildingSpawnTracker>();

    public CustomerSpawner(List<Building> buildings)
    {
        worldTimeController = WorldTimeController.Instance();

        BuildTrackersList(buildings);
    }

    public void RebuildTrackersList(List<Building> buildings)
    {
        foreach (BuildingSpawnTracker tracker in trackers)
            tracker.buildings = new Queue<NeutralBuilding>();

        BuildTrackersList(buildings);
    }

    private void BuildTrackersList(List<Building> buildings)
    {
        foreach(Building b in buildings)
        {
            /* If we need to expand with more building types that cant spawn, i suggest adding an ICustomerSpawnable interface which we use to indicate that this class can spawn. 
            Trying to detect that the class inherits from an abstract class (NeutralBuilding) is by my research not very easy. 
            Se http://stackoverflow.com/questions/2742276/how-do-i-check-if-a-type-is-a-subtype-or-the-type-of-an-object */
            if (b.GetType() == typeof(KebabBuilding))
                continue;

            BuildingSpawnTracker tracker = trackers.Find(t => t.buildingType == b.GetType());
            if (tracker == null)
            {
                tracker = new BuildingSpawnTracker(b.GetType());
                trackers.Add(tracker);
            }

            tracker.buildings.Enqueue((NeutralBuilding)b);
        }
    }

    public List<NeutralBuilding> GetSpawningBuildings()
    {
        List<NeutralBuilding> spawningBildings = new List<NeutralBuilding>();

        int currentDay = worldTimeController.day;
        int currentHour = worldTimeController.hour;

        foreach(BuildingSpawnTracker tracker in trackers)
        {
            float timeSinceLastSpawn = Time.time - tracker.lastSpawnCheckTime;
            double hoursPassed = (timeSinceLastSpawn / Settings.World_BaseTimeSecondsPerDay) * 24;

            List<NeutralBuilding> spawningBuildingsFromTracker = tracker.GetSpawningBuildings(hoursPassed, currentDay, currentHour);

            spawningBildings.AddRange(spawningBuildingsFromTracker);
            //Debug.LogFormat("Time since last spawn for {0}({4}): {1} - hoursPassed: {2} - spawnCount: {3}", tracker.buildingType, timeSinceLastSpawn, hoursPassed, spawningBuildingsFromTracker.Count, tracker.buildings.Count);
        }

        return spawningBildings;
    }


    private class BuildingSpawnTracker
    {
        
        public Type buildingType;
        public Queue<NeutralBuilding> buildings = new Queue<NeutralBuilding>();

        private double[,] dayHourPercentSpawnChanceTable;
        public float lastSpawnCheckTime = Time.time;
        public int totalSpawnCount = 0; // Statistical/Debug purpose only

        public BuildingSpawnTracker(Type type)
        {
            this.buildingType = type;

            var instanceOfBuildingType = (NeutralBuilding)Activator.CreateInstance(buildingType);
            dayHourPercentSpawnChanceTable = instanceOfBuildingType.DayHourPercentSpawnChanceTable;
        }

        public List<NeutralBuilding> GetSpawningBuildings(double hoursPassed, int currentDay, int currentHour)
        {
            double spawnChanceThisDayHour = dayHourPercentSpawnChanceTable[currentDay % 7, currentHour] * buildings.Count;
            double spawnCountAverage = (spawnChanceThisDayHour * hoursPassed); // Not perfect, since it will always spawn according to the spawn chance of the last hour, and not accross elapsed hours
            int spawnCountRoll = Utils.RandomInt(0.0, spawnCountAverage * 2.0);

            List<NeutralBuilding> spawningBuildings = new List<NeutralBuilding>();
            for (int i = 0; i < spawnCountRoll; i++)
            {
                NeutralBuilding dequeuedBuilding = buildings.Dequeue();
                spawningBuildings.Add(dequeuedBuilding);
                buildings.Enqueue(dequeuedBuilding);
                //Debug.LogFormat("Total spawn for {0} is now {1}", buildingType, totalSpawnCount);
            }
            totalSpawnCount += spawnCountRoll;

            lastSpawnCheckTime = Time.time;
            return spawningBuildings;
        }

    }

}