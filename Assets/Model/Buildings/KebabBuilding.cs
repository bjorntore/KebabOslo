using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public class KebabBuilding : Building
{
	private World world;

    private float spawnCooldown = 0;
    public override float SpawnCooldown { get { return spawnCooldown; } }

    public override float LastSpawnTimed { get; set; }

    public List<Customer> customers = new List<Customer>();
	public List<Employee> employees = new List<Employee> ();

    private int maintenancePerDay = 10;
    private int lastMaintenancePayDay = 0;

    public int cashEarned = 0;
    public bool cashEarnedTrigger = false;

	public KebabBuilding(Tile tile, World world) : base(tile) 
	{
		this.world = world;
	}

    public bool IsFull()
    {
		return customers.Count >= Settings.KebabBuilding_CustomerCapacity;
    }

	public void HireEmployee()
	{
		employees.Add(new Employee("Dude"));
		world.player.ChangeCash(Settings.Employee_HireCost);
	}

    public void AddCashEarned(int cash)
    {
        cashEarned += cash;
        world.player.ChangeCash(cash);
        cashEarnedTrigger = true;
    }

    public void CheckAndTriggerMaintenance(int currentDay)
    {
        int daysSinceLastMaintenance = currentDay - lastMaintenancePayDay;
        if (daysSinceLastMaintenance > 0)
        {
            world.player.ChangeCash(-maintenancePerDay * daysSinceLastMaintenance);
            lastMaintenancePayDay += daysSinceLastMaintenance;
        }
    }

    public override int Cost()
    {
        return -1; // Not possible
    }

    public int GetOwnerReputation()
    {
        return world.player.Reputation;
    }

}
