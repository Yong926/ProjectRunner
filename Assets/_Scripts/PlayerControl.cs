using UnityEngine;
using Unity.Mathematics;

public class PlayerControl : MonoBehaviour
{
    public float horzspeed;
    [HideInInspector] public TrackManager trackMgr;
    public int currentLane = 1;

    void Update()
    {
        if (Input.GetButtonDown("Left"))
        {
            currentLane -= 1;
            currentLane = math.clamp(currentLane, 0, trackMgr.laneList.Count - 1);

            Transform l = trackMgr.laneList[currentLane];

            transform.position = new Vector3(l.position.x, transform.position.y, transform.position.z);
        }

        else if (Input.GetButtonDown("Right"))
        {
            currentLane += 1;
            currentLane = math.clamp(currentLane, 0, trackMgr.laneList.Count - 1);

            Transform l = trackMgr.laneList[currentLane];

            transform.position = new Vector3(l.position.x, transform.position.y, transform.position.z);
        }

        else
        {

        }
    }
}