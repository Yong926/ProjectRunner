using System.Collections.Generic;
using UnityEngine;

public class TrackManager : MonoBehaviour
{
    public Track trackPrefab;
    [Range(0f, 50f)] public float scrollSpeed = 10f;
    [Range(1, 10)] public int trackCount = 3;
    private List<Track> trackList = new List<Track>();
    private Transform camTransform;

    void Start()
    {
        camTransform = Camera.main.transform;
        SpawnInitialTrack();
    }

    void Update()
    {
        ScrollTrack();
        RepositionTrack();
    }

    // 초기 트랙 생성 (한번만 실행)
    void SpawnInitialTrack()
    {
        Track track = null;

        for (int i = 0; i < trackCount; i++)
        {
            Vector3 pos = i == 0 ? Vector3.zero : track.ExitPoint.position;
            track = Instantiate(trackPrefab, pos, Quaternion.identity, transform);
            track.name = $"Track_{i}";
            trackList.Add(track);
        }
    }

    void ScrollTrack()
    {
        // if (track != null)
        //     track.transform.position += Vector3.back * scrollSpeed * Time.deltaTime;

        foreach (Track t in trackList)
        {
            if (t != null)
                t.transform.position += Vector3.back * scrollSpeed * Time.deltaTime;
        }
    }

    void RepositionTrack()
    {
        if (trackList.Count <= 0) return;

        if (trackList[0].ExitPoint.position.z < camTransform.position.z)
        {
            SpawNextTrack(trackList[trackList.Count - 1], trackList[0].name);

            Destroy(trackList[0].gameObject);
            trackList.RemoveAt(0);
        }
    }

    void SpawNextTrack(Track current, string trackname)
    {
        Track Next = Instantiate(trackPrefab, current.ExitPoint.position, Quaternion.identity, transform);
        Next.name = trackname;
        trackList.Add(Next);
    }
}