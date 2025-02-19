using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ObstacleType { Single, Double, Triple, _MAX_ }

public class ObstacleManager : MonoBehaviour
{
    [Space(20)]
    [SerializeField] List<Obstacle> obstacleSingle;
    [SerializeField] List<Obstacle> obstacleDouble;
    [SerializeField] List<Obstacle> obstacleTriple;

    [Space(20)]
    [SerializeField] float spawnZpos = 60f;

    [Space(20)]
    [SerializeField] float spawnInterval = 1f;

    TrackManager trackMgr;

    IEnumerator Start()
    {
        TrackManager[] tm = FindObjectsByType<TrackManager>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        if (tm == null || tm.Length <= 0)
        {
            Debug.LogError($"트랙 관리자 없음");
            yield break;
        }

        trackMgr = tm[0];

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

            if (GameManager.mileage - lastMileage > spawnInterval)
            {
                SpawnObstacle();

                lastMileage = GameManager.mileage;
            }
        }
    }

    (int, Obstacle) RandomLanePrefab()
    {
        int rndLane = Random.Range(0, trackMgr.laneList.Count);
        int rndType = Random.Range((int)ObstacleType.Single, (int)ObstacleType._MAX_);

        List<Obstacle> obstacles = rndType switch
        {
            (int)ObstacleType.Single => obstacleSingle,
            (int)ObstacleType.Double => obstacleDouble,
            (int)ObstacleType.Triple => obstacleTriple,
            _ => null
        };

        if (obstacles.Count <= 0) return (-1, null);

        Obstacle prefab = obstacles[Random.Range(0, obstacles.Count)];

        if (prefab == null) return (-1, null);

        return (rndLane, prefab);
    }
}