﻿using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Collections;

public class World
{
    public Player player;
    public CustomerSpawner customerSpawner;

    private int width;
    public int Width { get { return width; } }

    private int height;
    public int Height { get { return height; } }

    private float roadChance = 0.02f;

    private Tile[,] tiles;
    public Tile[,] Tiles { get { return tiles; } }

    private List<Tile> _roadTiles;
    public List<Tile> RoadTiles { get { return _roadTiles; } }

    private List<Building> buildings = new List<Building>();
    public List<Building> Buildings { get { return buildings; } }

    private List<KebabBuilding> _kebabBuildings = new List<KebabBuilding>();
    public List<KebabBuilding> KebabBuildings { get { return _kebabBuildings; } }

    private List<Customer> customers = new List<Customer>();
    public List<Customer> Customers { get { return customers; } }

    public World(Player player, int width = 200, int height = 200)
    {
        Debug.LogWarning("Using UnityEngine.Random.seed! Debug only!");
        UnityEngine.Random.seed = 42; // Debug purpose only, same random values for testing performance

        this.player = player;
        this.width = width;
        this.height = height;

        InitTiles(width, height);
        List<Tile> buildableTiles = SetAndGetBuildableTiles();
        InitTilePropertyValues();
        InitBuildings(buildableTiles);

        this.customerSpawner = new CustomerSpawner(buildings);
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

    public Customer CreateCustomer(Tile tile)
    {
        int customerCash = Settings.Customer_BaseCash + (tile.propertyValue / 500);
        Customer newCustomer = new Customer(this, tile.x, tile.z, customerCash);
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
            .Where(c => c.CanChangeItsMind())
            .OrderBy(c => Math.Abs(potentialNewX - c.X) + Math.Abs(potentialNewZ - c.Z) ).ToList();

        foreach (Customer customer in customersReferenceCopy)
        {
            customer.DecideDestinationAndPath();
            yield return new WaitForSeconds(0.05f);
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

    public int AverageKebabBuildingReputation()
    {
        if (_kebabBuildings.Count > 0)
            return _kebabBuildings.Sum(k => k.Reputation) / _kebabBuildings.Count;
        else
            return 0;
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
        customerSpawner.RebuildTrackersList(buildings);
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
        //List<int> xRoadLines = new List<int>() { 2, 6 }; // Debug purpose only
        //List<int> zRoadLines = new List<int>() { 2, 6 }; // Debug purpose only

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                if (xRoadLines.Contains(x) || zRoadLines.Contains(z))
                {
                    Tile roadTile = new Tile(x, z, TileType.Road);
                    if (x == 0 || x == width - 1 || z == 0 || z == height - 1)
                        roadTile.isWorldEdge = true;
                    tiles[x, z] = roadTile;
                    _roadTiles.Add(roadTile);
                }
            }
        }

        Debug.LogFormat("Created {0} tiles at model level.", width * height);
    }

    private void InitTilePropertyValues()
    {
        int maxDistance = (int)MathUtils.Distance(0, 0, width / 5, height / 5);

        int amountOfPropertyHighPoints = Utils.RandomInt(1, 3);
        for (int i = 0; i < amountOfPropertyHighPoints; i++)
        {
            int highPointX = UnityEngine.Random.Range(0, width);
            int highPointZ = UnityEngine.Random.Range(0, height);
            Debug.LogFormat("Rolled property value high point: ({0},{1})", highPointX, highPointZ);
            for (int x = highPointX - maxDistance; x < highPointX + maxDistance; x++)
            {
                for (int z = highPointZ - maxDistance; z < highPointZ + maxDistance; z++)
                {
                    Tile tile = SafelyGetTile(x, z);
                    if (tile != null && tile.type != TileType.Road)
                    {
                        int distance = (int)MathUtils.Distance(highPointX, highPointZ, x, z);
                        if (distance > maxDistance)
                            continue;

                        tile.propertyValue += MathUtils.LinearConversionInverted(distance, maxDistance, Settings.Tile_MaxPropertyValueGainableFromHighPoint);
                        //Debug.Log(tile.ToString() + " increased to " + tile.propertyValue);
                    }
                }
            }
        }
    }

    private void InitBuildings(List<Tile> buildableTiles)
    {
        ProportionItem clubSpawn = new ProportionItem("Club", 10);
        ProportionItem policeSpawn = new ProportionItem("Police", 2);
        ProportionItem houseSpawn = new ProportionItem("House", 10); // Might get converted to Villa, based on property value
        ProportionItem villaSpawn = new ProportionItem("Villa", 1);
        ProportionItem emptyTile = new ProportionItem("Empty", 48); // Might get converted to House, based on property value

        ProportionValues buildingSpawnChances = new ProportionValues(new ProportionItem[] { clubSpawn, policeSpawn, houseSpawn, villaSpawn, emptyTile });

        foreach (Tile tile in buildableTiles)
        {
            var randomChoice = buildingSpawnChances.RandomChoice();

            if (randomChoice.Name == "Club")
                AddBuilding(new ClubBuilding(), tile);
            else if (randomChoice.Name == "Police")
                AddBuilding(new PoliceBuilding(), tile);
            else if (randomChoice.Name == "House")
            {
                double villaChance = (Convert.ToDouble(tile.propertyValue) - Settings.Tile_BasePropertyValue) * 100 / Settings.Tile_MaxPropertyValueGainableFromHighPoint;
                if (Utils.PercentageRoll(villaChance))
                    AddBuilding(new VillaBuilding(), tile);
                else
                    AddBuilding(new HouseBuilding(), tile);
            }
            else
            {
                var propertyValueHouseChance = (Convert.ToDouble(tile.propertyValue) - Settings.Tile_BasePropertyValue) * 100 / Settings.Tile_MaxPropertyValueGainableFromHighPoint;
                if (Utils.PercentageRoll(propertyValueHouseChance))
                    AddBuilding(new HouseBuilding(), tile);
            }
        }

        LogBuildingTypes();
        Utils.Shuffle(buildings); // For customer spawning purposes

        Debug.LogFormat("Created {0} buildings at model level.", buildings.Count);
    }

    private List<Tile> SetAndGetBuildableTiles()
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

    private List<Tile> GetNeighborTiles(Tile tile)
    {
        List<Tile> tiles = new List<Tile>();

        tiles.Add(SafelyGetOrCreateTile(tile.x, tile.z + 1));
        tiles.Add(SafelyGetOrCreateTile(tile.x, tile.z - 1));
        tiles.Add(SafelyGetOrCreateTile(tile.x + 1, tile.z));
        tiles.Add(SafelyGetOrCreateTile(tile.x - 1, tile.z));

        return tiles.Where(t => t != null).ToList();
    }

    private Tile SafelyGetOrCreateTile(int x, int z)
    {
        if (x < 0) return null;
        if (x >= width) return null;
        if (z < 0) return null;
        if (z >= height) return null;

        if (tiles[x, z] == null)
            tiles[x, z] = new Tile(x, z, TileType.Empty);

        return tiles[x, z];
    }

    private Tile SafelyGetTile(int x, int z)
    {
        if (x < 0) return null;
        if (x >= width) return null;
        if (z < 0) return null;
        if (z >= height) return null;

        return tiles[x, z];
    }

    private List<int> RollRoadLines()
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