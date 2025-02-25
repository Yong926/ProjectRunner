using UnityEngine;
using TMPro;
using DG.Tweening;
using CustomInspector;
using MoreMountains.Feedbacks;

public class InGameUI : MonoBehaviour
{
    [HorizontalLine]
    [SerializeField] TextMeshProUGUI tmInformation;

    [SerializeField] MMF_Player feedbackInformation;

    [HorizontalLine]
    [SerializeField] TextMeshProUGUI tmMileage;

    [SerializeField] TextMeshProUGUI tmCoin;

    [SerializeField] TextMeshProUGUI tmLife;

    void Awake()
    {
    }

    void Update()
    {
        UpdateMileage();

        UpdateCoins();

        UpdateLife();
    }

    public void ShowInfo(string info, float duration = 1f)
    {
        if (feedbackInformation.IsPlaying)
            feedbackInformation.StopFeedbacks();

        tmInformation.text = info;
        feedbackInformation.GetFeedbackOfType<MMF_Pause>().PauseDuration = duration;
        feedbackInformation?.PlayFeedbacks();
    }

    void UpdateMileage()
    {
        if (GameManager.mileage <= 1000f)
        {
            long intpart = (long)GameManager.mileage;
            int decpart = (int)((GameManager.mileage - intpart) * 10);

            tmMileage.text = $"{intpart}<size=80%>.{decpart}</size><size=60%>m</size>";
        }
        else
        {
            ((long)GameManager.mileage).ToStringKilo(out string intpart, out string decpart, out string unitpart);

            tmMileage.text = $"{intpart}<size=80%>{decpart}{unitpart}</size><size=60%>m</size>";
        }
    }

    private uint _lastcoin;
    private Tween _tweencoin;

    void UpdateCoins()
    {

        if (_lastcoin == GameManager.coin)
            return;

        if (_tweencoin != null)
            _tweencoin.Kill(true);

        tmCoin.text = GameManager.coin.ToString("N0");
        _lastcoin = GameManager.coin;

        tmCoin.rectTransform.localScale = Vector3.one;
        _tweencoin = tmCoin.rectTransform.DOPunchScale(Vector3.one * 0.5f, 0.25f, 10, 1f)
                        .OnComplete(() => tmCoin.rectTransform.localScale = Vector3.one);

    }

    private int _lastlife;
    void UpdateLife()
    {
        if (_lastlife == GameManager.life)
            return;

        tmLife.text = GameManager.life.ToString();

        if (GameManager.life <= 0)
        {
            ShowInfo("GAME OVER", 5f);
            GameManager.IsGameover = true;
        }

        _lastlife = GameManager.life;
    }
}