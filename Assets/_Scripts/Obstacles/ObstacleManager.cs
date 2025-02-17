using UnityEngine;
using System.Collections;

public class ObstacleManager : MonoBehaviour
{
    [SerializeField] Obstacle obstaclePrefan;
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
        Obstacle o = Instantiate(obstaclePrefan, pos, Quaternion.identity, t.ObstacleRoot);
    }

    IEnumerator InfiniteSpawn()
    {
        while (true)
        {
            if (GameManager.IsPlaying == false)
                yield break;

            SpawnObstacle(Random.Range(0, trackMgr.laneList.Count));
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}