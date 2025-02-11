using UnityEngine;

public class Track : MonoBehaviour
{
    public Transform EntryPoint;
    public Transform ExitPoint;
    [HideInInspector] public TrackManager trackManager;

    void LateUpdate()
    {
        Scroll();
    }

    void Scroll()
    {
        if (trackManager == null) return;

        transform.position += Vector3.back * trackManager.scrollSpeed * Time.smoothDeltaTime;
    }
}