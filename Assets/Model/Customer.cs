using System.Collections;
using System.Collections.Generic;
using EpPathFinding.cs;
using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class Customer
{
    World world;

    public Guid InstanceID;


    // POSITION RELATED VARIABLES

    int x; // OBS: The position is set before the object actually arrives @ drawing
    public int X { get { return x; } }

    int z; // OBS: The position is set before the object actually arrives @ drawing
    public int Z { get { return z; } }

    public int originX;
    public int originZ;

    public int movingToX;
    public int movingToZ;

    Building destinationBuilding;
    public Building DestinationBuilding { get { return destinationBuilding; } }

    public int destinationX = -1;
    public int destinationZ = -1;

    List<GridPos> resultPath;
    int currentResultPathIndex = 0;

    //private static BaseGrid staticlyCachedSearchGrid;
    private static JumpPointParam staticlyCachedJpParam;


    // STATE VARIABLES

    CustomerState state = CustomerState.Nothing;
    public CustomerState State { get { return state; } }

    CustomerMood mood;
    public CustomerMood Mood { get { return mood; } }

    public float eatingUntil;
    public float moveSpeed;
    public int hunger;

    public Customer(int x, int z, World world)
    {
        this.world = world;
        this.InstanceID = Guid.NewGuid();
        this.originX = x;
        this.originZ = z;
        this.x = x;
        this.z = z;
        this.hunger = Utils.RandomInt(0, 100);
        SetMoveSpeed();

        CacheJumpPointParam();
    }

    public bool HasArrived()
    {
        return (x == destinationX && z == destinationZ);
    }

    public void TriggerArrivedAtKebabBuilding()
    {
        KebabBuilding destinationKebabBuilding = (KebabBuilding)destinationBuilding;
        if (destinationKebabBuilding.IsFull())
        {
            mood = CustomerMood.AngryNoCapacity;
            SetDestinationToMapEnd();
            FindPath();
            world.player.ChangeReputation(-Settings.KebabBuilding_ReputationLostFromFull);
        }
        else
        {
            destinationKebabBuilding.customers.Add(this);
            state = CustomerState.Eating;
            eatingUntil = Time.time + EatDuration(hunger);
            world.player.ChangeReputation(Settings.KebabBuilding_ReputationGainedFromSale);
            destinationKebabBuilding.AddCashEarned(Settings.KebabBuilding_CashPerKebab);
        }
    }

    private float EatDuration(int hunger)
    {
        return hunger / Settings.Customer_EatSpeedPerSec;
    }

    public void StopEating()
    {
        mood = CustomerMood.Normal;
        hunger = 0;
        SetMoveSpeed();
        KebabBuilding destinationKebabBuilding = (KebabBuilding)destinationBuilding;
        destinationKebabBuilding.customers.Remove(this);
        DecideDestinationAndPath();
    }

    public void DecideDestinationAndPath()
    {
        state = CustomerState.Nothing;
        mood = CustomerMood.Normal;

        if (hunger > 0)
        {
            destinationBuilding = CheckIfWantKebab();
            if (destinationBuilding is KebabBuilding)
                SetDestinationToKebabBuilding((KebabBuilding)destinationBuilding);
            else
                SetDestinationToMapEnd();
        }
        else
        {
            if (IsAtOrigin())
                SetDestinationToMapEnd();
            else
                SetDestinationToOrigin();
        }

        FindPath();
    }

    private KebabBuilding CheckIfWantKebab()
    {
        int mostWantedScore = 0;
        KebabBuilding mostWantedKebabBuilding = null;

        foreach (KebabBuilding building in world.KebabBuildings)
        {
            double distance = MathUtils.Distance(x, z, building.tile.x, building.tile.z);
            int buildingWantScore = hunger + building.GetOwnerReputation() - Convert.ToInt32(distance);

            if (buildingWantScore >= mostWantedScore)
            {
                mostWantedScore = buildingWantScore;
                mostWantedKebabBuilding = building;
            }
        }

        if (mostWantedKebabBuilding != null)
            return mostWantedKebabBuilding;
        else
        {
            mood = CustomerMood.SkippingKebabToday;
            return null;
        }
    }

    private void SetDestinationToKebabBuilding(KebabBuilding kebabBuilding)
    {
        destinationX = kebabBuilding.tile.x;
        destinationZ = kebabBuilding.tile.z;
        state = CustomerState.MovingToEat;
    }

    private void SetDestinationToMapEnd()
    {
        Tile destinationTile = world.RoadTiles.Where(t => t.isWorldEdge).OrderBy(t => MathUtils.Distance(t.x, t.z, x, z)).First();
        destinationX = destinationTile.x;
        destinationZ = destinationTile.z;
        state = CustomerState.MovingToMapEnd;
        destinationBuilding = null;
    }

    private void SetDestinationToOrigin()
    {
        destinationX = originX;
        destinationZ = originZ;
        state = CustomerState.MovingToOrigin;
        destinationBuilding = null;
    }

    public void SetPosition(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public void SetNextMovingToPosition()
    {
        GridPos next = resultPath[currentResultPathIndex];
        movingToX = next.x;
        movingToZ = next.y;
        currentResultPathIndex++;
    }

    public override string ToString()
    {
        return "Customer_" + x + "_" + z + "_" + InstanceID;
    }

    private void FindPath()
    {
        CacheJumpPointParam();

        GridPos firstRoadPos = FindWalkableTile(x, z);
        GridPos lastRoadPos = FindWalkableTile(destinationX, destinationZ);
        GridPos destinationPos = new GridPos(destinationX, destinationZ);
        staticlyCachedJpParam.Reset(firstRoadPos, lastRoadPos);

        resultPath = new List<GridPos>();
        resultPath.AddRange(JumpPointFinder.FindPath(staticlyCachedJpParam));
        resultPath.Add(destinationPos);
        currentResultPathIndex = 0;

        if (resultPath.Count == 0)
            throw new Exception("Path finding algorithm failed. Did not find any path. Should not happen.");
        else
            SetNextMovingToPosition();
    }

    private GridPos FindWalkableTile(int x, int z)
    {
        Tile tile = world.Tiles[x, z];
        if (tile.type != TileType.Road)
            return new GridPos(tile.adjacentRoadTile.x, tile.adjacentRoadTile.z);
        else
            return new GridPos(tile.x, tile.z);
    }

    private void CacheJumpPointParam()
    {
        if (staticlyCachedJpParam == null)
        {
            BaseGrid searchGrid = new StaticGrid(world.Width, world.Height);

            foreach (Tile roadTile in world.RoadTiles)
                searchGrid.SetWalkableAt(roadTile.x, roadTile.z, true);

            staticlyCachedJpParam = new JumpPointParam(searchGrid);
        }
    }

    private void SetMoveSpeed()
    {
        moveSpeed = (Settings.Customer_BaseMovementSpeed / 2.0f) + (Settings.Customer_BaseMovementSpeed * hunger / 100.0f);
    }

    private bool IsAtOrigin()
    {
        return (x == originX && z == originZ);
    }

}

public enum CustomerState
{
    Nothing,
    MovingToEat,
    MovingToMapEnd,
    MovingToOrigin,
    Eating,
}

public enum CustomerMood
{
    Normal,
    AngryNoCapacity,
    SkippingKebabToday,
}