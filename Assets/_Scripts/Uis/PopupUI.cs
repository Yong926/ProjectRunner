using UnityEngine;

public class PopupUI : MonoBehaviour
{
    [SerializeField] GameObject dimmerUI;
    [SerializeField] PopupUIModal modalUI;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        dimmerUI.SetActive(false);
        modalUI.gameObject.SetActive(false);
    }
}