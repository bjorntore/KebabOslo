using System.Collections;
using System.Collections.Generic;
using EpPathFinding.cs;
using System;
using UnityEngine;

[Serializable]
public class Customer
{

    public Guid InstanceID;
    public int x; // OBS: The position is set before the object actually arrives @ drawing
    public int z; // OBS: The position is set before the object actually arrives @ drawing

    public int movingToX;
    public int movingToZ;

    public int destinationX = -1;
    public int destinationZ = -1;

    List<GridPos> resultPath;
    int currentResultPathIndex = 0;

    public Customer(int x, int z)
    {
        this.InstanceID = Guid.NewGuid();
        this.x = x;
        this.z = z;
    }

    public override string ToString()
    {
        return  "Customer_" + x + "_" + z + "_" + InstanceID;
    }

    public bool HasArrived()
    {
        return (x == destinationX && z == destinationZ);
    }

    public void DecideDestinationAndPath(KebabBuilding nearestKebabBuilding, int worldWidth, int worldHeight, List<Tile> roadTiles)
    {
        if (nearestKebabBuilding != null)
        {
            destinationX = nearestKebabBuilding.tile.x;
            destinationZ = nearestKebabBuilding.tile.z;
        }
        else
        {
            Tile destinationTile = roadTiles[0];
            destinationX = destinationTile.x;
            destinationZ = destinationTile.z;
        }

        BaseGrid searchGrid = new StaticGrid(worldWidth, worldHeight);

        foreach(Tile roadTile in roadTiles)
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
            MoveToNextPos();
    }

    public void MoveToNextPos()
    {
        GridPos next = resultPath[currentResultPathIndex];
        movingToX = next.x;
        movingToZ = next.y;
        currentResultPathIndex++;
    }

}
