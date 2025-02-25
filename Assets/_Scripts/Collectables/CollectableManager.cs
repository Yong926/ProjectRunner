using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CustomInspector;

[System.Serializable]
public class CollectablePool : RandomItem
{
    public Collectable collectable;

    public override Object GetItem()
    {
        return collectable;
    }
}

public class CollectableManager : MonoBehaviour
{
    [Space(20)]
    public List<CollectablePool> collectaPools;

    [Space(20)]
    [SerializeField] float spawnZpos = 60f;

    [Space(20)]
    [SerializeField, AsRange(0, 100)] Vector2 spawnInterval;
    [SerializeField] int spawnQuota;

    private TrackManager trackMgr;
    private RandomGenerator randomGenerator = new RandomGenerator();
    private LaneGenerator laneGenerator;

    IEnumerator Start()
    {
        trackMgr = FindFirstObjectByType<TrackManager>();
        if (trackMgr == null)
        {
            Debug.LogError($"트랙 관리자 없음");
            yield break;
        }

        yield return new WaitForEndOfFrame();

        laneGenerator = new LaneGenerator(trackMgr.laneList.Count, spawnQuota);

        foreach (var pool in collectaPools)
            randomGenerator.AddItem(pool);

        yield return new WaitUntil(() => GameManager.IsPlaying == true);

        StartCoroutine(InfiniteSpawn());
    }

    public void SpawnCollectable()
    {
        (int lane, Collectable prefab) = RandomLanePrefab();

        Track t = trackMgr.GetTrackByZ(spawnZpos);
        if (t == null)
        {
            Debug.LogWarning("Z 위치에 해당하는 트랙이 없음");
            return;
        }

        if (prefab != null)
        {
            var o = Instantiate(prefab, t.CollectableRoot);
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
                SpawnCollectable();

                lastMileage = GameManager.mileage;
            }
        }
    }

    (int, Collectable) RandomLanePrefab()
    {
        int lane = laneGenerator.GetNextLane();

        Collectable prefab = randomGenerator.GetRandom().GetItem() as Collectable;

        if (prefab == null) return (-1, null);

        return (lane, prefab);
    }
}