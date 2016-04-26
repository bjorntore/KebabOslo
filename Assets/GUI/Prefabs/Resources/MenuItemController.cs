using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Collections.Generic;

public class MenuItemController : MonoBehaviour {

    public InputField nameInputField;
    public Dropdown meatDropdown;
    public Dropdown vegetableDropdown;
    public Dropdown sauceDropdown;
    public Text costLabel;
    public InputField priceInputField;

    public MenuItem menuItem;

    public void SetMenuItem(MenuItem newMenuItem)
    {
        menuItem = newMenuItem;
        SetupDropdowns();
        CopyModelDataToGUI();
    }
    
    private void CopyModelDataToGUI()
    {
        nameInputField.text = menuItem.Name;
        meatDropdown.value = ConvertIngredientToOptionValue(meatDropdown, menuItem.MeatType.Name); //meatDropdown.options.FindIndex(o => o.text == menuItem.MeatType.Name);
        vegetableDropdown.value = ConvertIngredientToOptionValue(vegetableDropdown, menuItem.VegetableType.Name);
        sauceDropdown.value = ConvertIngredientToOptionValue(sauceDropdown, menuItem.SauceType.Name);
        costLabel.text = menuItem.GetProductionCost().ToString();
        priceInputField.text = menuItem.Price.ToString();
    }

    internal void DiscardChanges()
    {
        CopyModelDataToGUI();
    }

    internal void SaveChanges()
    {
        menuItem.Name = nameInputField.text;
        menuItem.MeatType = ConvertDropdownValueToIngredient(meatDropdown, IngredientDB.Meats);
        menuItem.VegetableType = ConvertDropdownValueToIngredient(vegetableDropdown, IngredientDB.Vegetables);
        menuItem.SauceType = ConvertDropdownValueToIngredient(sauceDropdown, IngredientDB.Sauces);
        menuItem.Price = double.Parse(priceInputField.text);
        menuItem.IsActive = true;
    }

    private void SetupDropdowns()
    {
        IngredientDB.Meats.ForEach(x => meatDropdown.options.Add(new Dropdown.OptionData(x.Name)));
        IngredientDB.Vegetables.ForEach(x => vegetableDropdown.options.Add(new Dropdown.OptionData(x.Name)));
        IngredientDB.Sauces.ForEach(x => sauceDropdown.options.Add(new Dropdown.OptionData(x.Name)));

        /* Refresh should be done when changing options for dropdown. */
        meatDropdown.RefreshShownValue();
        vegetableDropdown.RefreshShownValue();
        sauceDropdown.RefreshShownValue();

        meatDropdown.onValueChanged.RemoveAllListeners();
        meatDropdown.onValueChanged.AddListener(UpdateCostInGUI);

        vegetableDropdown.onValueChanged.RemoveAllListeners();
        vegetableDropdown.onValueChanged.AddListener(UpdateCostInGUI);

        sauceDropdown.onValueChanged.RemoveAllListeners();
        sauceDropdown.onValueChanged.AddListener(UpdateCostInGUI);
    }

    private void UpdateCostInGUI(int arg0)
    {
        var tempMenuItem = new MenuItem();
        tempMenuItem.MeatType = ConvertDropdownValueToIngredient(meatDropdown, IngredientDB.Meats);
        tempMenuItem.VegetableType = ConvertDropdownValueToIngredient(vegetableDropdown, IngredientDB.Vegetables);
        tempMenuItem.SauceType = ConvertDropdownValueToIngredient(sauceDropdown, IngredientDB.Sauces);

        costLabel.text = tempMenuItem.GetProductionCost().ToString();
    }

    private int ConvertIngredientToOptionValue(Dropdown dropdown, string ingredientName)
    {
        return dropdown.options.FindIndex(o => o.text == ingredientName);
    }

    private Ingredient ConvertDropdownValueToIngredient(Dropdown dropdown, List<Ingredient> ingredientList)
    {
        var result = ingredientList.FirstOrDefault(i => i.Name == dropdown.options[dropdown.value].text);
        if (result == null)
            throw new Exception("Ingredient list did not contain selected option. Wrong combo of dropdown and ingredient list?");

        return result;
    }
}
