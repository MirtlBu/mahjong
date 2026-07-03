using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMapUI : MonoBehaviour
{
    [SerializeField] private GameObject mapPanel;
    [SerializeField] private LevelNodeUI nodePrefab;
    [SerializeField] private RectTransform nodeContainer;
    [SerializeField] private GameObject dashPrefab;   // small dot/dash Image prefab
    [SerializeField] private int dashesPerSegment = 8;

    [Header("Navigation")]
    [SerializeField] private Button backButton;
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private LevelNodeUI[] spawnedNodes;

    void Start()
    {
        backButton?.onClick.AddListener(() => SceneManager.LoadScene(mainMenuSceneName));
    }

    public void Build(LayoutSO[] levels)
    {
        Debug.Log($"LevelMapUI.Build: levels={levels?.Length}, nodePrefab={nodePrefab}, nodeContainer={nodeContainer}");
        if (nodePrefab == null || nodeContainer == null || levels == null || levels.Length == 0) return;

        foreach (Transform child in nodeContainer)
            Destroy(child.gameObject);

        spawnedNodes = new LevelNodeUI[levels.Length];

        for (int i = 0; i < levels.Length; i++)
        {
            var node = Instantiate(nodePrefab, nodeContainer);
            node.GetComponent<RectTransform>().anchoredPosition = levels[i].mapPosition;
            node.Setup(levels[i], i);
            spawnedNodes[i] = node;

            if (i > 0 && dashPrefab != null)
                DrawDashes(levels[i - 1].mapPosition, levels[i].mapPosition);
        }
    }

    public void Show()
    {
        mapPanel.SetActive(true);

        foreach (var node in spawnedNodes)
            node?.Refresh();
    }

    public void Hide()
    {
        mapPanel.SetActive(false);
    }

    void DrawDashes(Vector2 from, Vector2 to)
    {
        for (int d = 1; d < dashesPerSegment; d++)
        {
            float t = (float)d / dashesPerSegment;
            Vector2 pos = Vector2.Lerp(from, to, t);
            var dash = Instantiate(dashPrefab, nodeContainer);
            dash.GetComponent<RectTransform>().anchoredPosition = pos;
            dash.transform.SetSiblingIndex(0); // behind nodes
        }
    }
}
