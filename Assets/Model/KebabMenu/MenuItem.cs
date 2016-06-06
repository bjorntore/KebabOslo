using System;
using System.Collections;
using System.Linq;

[Serializable]
public class MenuItem
{
    public string Name;
    public Ingredient MeatType;
    public Ingredient VegetableType;
    public Ingredient SauceType;
    public int Price;
    public bool IsActive;

    public int sales = 0;

    public MenuItem()
    {
        Name = "Kebab";
        MeatType = IngredientDB.Meats.OrderBy(x => x.Cost).First();
        VegetableType = IngredientDB.Vegetables.OrderBy(x => x.Cost).First();
        SauceType = IngredientDB.Sauces.OrderBy(x => x.Cost).First();
        Price = Convert.ToInt32(GetProductionCost() * 1.3);
        IsActive = false;
    }

    public int GetProductionCost()
    {
        return MeatType.Cost + VegetableType.Cost + SauceType.Cost;
    }
    public int GetTasteScore() { throw new System.NotImplementedException(); }
}
