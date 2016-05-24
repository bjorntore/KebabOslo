using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class KebabBuilding : Building
{
    private World world;

    private float customerSpawnCooldown = 0;
    public override float CustomerSpawnCooldown { get { return customerSpawnCooldown; } }

    public KebabMenu kebabMenu = new KebabMenu();
    public List<Customer> customers = new List<Customer>();
    public List<Customer> customersInQueue = new List<Customer>();
    public List<Employee> employees = new List<Employee>();

    private int lastMaintenancePayDay = 0;
    private int lastEmployeePayDay = 0;

    public int cashEarned = 0;
    public bool cashEarnedTrigger = false;

    private int reputation = 0;
    public int Reputation { get { return reputation; } }

    public KebabBuilding(World world) : base()
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
                var salery = -Settings.Employee_WageCostPerDay * daysSinceLastEmployeePayDay;
                world.player.ChangeCash(salery);
                employee.ChangeTotalPaidSalery(salery);
            }
            lastEmployeePayDay += daysSinceLastEmployeePayDay;

            Debug.Log("Payday");
        }
    }

    public override int ReplaceCost()
    {
        throw new Exception("Not possible to replace kebab building");
    }

    public void ChangeReputation(int change)
    {
        reputation += change;
    }

    private void SanityCheckDaysIntervalSetting(int interval)
    {
        if (interval < 1)
            throw new ArgumentOutOfRangeException("Days interval setting can not be less than 1 day.");
    }

    public void RejectCustomers()
    {
        customers.ForEach(c => c.RejectFromKebabBuilding());
        customersInQueue.ForEach(c => c.RejectFromKebabBuilding());
        world.Customers.Where(c => c.DestinationBuilding == this).Select(c => c).ToList().ForEach(c => c.RejectFromKebabBuilding());
    }
}
