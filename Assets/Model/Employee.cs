using System;

[Serializable]
public class Employee  {

	private string name;
	public string Name { get { return name; } }

    private int totalPaidSalery;
    public int TotalPaidSalery { get { return totalPaidSalery; } }

	public Employee(string name){
		this.name = name;
	}

    public void ChangeTotalPaidSalery(int change)
    {
        totalPaidSalery += change;
    }
}
