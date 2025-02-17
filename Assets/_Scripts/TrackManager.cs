using UnityEngine;
using System.Collections.Generic;

public class TrackManager : MonoBehaviour
{
    [Space(20)]
    public Track trackPrefab;
    public PlayerControl playerprefab;

    [Space(20)]
    [Range(0f, 50f)] public float scrollSpeed = 10f;
    [Range(1, 10)] public int trackCount = 3;

    public Material CurvedMaterial;
    [Range(0f, 0.5f)] public float CurvedFrequencyX;
    [Range(0f, 10f)] public float CurvedAmplitudeX;
    [Range(0f, 0.5f)] public float CurvedFrequencyY;
    [Range(0f, 10f)] public float CurvedAmplitudeY;

    private List<Track> trackList = new List<Track>();
    private Transform camTransform;

    [HideInInspector] public List<Transform> laneList;

    private int _curveAmount = Shader.PropertyToID("_CurveAmount");

    void Start()
    {
        camTransform = Camera.main.transform;
        SpawnInitialTrack();
        SpawnPlayer();
    }

    void Update()
    {
        RepositionTrack();

        CurveTrack();
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

    void CurveTrack()
    {
        float rndX = Mathf.PerlinNoise1D(Time.time * CurvedFrequencyX) * 2f - 1f;
        rndX = rndX * CurvedAmplitudeX;

        float rndY = Mathf.PerlinNoise1D(Time.time * CurvedFrequencyY) * 2f - 1f;
        rndY = rndY * CurvedAmplitudeY;

        CurvedMaterial.SetVector(_curveAmount, new Vector4(rndX, rndY, 0f, 0f));
    }

}