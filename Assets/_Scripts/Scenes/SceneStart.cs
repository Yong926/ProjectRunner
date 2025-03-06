using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStart : MonoBehaviour
{
    public void TapToStart()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
}