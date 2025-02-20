using UnityEngine;
using TMPro;
using DG.Tweening;

public class InGameUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tmDistance;
    [SerializeField] TextMeshProUGUI tmInformation;
    Sequence _seqInfo;

    void Awake()
    {
        tmInformation.text = "";
    }

    void Update()
    {
        if (GameManager.IsPlaying == false) return;

        UpdateMilege();
    }

    void UpdateMilege()
    {
        if (GameManager.mileage <= 1000f)
        {
            long intpart = (long)GameManager.mileage;
            int decpart = (int)((GameManager.mileage - intpart) * 10);

            tmDistance.text = $"{intpart}<size=80%>.{decpart}</size><size=60%>m</size>";
        }
        else
        {
            ((long)GameManager.mileage).ToStringKilo(out string intpart, out string decpart, out string unitpart);

            tmDistance.text = $"{intpart}<size=80%>{decpart}{unitpart}</size><size=60%>m</size>";
        }
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
}