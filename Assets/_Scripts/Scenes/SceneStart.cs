using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneStart : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI tmVersion;

    void OnValidate()
    {
        if (tmVersion != null)
            tmVersion.text = $"v {Application.version}";
    }

    public void TapToStart()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
}