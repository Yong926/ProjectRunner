using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using System;

public class TrackManager : MonoBehaviour
{
    [Space(20)]
    [SerializeField] PlayerControl playerprefab;
    [SerializeField] Track trackPrefab;
    [SerializeField] GameObject trackstrat, trackEnd;
    [SerializeField] List<Material> CurvedMaterials;

    [Space(20)]
    [Range(0f, 50f)] public float scrollSpeed = 10f;
    [Range(1, 10)] public int trackCount = 3;
    [Range(1, 5)] public int countdown = 3;

    [Space(20)]
    [Range(0f, 0.5f), SerializeField] float CurvedFrequencyX;
    [Range(0f, 0.5f), SerializeField] float CurvedFrequencyY;

    [Space(20)]
    [Range(0f, 10f), SerializeField] float CurvedAmplitudeX;
    [Range(0f, 10f), SerializeField] float CurvedAmplitudeY;

    [HideInInspector] public List<Transform> laneList;

    List<Track> trackList = new List<Track>();
    Transform camTransform;
    InGameUI uiInGame;
    int _curveAmount = Shader.PropertyToID("_CurveAmount");
    float elapsedTime;

    void Start()
    {
        camTransform = Camera.main.transform;

        uiInGame = FindFirstObjectByType<InGameUI>();

        SpawnInitialTrack();
        SpawnStartZone();
        SpawnPlayer();

        StartCoroutine(Countdown());
    }

    void Update()
    {
        if (GameManager.IsPlaying == false)
            return;


        RepositionTrack();
        SpawnEndZone();
        BendTrack();

        GameManager.mileage += scrollSpeed * Time.smoothDeltaTime;
    }

    public void SetPhase(PhaseSO phase, float duration = 0.5f)
    {
        DOVirtual.Float(scrollSpeed, phase.scrollSpeed, duration, s => scrollSpeed = s).SetEase(Ease.InOutSine);
    }

    void SpawnInitialTrack()
    {
        Vector3 position = new Vector3(0f, 0f, camTransform.position.z);

        for (int i = 0; i < trackCount; i++)
        {
            Track Next = SpawNextTrack(position, $"Track_{i}");
            position = Next.ExitPoint.position;
        }

        BendTrack();
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

    void SpawnStartZone(float zpos = 3f)
    {
        Track t = GetTrackByZ(zpos);
        GameObject o = Instantiate(trackstrat, t.ObstacleRoot);
        Vector3 pos = new Vector3(0f, 0f, zpos);
        o.transform.SetPositionAndRotation(pos, Quaternion.identity);
    }

    GameObject _EndZone;
    void SpawnEndZone(float zpos = 60f)
    {
        if (_EndZone != null)
            return;
        if (GameManager.mileage + zpos < GameManager.mileageEnd)
            return;

        Track t = GetTrackByZ(zpos);
        _EndZone = Instantiate(trackEnd, t.ObstacleRoot);
        Vector3 pos = new Vector3(0f, 0f, zpos);
        _EndZone.transform.SetPositionAndRotation(pos, Quaternion.identity);
    }

    public Track GetTrackByZ(float z)
    {
        foreach (var t in trackList)
        {
            if (z > t.EntryPoint.position.z && z <= t.ExitPoint.position.z)
                return t;
        }

        return null;
    }

    void BendTrack()
    {
        elapsedTime += Time.deltaTime;

        float rndX = Mathf.PerlinNoise1D(elapsedTime * CurvedFrequencyX) * 2f - 1f;
        rndX = rndX * CurvedAmplitudeX;

        float rndY = Mathf.PerlinNoise1D(elapsedTime * CurvedFrequencyY) * 2f - 1f;
        rndY = rndY * CurvedAmplitudeY;

        foreach (var m in CurvedMaterials)
            m.SetVector(_curveAmount, new Vector4(rndX, rndY, 0f, 0f));
    }

    public void StopScollTrack()
    {
        scrollSpeed = 0f;
    }

    IEnumerator Countdown()
    {
        yield return new WaitForEndOfFrame();

        for (int i = countdown; i > 0; i--)
        {
            uiInGame.ShowInfo($"{i}");
            yield return new WaitForSeconds(1f);
        }

        uiInGame.ShowInfo($"Go!!", 1.5f);

        GameManager.IsPlaying = true;
    }
}