public class LaneStraight : Lane
{
    public string Name => "StraightPattern";

    private int currentLane;

    public void Initialize(int maxlane)
    {
        currentLane = UnityEngine.Random.Range(0, maxlane);
    }

    public int GetNextLane()
    {
        return currentLane;
    }

    public int count;
}