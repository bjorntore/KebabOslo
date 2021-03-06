﻿using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;
using System.Collections.Generic;

public class WorldController : MonoBehaviour
{
    public Transform groundTransform;
    public GameObject roadTilePrefab;
    public GameObject buildableTilePrefab;

    public GameObject houseBuildingPrefab;
    public GameObject villaBuildingPrefab;
    public GameObject clubBuildingPrefab;
    public GameObject kebabBuildingPrefab;
    public GameObject policeBuildingPrefab;

    public GameObject normalCustomerPrefab;
    public ObjectPool normalCustomerObjectPool;

    private GameObject tileContainer;
    private GameObject buildingContainer;
    private GameObject customerContainer;

    public World world;

    public WorldTimeController worldTimeController;
    public static WorldController instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
        {
            Debug.LogError("Tried to created another instance of " + GetType() + ". Destroying.");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Player player = new Player("ShrubNub");
        world = new World(player);
        tileContainer = new GameObject("TileContainer");
        buildingContainer = new GameObject("BuildingContainer");
        customerContainer = new GameObject("CustomerContainer");
        worldTimeController = FindObjectOfType<WorldTimeController>();

        AdjustGround();
        SpawnTiles();
        SpawnBuildings();

        normalCustomerObjectPool = new ObjectPool(normalCustomerPrefab, 300, customerContainer);
        StartCoroutine("CustomerSpawnerRutine");

        Time.timeScale = 1.0f;
    }

    private void Update()
    {
        if (world.player.CheckIfLost(worldTimeController.day))
        {
            GenericDialog panel = GenericDialog.instance;
            panel.OpenDialog("LOST!", "You ran out of money and local gangsters got pissed off and wasted you!");
            Time.timeScale = 0.0f;
        }
    }

    public void AddAndSpawnKebabBuilding(KebabBuilding building, Tile tile)
    {
        StopCoroutine("CustomerSpawnerRutine");

        if (tile.type == TileType.Buildable)
            Destroy(GameObject.Find(tile.ToString()));

        world.AddBuilding(building, tile);
        SpawnBuilding(building);
        StartCoroutine(world.SetNewCustomerDestinations(tile.x, tile.z));

        StartCoroutine("CustomerSpawnerRutine");
    }

    public void ReplaceBuilding(Building oldBuilding, Building newBuilding)
    {
        StopCoroutine("CustomerSpawnerRutine");

        world.ReplaceBuilding(oldBuilding, newBuilding);
        SpawnBuilding(newBuilding);

        StartCoroutine("CustomerSpawnerRutine");
    }

    public void DeleteKebabBuilding(KebabBuilding building)
    {
        world.DeleteBuilding(building, building.tile);

        GameObject tileGameObject = SpawnObject(buildableTilePrefab, building.tile.ToString(), building.tile.x, building.tile.z, tileContainer);
        TileController tileController = tileGameObject.GetComponent<TileController>();
        tileController.tile = building.tile;

        StartCoroutine(world.SetNewCustomerDestinations(building.tile.x, building.tile.z));
    }

    private void AdjustGround()
    {
        float xPosition = (world.Width / 2) - 0.5f; // Hack: The -0.5f is an offset we have to set to align the ground to the tiles
        float zPosition = (world.Height / 2) - 0.5f; // Hack: The -0.5f is an offset we have to set to align the ground to the tiles

        groundTransform.position = new Vector3(xPosition, -0.001f, zPosition);
        groundTransform.localScale = new Vector3(world.Width / 10, 1, world.Height / 10);
    }

    private void SpawnTiles()
    {
        for (int x = 0; x < world.Width; x++)
        {
            for (int z = 0; z < world.Height; z++)
            {
                Tile tile = world.Tiles[x, z];

                if (tile == null || tile.type == TileType.Occupied)
                    continue;

                GameObject prefab;
                if (tile.type == TileType.Road)
                    prefab = roadTilePrefab;
                else if (tile.type == TileType.Buildable)
                    prefab = buildableTilePrefab;
                else
                    throw new Exception("Not supporting tile type " + tile.type.ToString());

                GameObject tileGameObject = SpawnObject(prefab, tile.ToString(), tile.x, tile.z, tileContainer);
                TileController tileController = tileGameObject.GetComponent<TileController>();
                tileController.tile = tile;
            }
        }
    }

    private void SpawnBuildings()
    {
        foreach (var building in world.Buildings)
            SpawnBuilding(building);
    }

    private void SpawnBuilding(Building building)
    {
        GameObject prefab;
        if (building is HouseBuilding)
            prefab = houseBuildingPrefab;
        else if (building is VillaBuilding)
            prefab = villaBuildingPrefab;
        else if (building is ClubBuilding)
            prefab = clubBuildingPrefab;
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

    /// <summary>
    /// This method is and should only be started using the StartCoroutine(string) method. If a the name of the method in
    /// string is not used. Then we cant stop it.
    /// ref: http://answers.unity3d.com/questions/537591/how-do-i-stop-coroutine.html
    /// </summary>
    private IEnumerator CustomerSpawnerRutine()
    {
        while (true)
        {
            foreach (NeutralBuilding naturalBuilding in world.customerSpawner.GetSpawningBuildings())
            {
                Customer customer = world.CreateCustomer(naturalBuilding.tile);
                SpawnCustomer(customer, naturalBuilding.tile);
            }

            yield return 0;
        }
    }

    private void SpawnCustomer(Customer customer, Tile tile)
    {
        GameObject customerGameObject = normalCustomerObjectPool.GetObject();
        customerGameObject.transform.position = new Vector3(tile.x, 0, tile.z);

        CustomerController customerController = customerGameObject.GetComponent<CustomerController>();
        customerController.SetCustomer(customer);

        customerGameObject.SetActive(true);
    }

    private GameObject SpawnObject(GameObject prefab, string name, float x, float z, GameObject parent)
    {
        GameObject gameObject = (GameObject)Instantiate(prefab, new Vector3(x, 0, z), Quaternion.identity);
        gameObject.name = name;
        gameObject.transform.parent = parent.transform;

        return gameObject;
    }

}