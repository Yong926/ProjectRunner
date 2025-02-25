public interface Lane
{
    public string Name { get; }
    public void Initialize(int maxlane);
    public int GetNextLane();
}