public interface Lane
{
    public string Name { get; }
    public void Initialize(int maxlane);
    public LaneData GetNextLane();
}