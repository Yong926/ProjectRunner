using UnityEngine;
using Unity.Mathematics;
using DG.Tweening;
using Deform;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] Transform pivot;

    [SerializeField] SquashAndStretchDeformer deformLeft, deformRight, deformUp, deformDown, deformSlide;

    [SerializeField] float moveDuration = 0.5f;
    [SerializeField] Ease moveEase;

    [SerializeField] float jumpDuration = 0.5f;
    [SerializeField] float jumpHeight = 3f;
    [SerializeField] Ease jumpEase;

    [SerializeField] float slideDuration = 0.5f;

    [SerializeField] float[] jumpIntervals = { 0.25f, 0.5f, 0.75f, 0.25f };

    [HideInInspector] public TrackManager trackMgr;

    private int currentLane = 1;
    private Vector3 targetpos;
    private bool isMoving, isUpNDown;
    private Sequence _seqMove;

    void Update()
    {
        if (pivot == null)
            return;

        if (Input.GetButtonDown("Left") && currentLane > 0)
            HandleDirection(-1);

        if (Input.GetButtonDown("Right") && currentLane < trackMgr.laneList.Count - 1)
            HandleDirection(+1);

        if (Input.GetButton("Jump"))
            HandleJump();

        if (Input.GetButton("Slide"))
            HandleSlide();
    }

    void HandleDirection(int direction)
    {
        if (isUpNDown == true) return;

        isMoving = true;

        var squash = direction switch { -1 => deformLeft, 1 => deformRight, _ => null };

        if (_seqMove != null || isMoving == false)
            _seqMove.Kill(true);


        currentLane += direction;
        currentLane = math.clamp(currentLane, 0, trackMgr.laneList.Count - 1);

        Transform l = trackMgr.laneList[currentLane];

        targetpos = new Vector3(l.position.x, pivot.position.y, pivot.position.z);

        _seqMove = DOTween.Sequence().OnComplete(() => { squash.Factor = 0; isMoving = false; });
        _seqMove.Append(pivot.DOMove(targetpos, moveDuration));
        _seqMove.Join(DOVirtual.Float(0f, 1f, moveDuration / 2f, (v) => squash.Factor = v));
        _seqMove.Append(DOVirtual.Float(1f, 0f, moveDuration / 2f, (v) => squash.Factor = v));
    }
    void HandleJump()
    {
        if (isMoving == true || isUpNDown == true) return;

        isUpNDown = true;

        pivot.DOLocalJump(targetpos, jumpHeight, 1, jumpDuration)
            .OnComplete(() => isMoving = false)
            .SetEase(jumpEase);

        var seqJump = DOTween.Sequence().OnComplete(() => { deformUp.Factor = 0; deformDown.Factor = 0; isUpNDown = false; });


        seqJump.Append(DOVirtual.Float(0f, 1f, jumpDuration * jumpIntervals[0], (v) => deformUp.Factor = v));
        seqJump.Append(DOVirtual.Float(1f, 0f, jumpDuration * jumpIntervals[1], (v) => deformUp.Factor = v));
        seqJump.Join(DOVirtual.Float(0f, 1f, jumpDuration * jumpIntervals[2], (v) => deformDown.Factor = v));
        seqJump.Append(DOVirtual.Float(1f, 0f, jumpDuration * jumpIntervals[3], (v) => deformDown.Factor = v));
    }

    void HandleSlide()
    {
        if (isMoving == true || isUpNDown == true) return;

        isUpNDown = true;

        var seqSlide = DOTween.Sequence().OnComplete(() => { deformSlide.Factor = 0; isUpNDown = false; }); ;
        seqSlide.Append(DOVirtual.Float(0f, -1f, slideDuration * 0.25f, (v) => deformSlide.Factor = v));
        seqSlide.AppendInterval(slideDuration * 0.5f);
        seqSlide.Append(DOVirtual.Float(-1f, 0f, slideDuration * 0.5f, (v) => deformSlide.Factor = v));
    }
}