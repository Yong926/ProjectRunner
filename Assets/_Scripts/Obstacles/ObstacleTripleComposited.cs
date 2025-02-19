using UnityEngine;
using System.Collections.Generic;

public class ObstacleTripleComposited : obstacleTriple
{
    public List<Obstacle> compositedprefab;
    List<Vector3> spawnedPos = new List<Vector3>();

    void Start()
    {
        SpawnComposited();
    }

    void SpawnComposited()
    {
        foreach (var p in spawnedPos)
        {
            int rnd = Random.Range(0, compositedprefab.Count);
            Obstacle prefab = compositedprefab[rnd];

            var o = Instantiate(prefab, p, Quaternion.identity, transform);
            Vector3 localpos = o.transform.localPosition;
            o.transform.localPosition = new Vector3(localpos.x, 0f, 0f);
        }
        ;
    }

    public override void SetLanePosion(int lane, float zpos, TrackManager tm)
    {
        spawnedPos.Clear();

        lane = Mathf.Clamp(lane, 0, tm.laneList.Count - 1);
        Vector3 lanepos0 = tm.laneList[0].position;
        Vector3 lanepos1 = tm.laneList[1].position;
        Vector3 lanepos2 = tm.laneList[2].position;

        spawnedPos.Add(lanepos0);
        spawnedPos.Add(lanepos1);
        spawnedPos.Add(lanepos2);

        Vector3 pos = new Vector3(lanepos1.x, lanepos1.y, zpos);

        transform.SetPositionAndRotation(pos, Quaternion.identity);
    }
}