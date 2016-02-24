using System.Collections;
using System.Collections.Generic;
using EpPathFinding.cs;
using System;
using UnityEngine;

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

    public float moveSpeed;
    public int baseMoveSpeed = 3;
    public int hunger;

    public Customer(int x, int z, World world)
    {
        this.world = world;
        this.InstanceID = Guid.NewGuid();
        this.originX = x;
        this.originZ = z;
        this.x = x;
        this.z = z;
        this.hunger = 50;
        SetMoveSpeed();
    }

    public bool HasArrived()
    {
        return (x == destinationX && z == destinationZ);
    }

    public void TriggerArrived()
    {
        if (destinationBuilding is KebabBuilding)
        {
            hunger = 0;
            SetMoveSpeed();
        }
    }

    public void DecideDestinationAndPath()
    {
        if (hunger >= 50)
        {
            destinationBuilding = FindNearestKebabBuildingByLinearDistance(x, z);
            if (destinationBuilding is KebabBuilding)
            {
                destinationX = destinationBuilding.tile.x;
                destinationZ = destinationBuilding.tile.z;
            }
            else
                SetDestinationToMapEnd();
        }
        else
        {
            if (IsAtOrigin())
                SetDestinationToMapEnd();
            else
            {
                Debug.Log("GOING HOME");
                destinationBuilding = null;
                SetOriginAsDestination();
            }
        }

        PathFinding();
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

    void SetDestinationToMapEnd()
    {
        Tile destinationTile = world.RoadTiles[0];
        destinationX = destinationTile.x;
        destinationZ = destinationTile.z;
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
        moveSpeed = (baseMoveSpeed / 2.0f) + (baseMoveSpeed * hunger / 100.0f);
    }

    bool IsAtOrigin()
    {
        return (x == originX && z == originZ);
    }

    void SetOriginAsDestination()
    {
        destinationX = originX;
        destinationZ = originZ;
    }

    KebabBuilding FindNearestKebabBuildingByLinearDistance(int fromX, int fromZ)
    {
        double nearestDistance = double.PositiveInfinity;
        KebabBuilding nearestKebabBuilding = null;

        foreach (KebabBuilding building in world.KebabBuildings)
        {
            double distance = Math.Sqrt(Math.Pow(Math.Abs(building.tile.x - fromX), 2) + Math.Pow(Math.Abs(building.tile.z - fromZ), 2));
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestKebabBuilding = building;
            }
        }

        return nearestKebabBuilding;
    }

}
