﻿using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;

public class WorldController : MonoBehaviour
{
    public Transform groundTransform;
    public GameObject roadTilePrefab;
    public GameObject buildableTilePrefab;

    public GameObject houseBuildingPrefab;
    public GameObject clubBuildingPrefab;
    public GameObject kebabBuildingPrefab;
	public GameObject policeBuildingPrefab;

    public GameObject normalCustomerPrefab;

    GameObject tileContainer;
    GameObject buildingContainer;
    GameObject customerContainer;

    public World world;
    public Player player;

    // Use this for initialization
    void Start()
    {
        world = new World();
        player = new Player("ShrubNub");
        tileContainer = new GameObject("TileContainer");
        buildingContainer = new GameObject("BuildingContainer");
        customerContainer = new GameObject("CustomerContainer");

        AdjustGround();
        SpawnTiles();
        SpawnBuildings();

        StartCoroutine(CustomerSpawnerRutine());
    }

    void AdjustGround()
    {
        float xPosition = (world.Width / 2) - 0.5f; // Hack: The -0.5f is an offset we have to set to align the ground to the tiles
        float zPosition = (world.Height / 2) - 0.5f; // Hack: The -0.5f is an offset we have to set to align the ground to the tiles

        groundTransform.position = new Vector3(xPosition, -0.001f, zPosition);
        groundTransform.localScale = new Vector3(world.Width / 10, 1, world.Height / 10);
    }

    void SpawnTiles()
    {
        for (int x = 0; x < world.Width; x++)
        {
            for (int z = 0; z < world.Height; z++)
            {
                Tile tile = world.Tiles[x, z];

                if (tile == null)
                    continue;

                GameObject prefab;
                if (tile.type == TileType.Road)
                    prefab = roadTilePrefab;
                else if (tile.type == TileType.Buildable || tile.type == TileType.Occupied)
                    prefab = buildableTilePrefab;
                else
                    throw new Exception("Not supporting tile type " + tile.type.ToString());

                GameObject tileGameObject = SpawnObject(prefab, tile.ToString(), tile.x, tile.z, tileContainer);
                TileController tileController = tileGameObject.GetComponent<TileController>();
                tileController.tile = tile;
            }
        }
    }

    void SpawnBuildings()
    {
        foreach (var building in world.Buildings)
            SpawnBuilding(building);
    }

    private void SpawnBuilding(Building building)
    {
        GameObject prefab;
        if (building is ClubBuilding)
            prefab = clubBuildingPrefab;
        else if (building is HouseBuilding)
            prefab = houseBuildingPrefab;
        else if (building is KebabBuilding)
            prefab = kebabBuildingPrefab;
		else if (building is PoliceBuilding)
			prefab = policeBuildingPrefab;
        else
            throw new Exception("Not supporting building type " + building.GetType());

        GameObject buildingGameObject = SpawnObject(prefab, building.ToString(), building.tile.x, building.tile.z, buildingContainer);
        BuildingController buildingController = buildingGameObject.GetComponent<BuildingController>();
        buildingController.SetBuilding(building);
    }

    IEnumerator CustomerSpawnerRutine()
    {
        int MaxCustomers = 300;

        while (true)
        {
            foreach (Building building in world.Buildings)
            {
                if (world.Customers.Count >= MaxCustomers)
                    yield return new WaitForSeconds(10);

                if (world.Customers.Count < MaxCustomers && building.SpawnRoll())
                {
                    Customer customer = world.CreateCustomer(building.tile.x, building.tile.z);
                    SpawnCustomer(customer, building.tile);
                }
                yield return null;
            }

            yield return 0;
        }
    }

    private void SpawnCustomer(Customer customer, Tile tile)
    {
        float spawnX = (tile.x + tile.adjacentRoadTile.x) / 2.0f;
        float spawnZ = (tile.z + tile.adjacentRoadTile.z) / 2.0f;
        GameObject customerGameObject = SpawnObject(normalCustomerPrefab, customer.ToString(), spawnX, spawnZ, customerContainer);
        CustomerController customerController = customerGameObject.GetComponent<CustomerController>();
        customerController.customer = customer;
    }

    GameObject SpawnObject(GameObject prefab, string name, float x, float z, GameObject parent)
    {
        GameObject gameObject = (GameObject)Instantiate(prefab, new Vector3(x, 0, z), Quaternion.identity);
        gameObject.name = name;
        gameObject.transform.parent = parent.transform;

        return gameObject;
    }

    public void AddAndSpawnBuilding(Building building, Tile tile)
    {
        world.AddBuilding(building, tile);
        SpawnBuilding(building);
        Debug.Log("Added and spawned building " + building.ToString());
    }

    public void ReplaceBuilding(Building oldBuilding, Building newBuilding)
    {
        world.ReplaceBuilding(oldBuilding, newBuilding);
        SpawnBuilding(newBuilding);
    }

}