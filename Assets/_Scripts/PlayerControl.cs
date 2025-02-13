using UnityEngine;
using Unity.Mathematics;
using DG.Tweening;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] float moveDuration = 0.5f;
    [SerializeField] Ease moveEase;
    [SerializeField] float jumpDuration = 0.5f;
    [SerializeField] float jumpHeight = 3f;
    [SerializeField] Ease jumpEase;

    [HideInInspector] public TrackManager trackMgr;

    private int currentLane = 1;
    private Vector3 targetpos;
    private bool isMoving = false;

    void Update()
    {
        if (Input.GetButtonDown("Left") && isMoving == false)
            HandleDirection(-1);

        else if (Input.GetButtonDown("Right") && isMoving == false)
            HandleDirection(+1);

        else if (Input.GetButtonDown("Jump") && isMoving == false)
            HandleJump();

    }

    void HandleDirection(int direction)
    {
        isMoving = true;

        currentLane += direction;
        currentLane = math.clamp(currentLane, 0, trackMgr.laneList.Count - 1);

        Transform l = trackMgr.laneList[currentLane];

        targetpos = new Vector3(l.position.x, transform.position.y, transform.position.z);

        transform.DOMove(targetpos, moveDuration)
                .OnComplete(() => isMoving = false)
                .SetEase(moveEase);
    }

    void HandleJump()
    {
        isMoving = true;

        transform.DOLocalJump(targetpos, jumpHeight, 1, jumpDuration)
                .OnComplete(() => isMoving = false)
                .SetEase(jumpEase);
    }
}