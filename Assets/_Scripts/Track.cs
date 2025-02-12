using UnityEngine;
using System.Collections.Generic;

public class Track : MonoBehaviour
{
    public Transform EntryPoint;
    public Transform ExitPoint;
    public List<Transform> laneList;
    [HideInInspector] public TrackManager trackMgr;

    void LateUpdate()
    {
        Scroll();
    }

    void Scroll()
    {
        if (trackMgr == null) return;

        transform.position += Vector3.back * trackMgr.scrollSpeed * Time.smoothDeltaTime;
    }
}