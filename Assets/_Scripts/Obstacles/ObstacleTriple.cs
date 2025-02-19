using UnityEngine;

public class obstacleTriple : Obstacle
{
    public override void SetLanePosion(int lane, float zpos, TrackManager tm)
    {
        lane = Mathf.Clamp(lane, 0, tm.laneList.Count - 1);
        Vector3 lanepos1 = tm.laneList[1].position;


        Vector3 pos = new Vector3(lanepos1.x, lanepos1.y, zpos);
        transform.SetPositionAndRotation(pos, Quaternion.identity);
    }
}