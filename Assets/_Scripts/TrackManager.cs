using UnityEngine;
using System.Collections.Generic;

public class TrackManager : MonoBehaviour
{
    public Track trackPrefab;
    public PlayerControl playerprefab;
    [Range(0f, 50f)] public float scrollSpeed = 10f;
    [Range(1, 10)] public int trackCount = 3;
    private List<Track> trackList = new List<Track>();
    private Transform camTransform;

    [HideInInspector] public List<Transform> laneList;

    void Start()
    {
        camTransform = Camera.main.transform;
        SpawnInitialTrack();
        SpawnPlayer();
    }

    void Update()
    {
        RepositionTrack();
    }

    void SpawnInitialTrack()
    {
        Vector3 position = new Vector3(0f, 0f, camTransform.position.z);

        for (int i = 0; i < trackCount; i++)
        {
            Track Next = SpawNextTrack(position, $"Track_{i}");
            position = Next.ExitPoint.position;
        }
    }

    Track SpawNextTrack(Vector3 position, string trackname)
    {
        Track Next = Instantiate(trackPrefab, position, Quaternion.identity, transform);
        Next.name = trackname;
        Next.trackMgr = this;
        laneList = Next.laneList;
        trackList.Add(Next);

        return Next;
    }

    void RepositionTrack()
    {
        if (trackList.Count <= 0) return;

        if (trackList[0].ExitPoint.position.z < camTransform.position.z)
        {
            Track last = trackList[trackList.Count - 1];
            SpawNextTrack(last.ExitPoint.position, trackList[0].name);

            Destroy(trackList[0].gameObject);
            trackList.RemoveAt(0);
        }
    }

    void SpawnPlayer()
    {
        PlayerControl player = Instantiate(playerprefab, Vector3.zero, Quaternion.identity);
        player.trackMgr = this;
    }
}