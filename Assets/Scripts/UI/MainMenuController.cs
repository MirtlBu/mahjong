using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private string mapSceneName = "MapScene";

    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button creditsCloseButton;
    [SerializeField] private Button exitButton;

    [Header("Panels")]
    [SerializeField] private GameObject creditsPanel;

    void Start()
    {
        creditsPanel.SetActive(false);

        if (playButton == null)   Debug.LogError("MainMenu: playButton не назначена!");
        if (creditsButton == null) Debug.LogError("MainMenu: creditsButton не назначена!");
        if (creditsCloseButton == null) Debug.LogError("MainMenu: creditsCloseButton не назначена!");
        if (exitButton == null)   Debug.LogError("MainMenu: exitButton не назначена!");
        if (creditsPanel == null) Debug.LogError("MainMenu: creditsPanel не назначена!");

        playButton.onClick.AddListener(OnPlay);
        creditsButton.onClick.AddListener(OnCreditsOpen);
        creditsCloseButton.onClick.AddListener(OnCreditsClose);
        exitButton.onClick.AddListener(OnExit);

        Debug.Log("MainMenu: Start() завершён, слушатели назначены");
    }

    void OnPlay()
    {
        Debug.Log("MainMenu: Play нажата");
        SceneManager.LoadScene(mapSceneName);
    }

    void OnCreditsOpen()
    {
        Debug.Log("MainMenu: Credits нажата");
        creditsPanel.SetActive(true);
    }

    void OnCreditsClose()
    {
        creditsPanel.SetActive(false);
    }

    void OnExit()
    {
        Application.Quit();
    }
}
