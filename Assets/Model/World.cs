using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;


public class World
{

    int width;
    public int Width { get { return width; } }

    int height;
    public int Height { get { return height; } }

    float roadChance = 0.02f;

    Tile[,] tiles;
    public Tile[,] Tiles { get { return tiles; } }
    private List<Tile> _roadTiles;

    List<Building> buildings;
    public List<Building> Buildings { get { return buildings; } }

    public World(int width = 200, int height = 200)
    {
        this.width = width;
        this.height = height;

        InitTiles(width, height);
        InitBuildings();
    }

    public void AddBuilding(Building building, Tile tile)
    {
        if (tile.CanBuildOn())
        {
            tile.type = TileType.Occupied;
            buildings.Add(building);
        }
    }

    //public List<Customer> ArriveCustomer()
    //{
    //    List<Customer> newCustomers = new List<Customer>();

    //    foreach(Building building in buildings)
    //    {

    //    }
    //}

    private void InitTiles(int width, int height)
    {
        tiles = new Tile[width, height];
        _roadTiles = new List<Tile>();

        //List<int> xRoadLines = RollRoadLines();
        //List<int> zRoadLines = RollRoadLines();
        List<int> xRoadLines = new List<int>() { 2, 20, 50, 51, 80 }; // Debug purpose only
        List<int> zRoadLines = new List<int>() { 2, 20, 50, 51, 80 }; // Debug purpose only

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                if (xRoadLines.Contains(x) || zRoadLines.Contains(z))
                {
                    Tile roadTile = new Tile(x, z, TileType.Road);
                    tiles[x, z] = roadTile;
                    _roadTiles.Add(roadTile);
                }
            }
        }

        Debug.LogFormat("Created {0} tiles.", width * height);
    }

    private void InitBuildings()
    {
        List<Tile> buildableTiles = SetAndGetBuildableTiles();

        double clubSpawnChance = 0.1f;
        double houseSpawnChance = 0.4f;

        buildings = new List<Building>();
        foreach (Tile tile in buildableTiles)
        {
            float roll = UnityEngine.Random.Range(0.0f, 1.0f);
            if (roll < clubSpawnChance)
            {
                buildings.Add(new ClubBuilding(tile.x, tile.z));
                tile.type = TileType.Occupied;
            }
            else if (roll < houseSpawnChance)
            {
                buildings.Add(new HouseBuilding(tile.x, tile.z));
                tile.type = TileType.Occupied;
            }
        }

        Debug.LogFormat("Created {0} buildings.", buildings.Count);
    }

    List<Tile> SetAndGetBuildableTiles()
    {
        List<Tile> buildableTiles = new List<Tile>();

        foreach (Tile roadTile in _roadTiles)
        {
            foreach(Tile neighborTile in GetNeighborTiles(roadTile))
            {
                if (neighborTile.type != TileType.Road)
                {
                    neighborTile.type = TileType.Buildable;
                    buildableTiles.Add(neighborTile);
                }
            }
        }

        return buildableTiles;
    }

    List<Tile> GetNeighborTiles(Tile tile)
    {
        List<Tile> tiles = new List<Tile>();

        tiles.Add(SafelyGetTile(tile.x, tile.z + 1));
        tiles.Add(SafelyGetTile(tile.x, tile.z - 1));
        tiles.Add(SafelyGetTile(tile.x + 1, tile.z));
        tiles.Add(SafelyGetTile(tile.x - 1, tile.z));

        return tiles.Where(t => t != null).ToList();
    }

    Tile SafelyGetTile(int x, int z)
    {
        if (x < 0) return null;
        if (x >= width) return null;
        if (z < 0) return null;
        if (z >= height) return null;

        if (tiles[x, z] == null)
            tiles[x, z] = new Tile(x, z, TileType.Empty);

        return tiles[x, z];
    }

    List<int> RollRoadLines()
    {
        List<int> roadLines = new List<int>();
        for (int x = 0; x < width; x++)
        {
            float roll = UnityEngine.Random.Range(0.0f, 1.0f);
            if (roll < roadChance)
                roadLines.Add(x);
        }

        if (roadLines.Count == 0)
            roadLines.Add(width / 2);

        return roadLines;
    }

}