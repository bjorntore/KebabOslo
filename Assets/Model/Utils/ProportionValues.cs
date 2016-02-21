using System.Collections.Generic;
using UnityEngine;

public class ProportionValues
{
    ProportionItem[] proportionItems;
    int weightTotal;

    public ProportionValues(ProportionItem[] proportionItems)
    {
        this.proportionItems = proportionItems;

        weightTotal = 0;
        foreach (ProportionItem item in proportionItems)
        {
            weightTotal += item.Weight;
        }
    }

    public ProportionItem RandomChoice()
    {
        int result = 0, total = 0;
        int randVal = UnityEngine.Random.Range(0, weightTotal + 1);

        for (result = 0; result < proportionItems.Length; result++)
        {
            total += proportionItems[result].Weight;
            if (total >= randVal) break;
        }
        return proportionItems[result];
    }
}

public class ProportionItem
{
    string name;
    public string Name { get { return name; } }

    int weight;
    public int Weight { get { return weight; } }

    public ProportionItem(string name, int weight)
    {
        this.name = name;
        this.weight = weight;
    }
}
