using UnityEngine;
using Unity.Mathematics;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] AnimationCurve jumpCurve;
    [SerializeField] float jumpDuration = 0.5f;
    [SerializeField] float jumpHeight = 3f;

    [HideInInspector] public TrackManager trackMgr;

    private int currentLane = 1;
    private Vector3 targetpos;
    private float jumpStarttime;
    private bool isJumping = false;

    void Update()
    {
        if (Input.GetButtonDown("Left") && isJumping == false)
            HandlePlayer(-1);

        else if (Input.GetButtonDown("Right") && isJumping == false)
            HandlePlayer(+1);

        else if (Input.GetButtonDown("Jump") && isJumping == false)
        {
            jumpStarttime = Time.time;
            isJumping = true;
        }

        if (isJumping == true)
        {
            float elapsed = Time.time - jumpStarttime;
            float p = Mathf.Clamp(elapsed / jumpDuration, 0f, 1f);
            float height = jumpCurve.Evaluate(p) * jumpHeight;

            targetpos = new Vector3(targetpos.x, height, targetpos.z);

            if (p >= 1f)
                isJumping = false;
        }

        UpdatePosition();
    }

    void HandlePlayer(int direction)
    {
        currentLane += direction;
        currentLane = math.clamp(currentLane, 0, trackMgr.laneList.Count - 1);

        Transform l = trackMgr.laneList[currentLane];

        targetpos = new Vector3(l.position.x, transform.position.y, transform.position.z);
    }

    void UpdatePosition()
    {
        transform.position = Vector3.Lerp(transform.position, targetpos, speed * Time.deltaTime);
    }

    // void UpdateJump()
    // {
    // }
}