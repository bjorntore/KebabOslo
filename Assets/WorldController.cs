using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;

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

    private GameObject tileContainer;
    private GameObject buildingContainer;
    private GameObject customerContainer;

    public World world;

    public WorldTimeController worldTimeController;

    private int customerSpawnerBIndex = 0;

    private static WorldController worldController;
    public static WorldController Instance()
    {
        if (!worldController)
        {
            worldController = FindObjectOfType(typeof(WorldController)) as WorldController;
            if (!worldController)
                Debug.LogError("There needs to be one active WorldController script on a GameObject in your scene.");
        }

        return worldController;
    }

    // Use this for initialization
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

        StartCoroutine(CustomerSpawnerRutine());

        Time.timeScale = 1.0f;
    }

    // Update is called once per frame
    private void Update()
    {
        if (world.player.CheckIfLost(worldTimeController.day))
        {
            GenericDialog panel = GenericDialog.Instance();
            panel.OpenDialog("LOST!", "You ran out of money and local gangsters got pissed off and wasted you!");
            Time.timeScale = 0.0f;
        }
    }

    public void AddAndSpawnKebabBuilding(KebabBuilding building, Tile tile)
    {
        StopCoroutine(CustomerSpawnerRutine());

        if (tile.type == TileType.Buildable)
            Destroy(GameObject.Find(tile.ToString()));

        world.AddBuilding(building, tile);
        SpawnBuilding(building);
        StartCoroutine(world.SetNewCustomerDestinations(tile.x, tile.z));

        StartCoroutine(CustomerSpawnerRutine());
    }

    public void ReplaceBuilding(Building oldBuilding, Building newBuilding)
    {
        world.ReplaceBuilding(oldBuilding, newBuilding);
        SpawnBuilding(newBuilding);
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

    private IEnumerator CustomerSpawnerRutine()
    {
        while (true)
        {
            for (customerSpawnerBIndex = 0; customerSpawnerBIndex < world.Buildings.Count; customerSpawnerBIndex++)
            {
                Building building = world.Buildings[customerSpawnerBIndex];

                if (world.Customers.Count >= Settings.World_MaxCustomers)
                    yield return new WaitForSeconds(5);

                if (world.Customers.Count < Settings.World_MaxCustomers && building.CustomerSpawnRoll())
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
        //float spawnX = (tile.x + tile.adjacentRoadTile.x) / 2.0f;
        //float spawnZ = (tile.z + tile.adjacentRoadTile.z) / 2.0f;
        GameObject customerGameObject = SpawnObject(normalCustomerPrefab, customer.ToString(), tile.x, tile.z, customerContainer);
        CustomerController customerController = customerGameObject.GetComponent<CustomerController>();
        customerController.SetCustomer(customer);
    }

    private GameObject SpawnObject(GameObject prefab, string name, float x, float z, GameObject parent)
    {
        GameObject gameObject = (GameObject)Instantiate(prefab, new Vector3(x, 0, z), Quaternion.identity);
        gameObject.name = name;
        gameObject.transform.parent = parent.transform;

        return gameObject;
    }

}