using System.Collections;
using System.Collections.Generic;
using EpPathFinding.cs;
using System;
using UnityEngine;
using System.Linq;

[Serializable]
public class Customer
{
    World world;

    public Guid InstanceID;

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
        this.hunger = Settings.Customer_BaseHunger;
        SetMoveSpeed();
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
            PathFinding();
            world.player.ChangeReputation(-1);
        }
        else
        {
            destinationKebabBuilding.customers.Add(this);
            state = CustomerState.Eating;
            eatingUntil = Time.time + EatDuration(hunger);
            world.player.ChangeReputation(1);
        }
    }

    float EatDuration(int hunger)
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

        if (hunger >= 50)
        {
            destinationBuilding = FindNearestKebabBuildingByLinearDistance(x, z);
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

        PathFinding();
    }

    void SetDestinationToKebabBuilding(KebabBuilding kebabBuilding)
    {
        destinationX = kebabBuilding.tile.x;
        destinationZ = kebabBuilding.tile.z;
        state = CustomerState.MovingToEat;
    }

    void SetDestinationToMapEnd()
    {
        Tile destinationTile = Utils.Random(world.RoadTiles.Where(t => t.isWorldEdge).ToList());
        destinationX = destinationTile.x;
        destinationZ = destinationTile.z;
        state = CustomerState.MovingToMapEnd;
        destinationBuilding = null;
    }

    void SetDestinationToOrigin()
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

    void PathFinding()
    {
        BaseGrid searchGrid = new StaticGrid(world.Width, world.Height);

        foreach (Tile roadTile in world.RoadTiles)
        {
            searchGrid.SetWalkableAt(roadTile.x, roadTile.z, true);
        }

        GridPos startPos = new GridPos(x, z);
        GridPos endPos = new GridPos(destinationX, destinationZ);
        JumpPointParam jpParam = new JumpPointParam(searchGrid, startPos, endPos);

        resultPath = JumpPointFinder.FindPath(jpParam);
        currentResultPathIndex = 0;

        if (resultPath.Count == 0)
            throw new Exception("Path finding algorithm failed. Did not find any path. Should not happen.");
        else
            SetNextMovingToPosition();
    }

    void SetMoveSpeed()
    {
        moveSpeed = (Settings.Customer_BaseMovementSpeed / 2.0f) + (Settings.Customer_BaseMovementSpeed * hunger / 100.0f);
    }

    bool IsAtOrigin()
    {
        return (x == originX && z == originZ);
    }

    KebabBuilding FindNearestKebabBuildingByLinearDistance(int fromX, int fromZ)
    {
        double nearestDistance = double.PositiveInfinity;
        KebabBuilding nearestKebabBuilding = null;

        foreach (KebabBuilding building in world.KebabBuildings)
        {
            double distance = MathUtils.Distance(fromX, fromZ, building.tile.x, building.tile.z);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestKebabBuilding = building;
            }
        }

        return nearestKebabBuilding;
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
}