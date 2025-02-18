using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ObstacleType { Single, Top, Bottom, _MAX_ }

public class ObstacleManager : MonoBehaviour
{
    [Space(20)]
    [SerializeField] List<Obstacle> obstacleSingle;
    [SerializeField] List<Obstacle> obstacleTop;
    [SerializeField] List<Obstacle> obstacleBottom;

    [Space(20)]
    [SerializeField] Transform spawnPoint;

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

    public void SpawnObstacle(int lane)
    {
        lane = Mathf.Clamp(lane, 0, trackMgr.laneList.Count - 1);
        Transform laneTransform = trackMgr.laneList[lane];
        Vector3 pos = new Vector3(laneTransform.position.x, laneTransform.position.y, spawnPoint.position.z);

        Track t = trackMgr.GetTrackByZ(spawnPoint.position.z);
        if (t == null)
        {
            Debug.LogWarning("Z 위치에 해당하는 트랙이 없음");
            return;
        }

        var obsprefab = RandomTypeSpanw();

        Instantiate(obsprefab, pos, Quaternion.identity, t.ObstacleRoot);
    }

    IEnumerator InfiniteSpawn()
    {
        while (true)
        {
            yield return new WaitUntil(() => GameManager.IsPlaying);

            SpawnObstacle(Random.Range(0, trackMgr.laneList.Count));

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    Obstacle RandomTypeSpanw()
    {
        int rndType = Random.Range((int)ObstacleType.Single, (int)ObstacleType._MAX_);

        List<Obstacle> obstacles = rndType switch
        {
            (int)ObstacleType.Single => obstacleSingle,
            (int)ObstacleType.Top => obstacleTop,
            (int)ObstacleType.Bottom => obstacleBottom,
            _ => null
        };

        Obstacle prefab = obstacles[Random.Range(0, obstacles.Count)];

        return prefab;
    }
}