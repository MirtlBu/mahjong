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
    [SerializeField] private int startingScore = 300;

    [Header("Shop Prices")]
    public int hintCost    = 10;
    public int shuffleCost = 25;


    public int CurrentLevelIndex { get; private set; } = -1;

    public LayoutSO[] Levels     => levels;
    public LayoutSO CurrentLevel => (CurrentLevelIndex >= 0 && CurrentLevelIndex < levels.Length)
        ? levels[CurrentLevelIndex] : null;

    public int TotalScore { get; private set; }

    private int _sessionStartScore;

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
        GameHUD.Instance?.SetScore(TotalScore);
        GameHUD.Instance?.RefreshResourceCounts();
    }

    // ── Progress persistence ──────────────────────────────────────

    void LoadProgress()
    {
        TotalScore = PlayerPrefs.GetInt("total_score", startingScore);
    }

    void SaveProgress()
    {
        PlayerPrefs.SetInt("total_score", TotalScore);
        PlayerPrefs.Save();
    }

    // ── Score / currency ──────────────────────────────────────────

    public void AddScore(int amount)
    {
        TotalScore += amount;
        SaveProgress();
        GameHUD.Instance?.SetScore(TotalScore);
    }

    public bool SpendScore(int amount)
    {
        if (TotalScore < amount) return false;
        TotalScore -= amount;
        SaveProgress();
        GameHUD.Instance?.SetScore(TotalScore);
        return true;
    }

    // ── Level flow ────────────────────────────────────────────────

    public void StartLevel(int index)
    {
        if (index < 0 || index >= levels.Length) return;
        CurrentLevelIndex = index;
        _sessionStartScore = TotalScore;
        SceneManager.LoadScene(gameSceneName);
    }

    public void AbandonLevel()
    {
        TotalScore = _sessionStartScore;
        SaveProgress();
        SceneManager.LoadScene(mapSceneName);
    }

    public void OnGameOver()
    {
        SaveProgress();
    }

    // Returns stars earned (1-3)
    public int OnLevelComplete(int maxPossibleScore)
    {
        int scoreEarned = TotalScore - _sessionStartScore;
        int stars = CalculateStars(scoreEarned, maxPossibleScore);
        float ratio = maxPossibleScore > 0 ? (float)scoreEarned / maxPossibleScore : 0f;
        Debug.Log($"[Stars] earned={scoreEarned} max={maxPossibleScore} ratio={ratio:P0} → {stars} stars");
        LevelProgress.SetCompleted(CurrentLevelIndex);
        LevelProgress.SetStars(CurrentLevelIndex, stars);
        SaveProgress();
        return stars;
    }

    public void ReturnToMap()
    {
        SceneManager.LoadScene(mapSceneName);
    }

    static int CalculateStars(int scoreEarned, int maxPossibleScore)
    {
        if (maxPossibleScore <= 0) return 1;
        float ratio = (float)scoreEarned / maxPossibleScore;
        if (ratio >= 0.95f) return 3;
        if (ratio >= 0.75f) return 2;
        return 1;
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
