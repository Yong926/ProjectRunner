using System.Collections.Generic;
using UnityEngine;

public class LaneGenerator
{
    private List<Lane> lanePatterns = new List<Lane>();

    private int limitQuota = 10;
    private int currentQuota = 0;
    private int laneCount;

    [HideInInspector] public Lane currentPattern;

    public LaneGenerator(int Count, int quota)
    {
        limitQuota = quota;
        laneCount = Count;
        lanePatterns.Add(new LaneStraight());
        // lanePatterns.Add(new LaneWave());

        currentPattern = lanePatterns[0];
    }

    public int GetNextLane()
    {
        if (currentPattern == null)
            return -1;

        currentQuota++;

        if (currentQuota >= limitQuota)
            SwitchPattern();


        return currentPattern.GetNextLane();
    }

    public void SwitchPattern(int index = -1)
    {
        int i = index == -1 ? Random.Range(0, lanePatterns.Count) : Mathf.Clamp(index, 0, lanePatterns.Count - 1);

        Lane lanePattern = lanePatterns[i];
        currentPattern = lanePattern;
        currentPattern.Initialize(laneCount);

        currentQuota = 0;
    }
}