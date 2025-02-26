using UnityEngine;

public class LaneZigzag : Lane
{
    public string Name => "ZigzagPattern";
    private int elapsed = 0;

    private LaneData data;

    public void Initialize(int maxlane)
    {
        data.maxLane = maxlane;
    }
    public LaneData GetNextLane()
    {
        data.currentLane = (int)Mathf.PingPong(elapsed++, data.maxLane - 1);

        return data;
    }

}