using UnityEngine;
using System.Collections.Generic;

public class ObstacleDouble : Obstacle
{
    protected List<Vector3> spawnedPos = new List<Vector3>();

    public override void SetLanePosion(int lane, float zpos, TrackManager tm)
    {
        spawnedPos.Clear();

        lane = Mathf.Clamp(lane, 0, tm.laneList.Count - 1);
        Vector3 lanepos0 = tm.laneList[0].position;
        Vector3 lanepos1 = tm.laneList[1].position;
        Vector3 lanepos2 = tm.laneList[2].position;

        float posX = 0f;

        int rndLane = Random.Range(0, tm.laneList.Count - 1);
        if (rndLane == 0)
        {
            posX = (lanepos0.x + lanepos1.x) * 0.5f;
            spawnedPos.Add(lanepos0);
            spawnedPos.Add(lanepos1);

        }
        else if (rndLane == 1)
        {
            posX = (lanepos1.x + lanepos2.x) * 0.5f;
            spawnedPos.Add(lanepos1);
            spawnedPos.Add(lanepos2);
        }
        Vector3 pos = new Vector3(posX, tm.laneList[lane].position.y, zpos);

        transform.SetPositionAndRotation(pos, Quaternion.identity);
    }
}