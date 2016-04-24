using System;
using System.Collections;
using System.Linq;

[Serializable]
public class MenuItem
{
    public Ingredient MeatType { get; set; }
    public Ingredient VegetableType { get; set; }
    public Ingredient SauceType { get; set; }
    public int Price { get; set; }

    public MenuItem()
    {
        MeatType = IngredientDB.Meats.OrderBy(x => x.Cost).First();
        VegetableType = IngredientDB.Vegetables.OrderBy(x => x.Cost).First();
        SauceType = IngredientDB.Sauces.OrderBy(x => x.Cost).First();
    }

    public int GetProductionCost() { throw new System.NotImplementedException(); }
    public int GetTasteScore() { throw new System.NotImplementedException(); }
}
