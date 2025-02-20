using UnityEngine;

public abstract class Obstacle : MonoBehaviour
{
    public abstract void SetLanePosion(int lane, float zpos, TrackManager tm);
}