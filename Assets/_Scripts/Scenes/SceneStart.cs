using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStart : MonoBehaviour
{
    public string sceneIngame;
    public void TapToStart()
    {
        // Debug.Log("딸깍...");
        SceneManager.LoadScene(sceneIngame, LoadSceneMode.Single);
    }
}