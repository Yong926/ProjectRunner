using UnityEngine;
using Unity.Mathematics;
using DG.Tweening;
using Deform;
using MoreMountains.Feedbacks;

public enum PlayerState { Idle = 0, Move, Jump, Slide }

public class PlayerControl : MonoBehaviour
{
    [SerializeField] SquashAndStretchDeformer deformLeft, deformRight, deformUp, deformDown, deformSlide;

    [Space(20)]
    [SerializeField] Transform pivot;

    [Space(20)]
    [SerializeField] Collider colNormal;
    [SerializeField] Collider colSlide;

    [Space(20)]
    [SerializeField] float moveDuration = 0.5f;
    [SerializeField] Ease moveEase;

    [Space(20)]
    [SerializeField] float[] jumpIntervals = { 0.25f, 0.5f, 0.75f, 0.25f };
    [SerializeField] float jumpDuration = 0.5f;
    [SerializeField] float jumpHeight = 3f;
    [SerializeField] Ease jumpEase;

    [Space(20)]
    [SerializeField] float slideDuration = 0.5f;

    [Space(20)]
    [SerializeField] MMF_Player feedbackImpact;
    [SerializeField] MMF_Player feedbackCrash;

    [HideInInspector] public TrackManager trackMgr;
    [HideInInspector] public PlayerState state;

    int currentLane = 1;
    Vector3 targetpos;
    Sequence _seqMove;

    void Start()
    {
        SwitchCollider(true);
    }

    void Update()
    {
        if (GameManager.IsGameover == false && Input.GetKeyDown(KeyCode.Alpha1))
            GameManager.IsPlaying = !GameManager.IsPlaying;

        if (pivot == null || GameManager.IsPlaying == false) return;

        if (Input.GetButtonDown("Left") && currentLane > 0)
            HandleDirection(-1);

        if (Input.GetButtonDown("Right") && currentLane < trackMgr.laneList.Count - 1)
            HandleDirection(+1);

        if (Input.GetButton("Jump"))
            HandleJump();

        if (Input.GetButton("Slide"))
            HandleSlide();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Collectable")
        {
            feedbackImpact?.PlayFeedbacks();
            other.GetComponentInParent<Collectable>()?.Collect();
        }

        else if (other.tag == "Obstacle")
        {
            feedbackCrash?.PlayFeedbacks();
            GameManager.life -= 1;
            GameManager.IsPlaying = false;
        }
    }

    void HandleDirection(int direction)
    {
        if (state == PlayerState.Jump || state == PlayerState.Slide) return;

        state = PlayerState.Move;

        var squash = direction switch { -1 => deformLeft, 1 => deformRight, _ => null };

        if (_seqMove != null)
        {
            _seqMove.Kill(true);
            state = PlayerState.Move;
        }

        currentLane += direction;
        currentLane = math.clamp(currentLane, 0, trackMgr.laneList.Count - 1);

        Transform l = trackMgr.laneList[currentLane];

        targetpos = new Vector3(l.position.x, pivot.position.y, pivot.position.z);

        _seqMove = DOTween.Sequence().OnComplete(() => { squash.Factor = 0; state = PlayerState.Idle; });
        _seqMove.Append(pivot.DOMove(targetpos, moveDuration));
        _seqMove.Join(DOVirtual.Float(0f, 1f, moveDuration / 2f, (v) => squash.Factor = v));
        _seqMove.Append(DOVirtual.Float(1f, 0f, moveDuration / 2f, (v) => squash.Factor = v));
    }

    void HandleJump()
    {
        if (state != PlayerState.Idle) return;

        state = PlayerState.Jump;

        pivot.DOLocalJump(targetpos, jumpHeight, 1, jumpDuration)
            .SetEase(jumpEase);

        deformUp.Factor = 0;
        deformDown.Factor = 0;

        var seqJump = DOTween.Sequence().OnComplete(() => { state = PlayerState.Idle; });

        seqJump.Append(DOVirtual.Float(0f, 1f, jumpDuration * jumpIntervals[0], (v) => deformUp.Factor = v));
        seqJump.Append(DOVirtual.Float(1f, 0f, jumpDuration * jumpIntervals[1], (v) => deformUp.Factor = v));
        seqJump.Join(DOVirtual.Float(0f, 1f, jumpDuration * jumpIntervals[2], (v) => deformDown.Factor = v));
        seqJump.Append(DOVirtual.Float(1f, 0f, jumpDuration * jumpIntervals[3], (v) => deformDown.Factor = v));
    }

    void HandleSlide()
    {
        if (state != PlayerState.Idle) return;

        state = PlayerState.Slide;
        SwitchCollider(false);

        var seqSlide = DOTween.Sequence().OnComplete(() =>
        {
            state = PlayerState.Idle;

            SwitchCollider(true);
        });
        seqSlide.Append(DOVirtual.Float(0f, -1f, slideDuration * 0.25f, (v) => deformSlide.Factor = v));
        seqSlide.AppendInterval(slideDuration * 0.5f);
        seqSlide.Append(DOVirtual.Float(-1f, 0f, slideDuration * 0.5f, (v) => deformSlide.Factor = v));
    }

    void SwitchCollider(bool b)
    {
        colNormal.gameObject.SetActive(b);
        colSlide.gameObject.SetActive(!b);
    }
}