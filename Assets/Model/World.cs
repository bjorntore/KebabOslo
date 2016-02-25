using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Collections;

public class World
{
    public Player player;

    int width;
    public int Width { get { return width; } }

    int height;
    public int Height { get { return height; } }

    float roadChance = 0.02f;

    Tile[,] tiles;
    public Tile[,] Tiles { get { return tiles; } }

    List<Tile> _roadTiles;
    public List<Tile> RoadTiles { get { return _roadTiles; } }

    List<Building> buildings = new List<Building>();
    public List<Building> Buildings { get { return buildings; } }

    List<KebabBuilding> _kebabBuildings = new List<KebabBuilding>();
    public List<KebabBuilding> KebabBuildings { get { return _kebabBuildings; } }

    List<Customer> customers = new List<Customer>();
    public List<Customer> Customers { get { return customers; } }

    public World(Player player, int width = 200, int height = 200)
    {
        this.player = player;
        this.width = width;
        this.height = height;

        InitTiles(width, height);
        InitBuildings();
    }

    public void AddBuilding(Building building, Tile tile)
    {
        if (tile.type == TileType.Buildable)
        {
            buildings.Add(building);
            building.tile = tile;
            building.tile.type = TileType.Occupied;
            if (building is KebabBuilding)
            {
                _kebabBuildings.Add((KebabBuilding)building);
                SanityCheckKebabBuildingsCount();
            }
        }
        else
            throw new Exception("Tried to add a new building to tile " + tile.ToString() + " when not allowed. Should not happend. Fix originating code.");
    }

    public Customer CreateCustomer(int x, int z)
    {
        Customer newCustomer = new Customer(x, z, this);
        newCustomer.DecideDestinationAndPath();
        customers.Add(newCustomer);
        return newCustomer;
    }

    public void RemoveCustomer(Customer customer)
    {
        customers.Remove(customer);
    }

    public IEnumerator SetNewCustomerDestinations(int potentialNewX, int potentialNewZ)
    {
        List<Customer> customersReferenceCopy = new List<Customer>(customers)
            .Where(c => !c.HasArrived())
            .OrderBy(c => Math.Abs(potentialNewX - c.X) + Math.Abs(potentialNewZ - c.Z) ).ToList();

        foreach (Customer customer in customersReferenceCopy)
        {
            customer.DecideDestinationAndPath();
            yield return new WaitForSeconds(0.3f);
        }
    }

    public void DeleteBuilding(Building building, Tile tile)
    {
        tile.type = TileType.Buildable;
        buildings.Remove(building);
        if (building is KebabBuilding)
        {
            _kebabBuildings.Remove((KebabBuilding)building);
            SanityCheckKebabBuildingsCount();
        }
    }

    private void SanityCheckKebabBuildingsCount()
    {
        int _kebabBuildingsCount = _kebabBuildings.Count;
        int buildingsCount = buildings.Where(b => b is KebabBuilding).Count();
        if (_kebabBuildingsCount != buildingsCount)
            throw new Exception(string.Format("Missmatch between count in _kebabBuildings ({0}) and buildings ({1}). Should never happend that the cached _KebabBuildings has different number.", _kebabBuildingsCount, buildingsCount));
    }

    public void ReplaceBuilding(Building oldBuilding, Building newBuilding)
    {
        DeleteBuilding(oldBuilding, oldBuilding.tile);
        AddBuilding(newBuilding, oldBuilding.tile);
        Debug.Log("Replaced building at model level. Now " + buildings.Count + " total buildings.");
    }

    private void InitTiles(int width, int height)
    {
        tiles = new Tile[width, height];
        _roadTiles = new List<Tile>();

        //List<int> xRoadLines = RollRoadLines();
        //List<int> zRoadLines = RollRoadLines();
        List<int> xRoadLines = new List<int>() { 2, 20, 50, 51, 80 }; // Debug purpose only
        List<int> zRoadLines = new List<int>() { 2, 20, 50, 51, 80 }; // Debug purpose only
        //List<int> xRoadLines = new List<int>() { 2, 6}; // Debug purpose only
        //List<int> zRoadLines = new List<int>() { 2, 6}; // Debug purpose only

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

        Debug.LogFormat("Created {0} tiles at model level.", width * height);
    }

    private void InitBuildings()
    {
        List<Tile> buildableTiles = SetAndGetBuildableTiles();

        ProportionItem clubSpawn = new ProportionItem("Club", 10);
        ProportionItem policeSpawn = new ProportionItem("Police", 2);
        ProportionItem houseSpawn = new ProportionItem("House", 40);
        ProportionItem emptyTile = new ProportionItem("Empty", 48);

        ProportionValues buildingSpawnChances = new ProportionValues(new ProportionItem[] { clubSpawn, policeSpawn, houseSpawn, emptyTile });

        UnityEngine.Random.seed = 42; // Debug purpose only, same random values for testing performance

        foreach (Tile tile in buildableTiles)
        {
            var randomChoice = buildingSpawnChances.RandomChoice();

            if (randomChoice.Name == "Club")
                AddBuilding(new ClubBuilding(null), tile);
            else if (randomChoice.Name == "Police")
                AddBuilding(new PoliceBuilding(null), tile);
            else if (randomChoice.Name == "House")
                AddBuilding(new HouseBuilding(null), tile);
        }

        LogBuildingTypes();
        Shuffle(buildings);

        Debug.LogFormat("Created {0} buildings at model level.", buildings.Count);
    }

    List<Tile> SetAndGetBuildableTiles()
    {
        List<Tile> buildableTiles = new List<Tile>();

        foreach (Tile roadTile in _roadTiles)
        {
            foreach (Tile neighborTile in GetNeighborTiles(roadTile))
            {
                if (neighborTile.type != TileType.Road)
                {
                    neighborTile.type = TileType.Buildable;
                    if (!buildableTiles.Contains(neighborTile))
                    {
                        neighborTile.adjacentRoadTile = roadTile;
                        buildableTiles.Add(neighborTile);
                    }
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


    public void Shuffle<T>(List<T> list)
    {
        System.Random rng = new System.Random();

        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    private void LogBuildingTypes()
    {
        if (Debug.isDebugBuild)
        {
            var types = buildings.GroupBy(b => b.GetType())
                    .Select(g => new { Type = g.Key, Count = g.Count() })
                    .ToList();

            foreach (var a in types)
            {
                Debug.Log(a.Type + ": " + a.Count);
            }
        }
    }
}