using UnityEngine;

public class LaneWave : Lane
{
    public string Name => "WavePattern";

    private LaneData data;
    private float Amplitude = 1.5f;
    private float Frequency = 2.5f;
    private float elapsed = 0f;

    public void Initialize(int maxlane)
    {
        data.maxLane = maxlane;

        System.Random random = new System.Random();
        data.currentLane = random.Next(0, maxlane);
    }

    public LaneData GetNextLane()
    {
        elapsed += 0.1f;
        data.currentY = Mathf.Abs(Mathf.Sin(elapsed * Mathf.PI * Frequency)) * Amplitude;
        return data;
    }
}