using UnityEngine;
using TMPro;

public class InGameUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tmDistance;

    void Update()
    {
        if (GameManager.IsPlaying == false) return;


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
}