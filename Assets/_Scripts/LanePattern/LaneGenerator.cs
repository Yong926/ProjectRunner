using System.Collections.Generic;
using UnityEngine;

public class LaneGenerator
{
    private List<Lane> lanePatterns = new List<Lane>();

    private Vector2 limitQuota;
    private int currentQuota = 0;
    private int laneCount;

    [HideInInspector] public Lane currentPattern;

    public LaneGenerator(int Count, Vector2 quota)
    {
        laneCount = Count;
        limitQuota = quota;

        lanePatterns.Add(new LaneStraight());
        lanePatterns.Add(new LaneWave());
        lanePatterns.Add(new LaneZigzag());

        SwitchPattern();
    }

    public LaneData GetNextLane()
    {
        currentQuota++;

        if (currentQuota >= Random.Range((int)limitQuota.x, (int)limitQuota.y))
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