using System;
using System.Collections;
using System.Linq;

[Serializable]
public class MenuItem
{
    public string Name { get; set; }
    public Ingredient MeatType { get; set; }
    public Ingredient VegetableType { get; set; }
    public Ingredient SauceType { get; set; }
    public double Price { get; set; }

    public MenuItem()
    {
        Name = "Kebab";
        MeatType = IngredientDB.Meats.OrderBy(x => x.Cost).First();
        VegetableType = IngredientDB.Vegetables.OrderBy(x => x.Cost).First();
        SauceType = IngredientDB.Sauces.OrderBy(x => x.Cost).First();
        Price = GetProductionCost() * 1.25;
    }

    public double GetProductionCost()
    {
        return MeatType.Cost + VegetableType.Cost + SauceType.Cost;
    }
    public int GetTasteScore() { throw new System.NotImplementedException(); }
}
