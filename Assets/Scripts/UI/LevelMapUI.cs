using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMapUI : MonoBehaviour
{
    [SerializeField] private GameObject mapPanel;
    [SerializeField] private LevelNodeUI nodePrefab;
    [SerializeField] private RectTransform nodeContainer;

    [Header("Pagination")]
    [SerializeField] private int levelsPerPage = 4;
    [SerializeField] private Button prevButton;
    [SerializeField] private Button nextButton;

    [Header("Navigation")]
    [SerializeField] private Button backButton;
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private LayoutSO[] _levels;
    private int _currentPage = 0;
    private int PageCount => (_levels == null || _levels.Length == 0) ? 0
        : Mathf.CeilToInt((float)_levels.Length / levelsPerPage);

    void Start()
    {
        backButton?.onClick.AddListener(() => SceneManager.LoadScene(mainMenuSceneName));
        prevButton?.onClick.AddListener(PrevPage);
        nextButton?.onClick.AddListener(NextPage);
    }

    public void Build(LayoutSO[] levels)
    {
        if (nodePrefab == null || nodeContainer == null || levels == null || levels.Length == 0) return;
        _levels = levels;
        _currentPage = 0;
        RebuildPage();
    }

    public void Show()
    {
        mapPanel.SetActive(true);
        RebuildPage();
    }

    public void Hide()
    {
        mapPanel.SetActive(false);
    }

    void PrevPage()
    {
        if (_currentPage > 0) { _currentPage--; RebuildPage(); }
    }

    void NextPage()
    {
        if (_currentPage < PageCount - 1) { _currentPage++; RebuildPage(); }
    }

    void RebuildPage()
    {
        if (_levels == null) return;

        foreach (Transform child in nodeContainer)
            Destroy(child.gameObject);

        int start = _currentPage * levelsPerPage;
        int end   = Mathf.Min(start + levelsPerPage, _levels.Length);

        for (int i = start; i < end; i++)
        {
            var node = Instantiate(nodePrefab, nodeContainer);
            node.Setup(_levels[i], i);
        }

        if (prevButton != null) prevButton.gameObject.SetActive(_currentPage > 0);
        if (nextButton != null) nextButton.gameObject.SetActive(_currentPage < PageCount - 1);
    }
}
