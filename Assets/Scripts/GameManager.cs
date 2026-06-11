using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Levels")]
    [SerializeField] private LayoutSO[] levels;

    [Header("Scene Names")]
    [SerializeField] private string mapSceneName  = "MapScene";
    [SerializeField] private string gameSceneName = "GameScene";

    [Header("Starting Resources")]
    [SerializeField] private int startingHints    = 5;
    [SerializeField] private int startingShuffles = 1;

    [Header("Shop Prices")]
    public int hintCost    = 100;
    public int shuffleCost = 200;

    [Header("Victory")]
    [SerializeField] private float victoryToMapDelay = 3f;

    public int CurrentLevelIndex { get; private set; } = -1;
    public LayoutSO[] Levels  => levels;
    public LayoutSO CurrentLevel => (CurrentLevelIndex >= 0 && CurrentLevelIndex < levels.Length)
        ? levels[CurrentLevelIndex] : null;

    public int TotalScore   { get; private set; }
    public int HintCount    { get; private set; }
    public int ShuffleCount { get; private set; }

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadProgress();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameHUD.Instance?.RefreshResourceCounts();
    }

    // ── Progress persistence ──────────────────────────────────────

    void LoadProgress()
    {
        TotalScore   = PlayerPrefs.GetInt("total_score",    0);
        HintCount    = PlayerPrefs.GetInt("hint_count",    startingHints);
        ShuffleCount = PlayerPrefs.GetInt("shuffle_count", startingShuffles);
    }

    void SaveProgress()
    {
        PlayerPrefs.SetInt("total_score",   TotalScore);
        PlayerPrefs.SetInt("hint_count",    HintCount);
        PlayerPrefs.SetInt("shuffle_count", ShuffleCount);
        PlayerPrefs.Save();
    }

    // ── Level flow ────────────────────────────────────────────────

    public void StartLevel(int index)
    {
        if (index < 0 || index >= levels.Length) return;
        CurrentLevelIndex = index;
        SceneManager.LoadScene(gameSceneName);
    }

    public void OnGameOver()
    {
        SaveProgress();
        StartCoroutine(ReturnToMapAfterDelay());
    }

    public void OnLevelComplete(int levelScore)
    {
        LevelProgress.SetCompleted(CurrentLevelIndex);
        TotalScore += levelScore;
        SaveProgress();
        StartCoroutine(ReturnToMapAfterDelay());
    }

    IEnumerator ReturnToMapAfterDelay()
    {
        yield return new WaitForSeconds(victoryToMapDelay);
        SceneManager.LoadScene(mapSceneName);
    }

    // ── Resource consumption ──────────────────────────────────────

    public bool UseHint()
    {
        if (HintCount <= 0) return false;
        HintCount--;
        SaveProgress();
        GameHUD.Instance?.RefreshResourceCounts();
        return true;
    }

    public bool UseShuffle()
    {
        if (ShuffleCount <= 0) return false;
        ShuffleCount--;
        SaveProgress();
        GameHUD.Instance?.RefreshResourceCounts();
        return true;
    }

    // ── Shop ──────────────────────────────────────────────────────

    public bool BuyHint()
    {
        if (TotalScore < hintCost) return false;
        TotalScore -= hintCost;
        HintCount++;
        SaveProgress();
        return true;
    }

    public bool BuyShuffle()
    {
        if (TotalScore < shuffleCost) return false;
        TotalScore -= shuffleCost;
        ShuffleCount++;
        SaveProgress();
        return true;
    }

#if UNITY_EDITOR
    [ContextMenu("Debug: Reset All Progress")]
    void DebugResetAll()
    {
        LevelProgress.ResetAll();
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        LoadProgress();
        Debug.Log("All progress reset");
    }

#endif
}
