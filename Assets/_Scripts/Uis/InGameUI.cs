using UnityEngine;
using TMPro;
using DG.Tweening;
using CustomInspector;

public class InGameUI : MonoBehaviour
{
    [HorizontalLine]
    [SerializeField] TextMeshProUGUI tmInformation;

    [HorizontalLine]
    [SerializeField] TextMeshProUGUI tmMileage;

    [SerializeField] TextMeshProUGUI tmCoin;

    [SerializeField] TextMeshProUGUI tmHealth;

    Sequence _seqInfo;
    Sequence _seqCoin;

    void Awake()
    {
        tmInformation.text = "";
    }

    void Update()
    {
        UpdateMileage();

        UpdateCoins();
    }

    public void ShowInfo(string info, float duration = 1f)
    {
        tmInformation.transform.localScale = Vector3.zero;

        if (_seqInfo != null)
            _seqInfo.Kill(true);

        _seqInfo = DOTween.Sequence().OnComplete(() => tmInformation.transform.localScale = Vector3.zero);
        _seqInfo.AppendCallback(() => tmInformation.text = info);
        _seqInfo.Append(tmInformation.transform.DOScale(1.2f, duration * 0.1f));
        _seqInfo.Append(tmInformation.transform.DOScale(1f, duration * 0.2f));
        _seqInfo.AppendInterval(duration * 0.4f);
        _seqInfo.Append(tmInformation.transform.DOScale(0f, duration * 0.3f));
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
}