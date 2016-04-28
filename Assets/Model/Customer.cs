using System.Collections;
using System.Collections.Generic;
using EpPathFinding.cs;
using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class Customer
{
    World world;

    public Guid InstanceID;


    // POSITION RELATED VARIABLES

    private int x; // OBS: The position is set before the object actually arrives @ drawing
    public int X { get { return x; } }

    private int z; // OBS: The position is set before the object actually arrives @ drawing
    public int Z { get { return z; } }

    private int originX;
    private int originZ;

    public int movingToX;
    public int movingToZ;

    private Building destinationBuilding;
    public Building DestinationBuilding { get { return destinationBuilding; } }

    public int destinationX = -1;
    public int destinationZ = -1;

    private List<GridPos> resultPath;
    private int currentResultPathIndex = 0;

    private static JumpPointParam staticlyCachedJpParam;


    // STATE VARIABLES

    private CustomerState state = CustomerState.Nothing;
    public CustomerState State { get { return state; } }

    private CustomerMood mood;
    public CustomerMood Mood { get { return mood; } }

    public float eatingUntil;
    public float moveSpeed;
    public int hunger;

    private float timeWhenLeavesQueue;

    public Customer(int x, int z, World world)
    {
        this.world = world;
        this.InstanceID = Guid.NewGuid();
        this.originX = x;
        this.originZ = z;
        this.x = x;
        this.z = z;
        this.hunger = Utils.RandomInt(0, 100);
        SetMoveSpeed();

        CacheJumpPointParam();
    }

    public bool CanChangeItsMind()
    {
        if (hunger == 0)
            return false;
        else
            return state == CustomerState.Nothing || state == CustomerState.MovingToMapEnd || state == CustomerState.MovingToOrigin;
    }

    public void TriggerArrivedAtKebabBuilding()
    {
        KebabBuilding destinationKebabBuilding = (KebabBuilding)destinationBuilding;
        if (destinationKebabBuilding.IsFull())
        {
            if (!DecideIfJoinQueue(destinationKebabBuilding))
            {
                mood = CustomerMood.AngryNoCapacity;
                GoToMapEnd();
                destinationKebabBuilding.ChangeReputation(-Settings.KebabBuilding_ReputationLostFromFull);
            }
        }
        else
            BuyAndStartEating();
    }

    public void TriggerMaybeLeaveQueueOrBuyKebab()
    {
        KebabBuilding destinationKebabBuilding = (KebabBuilding)destinationBuilding;

        if (Time.time >= timeWhenLeavesQueue)
        {
            mood = CustomerMood.AngryToLongWaitTime;
            GoToMapEnd();
            destinationKebabBuilding.ChangeReputation(-Settings.KebabBuilding_ReputationLostFromFull);
            if (!destinationKebabBuilding.customersInQueue.Remove(this))
                throw new Exception("Did not remove customer from queue. Was not found.");
        }

        if (!destinationKebabBuilding.IsFull() && destinationKebabBuilding.customersInQueue.FirstOrDefault() == this)
        {
            BuyAndStartEating();
            if (!destinationKebabBuilding.customersInQueue.Remove(this))
                throw new Exception("Did not remove customer from queue. Was not found.");
        }
    }

    public void BuyAndStartEating()
    {
        KebabBuilding destinationKebabBuilding = (KebabBuilding)destinationBuilding;
        destinationKebabBuilding.customers.Add(this);
        state = CustomerState.Eating;
        eatingUntil = Time.time + EatDuration(hunger);
        destinationKebabBuilding.ChangeReputation(Settings.KebabBuilding_ReputationGainedFromSale);
        destinationKebabBuilding.AddCashEarned(Settings.KebabBuilding_CashPerKebab);
    }

    private float EatDuration(int hunger)
    {
        return hunger / Settings.Customer_EatSpeedPerSec;
    }

    public void StopEating()
    {
        mood = CustomerMood.Normal;
        state = CustomerState.Nothing;
        hunger = 0;
        SetMoveSpeed();
        KebabBuilding destinationKebabBuilding = (KebabBuilding)destinationBuilding;
        if (!destinationKebabBuilding.customers.Remove(this))
            throw new Exception("Did not find customer in kebab building customer list when removing it.");
        DecideDestinationAndPath();
    }

    public void DecideDestinationAndPath()
    {
        if (state == CustomerState.Eating)
            throw new Exception("Should never decide new destination while eating.");

        mood = CustomerMood.Normal;

        if (hunger > 0)
        {
            destinationBuilding = DecideIfWantKebab();
            if (destinationBuilding is KebabBuilding)
                GoToKebabBuilding((KebabBuilding)destinationBuilding);
            else
                GoToMapEnd();
        }
        else
        {
            if (IsAtOrigin())
                GoToMapEnd();
            else
                GoToOrigin();
        }
    }

    private KebabBuilding DecideIfWantKebab()
    {
        int mostWantedScore = 0;
        KebabBuilding mostWantedKebabBuilding = null;

        foreach (KebabBuilding building in world.KebabBuildings)
        {
            double distance = MathUtils.Distance(x, z, building.tile.x, building.tile.z);
            int buildingWantScore = hunger + building.Reputation - Convert.ToInt32(distance);

            if (buildingWantScore >= mostWantedScore)
            {
                mostWantedScore = buildingWantScore;
                mostWantedKebabBuilding = building;
            }
        }

        if (mostWantedKebabBuilding != null)
            return mostWantedKebabBuilding;
        else
        {
            mood = CustomerMood.SkippingKebabToday;
            return null;
        }
    }

    private bool DecideIfJoinQueue(KebabBuilding kebabBuilding)
    {
        int decisionValue   = hunger - 50; // Since 50 should be the average
        decisionValue       += kebabBuilding.Reputation;
        decisionValue       -= kebabBuilding.customersInQueue.Count;
        decisionValue       += Utils.RandomInt(-10, 10);

        bool decision = decisionValue >= 0;
        if (decision)
        {
            state = CustomerState.Queued;
            kebabBuilding.customersInQueue.Add(this);
            timeWhenLeavesQueue = Time.time + decisionValue;
        }

        //Debug.Log(string.Format("Decision value {0}, hunger {1}, building rep {2}, queue {3}", decisionValue, hunger, kebabBuilding.Reputation, kebabBuilding.customersInQueue.Count));
        return decision;
    }

    private void GoToKebabBuilding(KebabBuilding kebabBuilding)
    {
        destinationX = kebabBuilding.tile.x;
        destinationZ = kebabBuilding.tile.z;
        state = CustomerState.MovingToEat;
        FindPath();
    }

    private void GoToMapEnd()
    {
        Tile destinationTile = world.RoadTiles.Where(t => t.isWorldEdge).OrderBy(t => MathUtils.Distance(t.x, t.z, x, z)).First();
        destinationX = destinationTile.x;
        destinationZ = destinationTile.z;
        state = CustomerState.MovingToMapEnd;
        destinationBuilding = null;
        FindPath();
    }

    private void GoToOrigin()
    {
        destinationX = originX;
        destinationZ = originZ;
        state = CustomerState.MovingToOrigin;
        destinationBuilding = null;
        FindPath();
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

    private void FindPath()
    {
        CacheJumpPointParam();

        GridPos firstRoadPos = FindWalkableTile(x, z);
        GridPos lastRoadPos = FindWalkableTile(destinationX, destinationZ);
        GridPos destinationPos = new GridPos(destinationX, destinationZ);
        staticlyCachedJpParam.Reset(firstRoadPos, lastRoadPos);

        resultPath = new List<GridPos>();
        resultPath.AddRange(JumpPointFinder.FindPath(staticlyCachedJpParam));
        resultPath.Add(destinationPos);
        currentResultPathIndex = 0;

        if (resultPath.Count == 0)
            throw new Exception("Path finding algorithm failed. Did not find any path. Should not happen.");
        else
            SetNextMovingToPosition();
    }

    private GridPos FindWalkableTile(int x, int z)
    {
        Tile tile = world.Tiles[x, z];
        if (tile.type != TileType.Road)
            return new GridPos(tile.adjacentRoadTile.x, tile.adjacentRoadTile.z);
        else
            return new GridPos(tile.x, tile.z);
    }

    private void CacheJumpPointParam()
    {
        if (staticlyCachedJpParam == null)
        {
            BaseGrid searchGrid = new StaticGrid(world.Width, world.Height);

            foreach (Tile roadTile in world.RoadTiles)
                searchGrid.SetWalkableAt(roadTile.x, roadTile.z, true);

            staticlyCachedJpParam = new JumpPointParam(searchGrid);
        }
    }

    private void SetMoveSpeed()
    {
        moveSpeed = (Settings.Customer_MovementSpeed / 2.0f) + (Settings.Customer_MovementSpeed * hunger / 100.0f);
    }

    private bool IsAtOrigin()
    {
        return (x == originX && z == originZ);
    }

}

public enum CustomerState
{
    Nothing,
    MovingToEat,
    MovingToMapEnd,
    MovingToOrigin,
    Queued,
    Eating,
}

public enum CustomerMood
{
    Normal,
    AngryNoCapacity,
    AngryToLongWaitTime,
    SkippingKebabToday,
}