using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CustomInspector;

[System.Serializable]
public class ObstaclePool : RandomItem
{
    public List<Obstacle> obstacleList;

    public override Object GetItem()
    {
        if (obstacleList == null || obstacleList.Count <= 0)
            return null;

        return obstacleList[Random.Range(0, obstacleList.Count)];
    }
}

public class ObstacleManager : MonoBehaviour
{
    [Space(20)]
    public List<ObstaclePool> obstaclePools;

    [Space(20)]
    [SerializeField] float spawnZpos = 60f;

    [Space(20)]
    [SerializeField, AsRange(0, 100)] Vector2 spawnInterval;

    TrackManager trackMgr;
    RandomGenerator randomGenerator = new RandomGenerator();

    IEnumerator Start()
    {
        trackMgr = FindFirstObjectByType<TrackManager>();
        if (trackMgr == null)
        {
            Debug.LogError($"트랙 관리자 없음");
            yield break;
        }

        foreach (var pool in obstaclePools)
            randomGenerator.AddItem(pool);

        yield return new WaitUntil(() => GameManager.IsPlaying == true);

        StartCoroutine(InfiniteSpawn());
    }

    public void SpawnObstacle()
    {
        (int lane, Obstacle prefab) = RandomLanePrefab();

        Track t = trackMgr.GetTrackByZ(spawnZpos);
        if (t == null)
        {
            Debug.LogWarning("Z 위치에 해당하는 트랙이 없음");
            return;
        }

        if (prefab != null)
        {
            var o = Instantiate(prefab, t.ObstacleRoot);
            o.SetLanePosion(lane, spawnZpos, trackMgr);
        }
    }

    IEnumerator InfiniteSpawn()
    {
        double lastMileage = 0f;

        while (true)
        {
            yield return new WaitUntil(() => GameManager.IsPlaying);

            if (GameManager.mileage - lastMileage > Random.Range(spawnInterval.x, spawnInterval.y))
            {
                SpawnObstacle();

                lastMileage = GameManager.mileage;
            }
        }
    }

    (int, Obstacle) RandomLanePrefab()
    {
        int rndLane = Random.Range(0, trackMgr.laneList.Count);

        Obstacle prefab = randomGenerator.GetRandom().GetItem() as Obstacle;

        if (prefab == null) return (-1, null);

        return (rndLane, prefab);
    }
}