using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public class KebabBuilding : Building
{
	private World world;

    float spawnCooldown = 0;
    public override float SpawnCooldown { get { return spawnCooldown; } }

    public override float LastSpawnTimed { get; set; }

    public List<Customer> customers = new List<Customer>();

	public List<Employee> employees = new List<Employee> ();

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

    public override int Cost()
    {
        return -1; // Not possible
    }

}
