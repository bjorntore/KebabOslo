using System;
using System.Linq;

[Serializable]
public class Ingredient
{
    private string name;
    public string Name { get { return name; } }

    string qualityDescription;
    public string QualityDescription { get { return qualityDescription; } }

    private int qualityValue;
    public int QualityValue { get { return qualityValue; } }

    private int cost;
    public int Cost { get { return cost; } }

    public Ingredient(string name, string qualityDescription, int qualityValue, int cost)
    {
        this.name = name;
        this.qualityDescription = qualityDescription;
        this.qualityValue = qualityValue;
        this.cost = cost;
    }
}