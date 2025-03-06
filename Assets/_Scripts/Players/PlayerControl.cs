using UnityEngine;
using Unity.Mathematics;
using DG.Tweening;
using Deform;
using MoreMountains.Feedbacks;

public enum PlayerMove { Idle, Move, Jump, Slide }
public enum PlayerState
{
    INVINCIBLE = 1 << 0,
    MAGNETIC = 1 << 1,
    MULTIPLE = 1 << 2
}

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
    [SerializeField] MMF_Player feedbackInvincible;

    [HideInInspector] public TrackManager trackMgr;
    [HideInInspector] public PlayerMove state;

    private int currentLane = 1;
    private Vector3 targetpos;
    private Sequence _seqMove;

    void Start()
    {
        SwitchCollider(true);
    }

    void Update()
    {
        if (pivot == null || GameManager.IsPlaying == false || GameManager.IsGameover == true) return;

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

        else if (other.tag == "Obstacle" && !GameManager.playerState.HasAny(PlayerState.INVINCIBLE))
        {
            feedbackCrash?.PlayFeedbacks();
            feedbackInvincible?.PlayFeedbacks();
        }

        other.enabled = false;
    }

    void HandleDirection(int direction)
    {
        if (state == PlayerMove.Jump || state == PlayerMove.Slide) return;

        state = PlayerMove.Move;

        var squash = direction switch { -1 => deformLeft, 1 => deformRight, _ => null };

        if (_seqMove != null)
        {
            _seqMove.Kill(true);
            state = PlayerMove.Move;
        }

        currentLane += direction;
        currentLane = math.clamp(currentLane, 0, trackMgr.laneList.Count - 1);

        Transform l = trackMgr.laneList[currentLane];

        targetpos = new Vector3(l.position.x, pivot.position.y, pivot.position.z);

        _seqMove = DOTween.Sequence().OnComplete(() => { squash.Factor = 0; state = PlayerMove.Idle; });
        _seqMove.Append(pivot.DOMove(targetpos, moveDuration));
        _seqMove.Join(DOVirtual.Float(0f, 1f, moveDuration / 2f, (v) => squash.Factor = v));
        _seqMove.Append(DOVirtual.Float(1f, 0f, moveDuration / 2f, (v) => squash.Factor = v));
    }

    void HandleJump()
    {
        if (state != PlayerMove.Idle) return;

        state = PlayerMove.Jump;

        pivot.DOLocalJump(targetpos, jumpHeight, 1, jumpDuration)
            .SetEase(jumpEase);

        deformUp.Factor = 0;
        deformDown.Factor = 0;

        var seqJump = DOTween.Sequence().OnComplete(() => { state = PlayerMove.Idle; });

        seqJump.Append(DOVirtual.Float(0f, 1f, jumpDuration * jumpIntervals[0], (v) => deformUp.Factor = v));
        seqJump.Append(DOVirtual.Float(1f, 0f, jumpDuration * jumpIntervals[1], (v) => deformUp.Factor = v));
        seqJump.Join(DOVirtual.Float(0f, 1f, jumpDuration * jumpIntervals[2], (v) => deformDown.Factor = v));
        seqJump.Append(DOVirtual.Float(1f, 0f, jumpDuration * jumpIntervals[3], (v) => deformDown.Factor = v));
    }

    void HandleSlide()
    {
        if (state != PlayerMove.Idle) return;

        state = PlayerMove.Slide;
        SwitchCollider(false);

        var seqSlide = DOTween.Sequence().OnComplete(() =>
        {
            state = PlayerMove.Idle;

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

    public void OnCrash(bool b)
    {
        if (b)
            GameManager.life -= 1;

        GameManager.IsPlaying = !b;

    }

    public void ONInvincible(bool b)
    {
        if (b)
            GameManager.playerState |= PlayerState.INVINCIBLE;
        else
            GameManager.playerState &= ~PlayerState.INVINCIBLE;
    }
}