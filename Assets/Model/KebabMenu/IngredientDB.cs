using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IngredientDB {

    public static readonly List<Ingredient> Meats = new List<Ingredient>()
    {
        new Ingredient(@"Rat", "Terrible", 0, 3),
        new Ingredient(@"Cat", "Very low", 1, 4),
        new Ingredient(@"""Halal""", "Low", 2, 6),
        new Ingredient(@"Halal", "Medium", 4, 10),
        new Ingredient(@"Doner", "Good", 6, 12),
        new Ingredient(@"Chicken", "Very good", 8, 14),
        new Ingredient(@"Beef", "Excellent", 10, 18)
    };

    public static readonly List<Ingredient> Vegetables = new List<Ingredient>()
    {
        new Ingredient(@"Shady illegal import", "Terrible", 0, 3),
        new Ingredient(@"Imported by bro", "Terrible", 1, 4),
        new Ingredient(@"Basement grown", "Very low", 2, 6),        
        new Ingredient(@"Grocery store", "Medium", 4, 10),
        new Ingredient(@"Vegetable store", "Good", 6, 12),
        new Ingredient(@"Gourmet store", "Very good", 8, 14),
        new Ingredient(@"Local farm", "Excellent", 10, 18)
    };

    public static readonly List<Ingredient> Sauces = new List<Ingredient>()
    {
        new Ingredient(@"Made in bros basement", "Terrible", 0, 3),
        new Ingredient(@"Expired salad dressing", "Very low", 1, 4),
        new Ingredient(@"Hamburger dressing", "Low", 2, 6),
        new Ingredient(@"Garlic dressing", "Medium", 4, 10),
        new Ingredient(@"Kebab sauce from store", "Good", 6, 12),
        new Ingredient(@"Homemade kebab sauce", "Very good", 8, 14),
        new Ingredient(@"Special recipe", "Excellent", 10, 18)
    };
}
