using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CustomInspector;
using DG.Tweening;

[System.Serializable]
public class ObstaclePool : RandomItem
{
    public List<Obstacle> obstacleList;

    public override object GetItem()
    {
        if (obstacleList == null || obstacleList.Count <= 0)
            return null;

        return obstacleList[Random.Range(0, obstacleList.Count)];
    }
}

public class ObstacleManager : MonoBehaviour
{
    [Space(20)]
    [SerializeField] float spawnZpos = 60f;
    [SerializeField, ReadOnly] Vector2 spawnInterval;
    [SerializeField, ReadOnly] ObstacleSO data;
    private TrackManager trackMgr;
    private RandomGenerator randomGenerator = new RandomGenerator();

    IEnumerator Start()
    {
        data = null;
        trackMgr = FindFirstObjectByType<TrackManager>();
        if (trackMgr == null)
        {
            Debug.LogError($"트랙 관리자 없음");
            yield break;
        }

        yield return new WaitUntil(() => data != null);
        yield return new WaitUntil(() => GameManager.IsPlaying == true);

        StartCoroutine(InfiniteSpawn());
    }

    public void SpawnObstacle()
    {
        if (data == null)
            return;
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

    public void SetPhase(PhaseSO phase, float duration = 1f)
    {
        if (phase.obstacleData == null)
        {
            ClearObstacles();
            return;
        }

        data = phase.obstacleData;

        randomGenerator.Clear();

        foreach (ObstaclePool pool in data.pools)
            randomGenerator.AddItem(pool);

        DOVirtual.Vector2(spawnInterval, data.interval, duration, i => spawnInterval = i).SetEase(Ease.InOutSine);
    }

    public void ClearObstacles()
    {
        randomGenerator.Clear();
        data = null;
    }
}