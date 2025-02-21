using UnityEngine;

public abstract class Collectable : MonoBehaviour
{
    public abstract void SetLanePosion(int lane, float zpos, TrackManager tm);

    public abstract void Collect();
}