using System.Collections;
using System.Collections.Generic;
using EpPathFinding.cs;
using System;

public class Customer
{

    public int x;
    public int z;

    public int destinationX = -1;
    public int destinationZ = -1;

    List<GridPos> resultPath;
    int currentResultPathIndex = 0;

    public Customer(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public override string ToString()
    {
        return "Customer_" + x + "_" + z;
    }

    public void DecideDestinationAndPath(KebabBuilding nearestKebabBuilding, int worldWidth, int worldHeight, List<Tile> roadTiles)
    {
        if (nearestKebabBuilding != null)
        {
            destinationX = nearestKebabBuilding.tile.adjacentRoadTile.x;
            destinationZ = nearestKebabBuilding.tile.adjacentRoadTile.z;
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

    }

    public bool HasArrived()
    {
        return (x == destinationX && z == destinationZ);
    }

    public void MoveToNextPos()
    {
        GridPos next = resultPath[currentResultPathIndex];
        x = next.x;
        z = next.y;
        currentResultPathIndex++;
    }

}
