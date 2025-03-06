using UnityEngine;
using System.Collections;
using CustomInspector;
using DG.Tweening;

[System.Serializable]
public class CollectablePool : RandomItem
{
    public Collectable collectable;

    public override object GetItem()
    {
        return collectable;
    }
}

[System.Serializable]
public class LanePatternPool : RandomItem
{
    public LaneType patternType;
    public override object GetItem()
    {
        return patternType;
    }
}
public class CollectableManager : MonoBehaviour
{
    [Space(20)]
    [SerializeField] float spawnZpos = 60f;

    [Space(20)]
    [SerializeField, ReadOnly, AsRange(0, 100)] Vector2 spawnInterval;

    private CollectableSO data;
    private RandomGenerator randomGenerator = new RandomGenerator();
    private LaneGenerator laneGenerator;
    private TrackManager trackMgr;

    IEnumerator Start()
    {
        trackMgr = FindFirstObjectByType<TrackManager>();
        if (trackMgr == null)
        {
            Debug.LogError($"트랙 관리자 없음");
            yield break;
        }

        yield return new WaitUntil(() => GameManager.IsPlaying == true);

        StartCoroutine(InfiniteSpawn());
    }

    public void SpawnCollectable()
    {
        if (data == null)
            return;
        (LaneData lanedata, Collectable prefab) = RandomLanePrefab();

        Track t = trackMgr.GetTrackByZ(spawnZpos);
        if (t == null)
        {
            Debug.LogWarning("Z 위치에 해당하는 트랙이 없음");
            return;
        }

        if (prefab != null && lanedata.currentLane != -1)
        {
            var o = Instantiate(prefab, t.CollectableRoot);
            o.SetLanePosion(lanedata.currentLane, lanedata.currentY, spawnZpos, trackMgr);
        }
    }

    IEnumerator InfiniteSpawn()
    {
        double lastMileage = 0f;

        while (true)
        {
            yield return new WaitUntil(() => GameManager.IsPlaying && data != null);

            if (GameManager.mileage - lastMileage > Random.Range(spawnInterval.x, spawnInterval.y))
            {
                SpawnCollectable();

                lastMileage = GameManager.mileage;
            }
        }
    }

    (LaneData, Collectable) RandomLanePrefab()
    {
        LaneData lane = laneGenerator.GetNextLane();

        Collectable prefab = randomGenerator.GetRandom().GetItem() as Collectable;

        if (prefab == null) return (lane, null);

        return (lane, prefab);
    }

    public void SetPhase(PhaseSO phase, float duration = 1f)
    {
        if (phase.CollectableData == null)
        {
            randomGenerator.Clear();
            return;
        }

        data = phase.CollectableData;

        randomGenerator.Clear();

        foreach (var pool in data.collectablePools)
            randomGenerator.AddItem(pool);

        laneGenerator = new LaneGenerator(trackMgr.laneList.Count, data.quota, data.lanepatternPools);

        DOVirtual.Vector2(spawnInterval, data.interval, duration, i => spawnInterval = i).SetEase(Ease.InOutSine);
    }

    public void ClearCollectables()
    {
        data = null;
        randomGenerator.Clear();
    }
}