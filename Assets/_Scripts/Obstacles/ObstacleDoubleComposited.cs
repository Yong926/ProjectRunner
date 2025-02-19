using UnityEngine;
using System.Collections.Generic;

public class ObstacleDoubleComposited : ObstacleDouble
{
    public List<Obstacle> compositedprefab;

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
        };
    }
}