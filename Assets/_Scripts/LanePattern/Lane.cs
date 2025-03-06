
public enum LaneType { EMPTY, STRAIGHT, WAVE, ZIGZAG }

public interface Lane
{
    public LaneType laneType { get; }
    public void Initialize(int maxlane);
    public LaneData GetNextLane();
}