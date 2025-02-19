using UnityEngine;

public class ObstacleDouble : Obstacle
{
    public override void SetLanePosion(int lane, float zpos, TrackManager tm)
    {
        lane = Mathf.Clamp(lane, 0, tm.laneList.Count - 1);
        Vector3 lanepos0 = tm.laneList[0].position;
        Vector3 lanepos1 = tm.laneList[1].position;
        Vector3 lanepos2 = tm.laneList[2].position;

        float posX = 0f;

        int rndLane = Random.Range(0, tm.laneList.Count - 1);
        if (rndLane == 0)
            posX = (lanepos0.x + lanepos1.x) * 0.5f;
        else if (rndLane == 1)
            posX = (lanepos1.x + lanepos2.x) * 0.5f;

        Vector3 pos = new Vector3(posX, lanepos1.y, zpos);
        transform.SetPositionAndRotation(pos, Quaternion.identity);
    }
}