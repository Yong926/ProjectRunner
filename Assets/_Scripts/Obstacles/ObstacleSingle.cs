using UnityEngine;

public class ObstacleSingle : Obstacle
{
    public enum SingleType { NONE, BLOCK }
    public SingleType singleType = SingleType.NONE;

    public override void SetLanePosion(int lane, float zpos, TrackManager tm)
    {
        lane = Mathf.Clamp(lane, 0, tm.laneList.Count - 1);
        Transform laneTransform = tm.laneList[lane];
        Vector3 pos = new Vector3(laneTransform.position.x, laneTransform.position.y, zpos);

        transform.SetPositionAndRotation(pos, Quaternion.identity);
    }
}