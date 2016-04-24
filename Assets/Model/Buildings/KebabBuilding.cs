﻿using UnityEngine;
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

    public KebabMenu kebabMenu = new KebabMenu();
    public List<Customer> customers = new List<Customer>();
    public List<Employee> employees = new List<Employee>();

    private int lastMaintenancePayDay = 0;

    public int cashEarned = 0;
    public bool cashEarnedTrigger = false;

    private int lastEmployeePayDay = 0;

    public KebabBuilding(Tile tile, World world) : base(tile)
    {
        this.world = world;
        employees.Add(new Employee("Dude"));
    }

    public bool IsFull()
    {
        return customers.Count >= GetCurrentCapacity();
    }

    public int GetCurrentCapacity()
    {
        return Settings.KebabBuilding_CustomerCapacityPerEmployee * employees.Count;
    }

    public void HireEmployee()
    {
        if (employees.Count == Settings.KebabBuilding_MaxEmployees)
            return;
                
        employees.Add(new Employee("Dude"));
        world.player.ChangeCash(-Settings.Employee_HireCost);
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

    public void TriggerExpences(int currentDay)
    {
        CheckAndTriggerMaintenance(currentDay);
        CheckAndTriggerEmployeeWages(currentDay);
    }

    private void CheckAndTriggerMaintenance(int currentDay)
    {
        int daysSinceLastMaintenance = currentDay - lastMaintenancePayDay;
        int interval = Settings.KebabBuilding_DaysBetweenMaintenance;
        SanityCheckDaysIntervalSetting(interval);

        if (daysSinceLastMaintenance - interval > 0)
        {
            world.player.ChangeCash(-Settings.KebabBuilding_MaintenanceCostPerDay * daysSinceLastMaintenance);
            lastMaintenancePayDay += daysSinceLastMaintenance;

            Debug.Log("Maintenance");
        }
    }

    private void CheckAndTriggerEmployeeWages(int currentDay)
    {
        if (employees.Count == 0)
            return;

        int interval = Settings.Employee_DaysBetweenEmployeeWages;
        SanityCheckDaysIntervalSetting(interval);

        int daysSinceLastEmployeePayDay = currentDay - lastEmployeePayDay;
        if (daysSinceLastEmployeePayDay - interval > 0)
        {
            foreach (Employee employee in employees)
            {
                world.player.ChangeCash(-Settings.Employee_WageCostPerDay * daysSinceLastEmployeePayDay);               
                /* Doing foreach employee so that we can do stuff to the employee object if we want to. */
            }
            lastEmployeePayDay += daysSinceLastEmployeePayDay;

            Debug.Log("Payday");
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

    private void SanityCheckDaysIntervalSetting(int interval)
    {
        if (interval < 1)
            throw new ArgumentOutOfRangeException("Days interval setting can not be less than 1 day.");
    }

}
