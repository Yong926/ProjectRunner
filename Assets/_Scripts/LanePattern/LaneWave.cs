using UnityEngine;

public class LaneWave : Lane
{
    public LaneType laneType => LaneType.WAVE;

    private LaneData data;
    private float Amplitude = 1.5f;
    private float Frequency = 2f;
    private float elapsed = 0f;

    public void Initialize(int maxlane)
    {
        elapsed = 0f;
        data.maxLane = maxlane;

        System.Random random = new System.Random();
        data.currentLane = random.Next(0, maxlane);
    }

    public LaneData GetNextLane()
    {
        data.currentY = Mathf.Abs(Mathf.Sin(elapsed * Mathf.PI * Frequency)) * Amplitude;
        elapsed += 0.1f;
        return data;
    }
}