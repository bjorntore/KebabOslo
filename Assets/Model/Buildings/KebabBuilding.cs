using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class KebabBuilding : Building
{
    private World world;

    private float spawnCooldown = 0;
    public override float SpawnCooldown { get { return spawnCooldown; } }

    public override float LastSpawnTimed { get; set; }

    public List<Customer> customers = new List<Customer>();
    public List<Employee> employees = new List<Employee>();

    private int maintenancePerDay = 10;
    private int lastMaintenancePayDay = 0;

    public int cashEarned = 0;
    public bool cashEarnedTrigger = false;

    private int lastEmployeePayDay = 0;

    public KebabBuilding(Tile tile, World world) : base(tile)
    {
        this.world = world;
    }

    public bool IsFull()
    {
        return customers.Count >= GetCurrentCapacity();
    }

    public int GetCurrentCapacity()
    {
        return Settings.KebabBuilding_CustomerCapacity * employees.Count;
    }

    public void HireEmployee()
    {
        if (employees.Count == Settings.KebabBuilding_MaxEmployees)
            return;

        employees.Add(new Employee("Dude"));
        world.player.ChangeCash(Settings.Employee_HireCost);

    }

    /* TODO: Fire last employed person for now, but change to fire spesific at a later point when we implement GUI for that */
    public void FireEmployee()
    {
        if (employees.Count > 0)
            employees.Remove(employees.Last());
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

    public void CheckAndTriggerEmployeeWages(int currentDay)
    {
        if (employees.Count == 0)
            return;

        int daysSinceLastEmployeePayDay = currentDay - lastEmployeePayDay;
        if (daysSinceLastEmployeePayDay > 0)
        {
            foreach (Employee employee in employees)
            {
                world.player.ChangeCash(-Settings.Employee_DailyCost * daysSinceLastEmployeePayDay);               
                /* Doing foreach employee so that we can do stuff to the employee object if we want to. */
            }
            lastEmployeePayDay += daysSinceLastEmployeePayDay;
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
