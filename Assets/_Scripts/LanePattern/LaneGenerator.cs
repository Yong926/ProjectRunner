using System.Collections.Generic;
using UnityEngine;

public class LaneGenerator
{
    private List<Lane> lanePatterns = new List<Lane>();

    private Vector2 limitQuota;
    private int currentQuota = 0;
    private int laneCount;

    [HideInInspector] public Lane currentPattern;
    private RandomGenerator randomGenerator = new RandomGenerator();

    public LaneGenerator(int Count, Vector2 quota, List<LanePatternPool> pool)
    {
        laneCount = Count;
        limitQuota = quota;

        lanePatterns.Add(new LaneStraight());
        lanePatterns.Add(new LaneWave());
        lanePatterns.Add(new LaneZigzag());

        foreach (var p in pool)
            randomGenerator.AddItem(p);

        SwitchPattern();
    }

    public LaneData GetNextLane()
    {
        currentQuota++;

        if (currentQuota >= Random.Range((int)limitQuota.x, (int)limitQuota.y))
            SwitchPattern();

        if (currentPattern == null)
            return new LaneData(-1);

        return currentPattern.GetNextLane();
    }

    public void SwitchPattern(int index = -1)
    {
        string patternName = randomGenerator.GetRandom().GetItem() as string;

        Lane lanePattern = lanePatterns.Find(f => f.Name == patternName);
        currentPattern = lanePattern;
        currentPattern?.Initialize(laneCount);

        currentQuota = 0;
    }
}