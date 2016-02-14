﻿using UnityEngine;
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

    public World world;
    GameObject tileContainer;
    GameObject buildingContainer;

    // Use this for initialization
    void Start()
    {
        world = new World();
        tileContainer = new GameObject("TileContainer");
        buildingContainer = new GameObject("BuildingContainer");

        AdjustGround();
        SpawnTiles();
        SpawnBuildings();
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
                else if (tile.type == TileType.Buildable)
                    prefab = buildableTilePrefab;
                else
                    throw new Exception("Not supporting tile type " + tile.type.ToString());

                SpawnObject(prefab, tile.ToString(), tile.x, tile.z, tileContainer);
            }
        }
    }

    void SpawnBuildings()
    {
        foreach (var building in world.buildings)
        {
            GameObject prefab;
            if (building is ClubBuilding)
                prefab = clubBuildingPrefab;
            else if (building is HouseBuilding)
                prefab = houseBuildingPrefab;
            else
                throw new Exception("Not supporting building type " + building.GetType());

            SpawnObject(prefab, building.ToString(), building.x, building.z, buildingContainer);
              
        }

    }

    void SpawnObject(GameObject prefab, string name, int x, int z, GameObject parent=null)
    {
        GameObject tileGameObject = (GameObject)Instantiate(prefab, new Vector3(x, 0, z), Quaternion.identity);
        tileGameObject.name = name;
        tileGameObject.transform.parent = parent.transform;
    }

}

