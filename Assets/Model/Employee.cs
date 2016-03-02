using System;

[Serializable]
public class Employee  {

	private string name;
	public string Name { get { return name; } }

	public Employee(string name){
		this.name = name;
	}
}
