using MoreMountains.Feedbacks;
using UnityEngine;

public class CollectableCoin : Collectable
{
    [SerializeField] Transform pivot;
    [SerializeField] MMF_Player feedbackDisappear;
    public uint Add = 1;
    public override void SetLanePosion(int lane, float zpos, TrackManager tm)
    {
        lane = Mathf.Clamp(lane, 0, tm.laneList.Count - 1);
        Transform laneTransform = tm.laneList[lane];
        Vector3 pos = new Vector3(laneTransform.position.x, laneTransform.position.y, zpos);

        transform.SetPositionAndRotation(pos, Quaternion.identity);
    }
    public override void Collect()
    {
        GameManager.coin += Add;

        transform.SetParent(null);
        feedbackDisappear?.PlayFeedbacks();
    }
}