using UnityEngine;
using TMPro;

public class InGameUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tmDistance;

    void Update()
    {
        if (GameManager.IsPlaying == false) return;

        tmDistance.text = $"{((int)GameManager.mileage).ToStringKilo()}" + "<size=80%>m</size>";
    }
}