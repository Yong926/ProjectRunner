public class LaneWave : Lane
{
    public string Name => "WavePattern";

    public void Initialize(int maxlane)
    {
    }

    public int GetNextLane()
    {
        return -1;
    }

    public float Amplitude;
    public float Frequency;
    public float offsetZ;

    public int count;

    // void OnDrawGizmos()
    // {

    //     for (int i = 0; i < count; i++)
    //     {
    //         float t = (float)i / (count - 1);
    //         Vector3 v = Vector3.Lerp(transform.position, transform.position + transform.forward * offsetZ, t);
    //         float s = Mathf.Abs(Mathf.Sin(t * Mathf.PI * Frequency)) * Amplitude;
    //         v = new Vector3(v.x, v.y + s, v.z);
    //         Gizmos.DrawCube(v, Vector3.one * 0.5f);
    //     }
    // }
}