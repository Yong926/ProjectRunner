using System.Collections.Generic;
using UnityEngine;

public abstract class RandomItem
{
    public string name;
    public int weight;
    public abstract Object GetItem();
}

public class RandomGenerator
{
    public List<RandomItem> items = new List<RandomItem>();

    public int totalweight;

    void CalcTotalWeight()
    {
        totalweight = 0;

        foreach (var item in items)
        {
            totalweight += item.weight;
        }
    }

    public RandomItem GetRandom()
    {
        int rnd = Random.Range(0, totalweight);
        int weightSum = 0;

        foreach (var i in items)
        {
            weightSum += i.weight;

            if (rnd < weightSum)
                return i;
        }

        return null;
    }

    public void AddItem(RandomItem item)
    {
        items.Add(item);

        CalcTotalWeight();
    }
}