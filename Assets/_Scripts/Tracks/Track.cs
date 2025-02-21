using UnityEngine;
using System.Collections.Generic;

public class Track : MonoBehaviour
{
    [Space(20)]
    public Transform EntryPoint;
    public Transform ExitPoint;

    [Space(20)]
    public Transform ObstacleRoot;
    public Transform CollectableRoot;

    [Space(20)]
    public List<Transform> laneList;

    [HideInInspector] public TrackManager trackMgr;

    void LateUpdate()
    {
        if (GameManager.IsPlaying == false)
            return;

        Scroll();
    }

    void Scroll()
    {
        if (trackMgr == null) return;

        transform.position += Vector3.back * trackMgr.scrollSpeed * Time.smoothDeltaTime;
    }
}