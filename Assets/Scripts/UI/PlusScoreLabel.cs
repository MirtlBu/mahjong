using System.Collections;
using UnityEngine;
using TMPro;

// Повесить на объект plus_score (TMP_Text).
// Вызывать: PlusScoreLabel.Instance.Show(amount);
public class PlusScoreLabel : MonoBehaviour
{
    public static PlusScoreLabel Instance { get; private set; }

    [SerializeField] private TMP_Text label;

    [Header("Animation")]
    [SerializeField] private float scrambleDuration = 0.7f;  // фаза случайных цифр
    [SerializeField] private float countDuration    = 0.6f;  // фаза плавного отсчёта

    private RectTransform _rect;
    private Vector2 _startPos;
    private Coroutine _current;

    void Awake()
    {
        Instance = this;
        _rect = label.GetComponent<RectTransform>();
        _startPos = _rect.anchoredPosition;
        label.gameObject.SetActive(false);
    }

    public void Show(int amount)
    {
        if (amount == 0) return;
        gameObject.SetActive(true);
        if (_current != null) StopCoroutine(_current);
        _current = StartCoroutine(Animate(amount));
    }

    IEnumerator Animate(int amount)
    {
        Color baseColor = amount > 0 ? new Color(0.2f, 0.9f, 0.2f) : new Color(0.9f, 0.3f, 0.2f);
        label.color = baseColor;
        label.gameObject.SetActive(true);
        _rect.anchoredPosition = _startPos;

        // Фаза 1: мелькание случайных цифр, постепенно замедляется
        float elapsed = 0f;
        float nextFlip = 0f;
        float flipInterval = 0.04f;
        while (elapsed < scrambleDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / scrambleDuration;
            flipInterval = Mathf.Lerp(0.04f, 0.18f, t * t); // замедляем к концу
            if (elapsed >= nextFlip)
            {
                int rand = Random.Range(Mathf.Max(1, amount / 4), amount + 1);
                label.text = $"+{rand}";
                nextFlip = elapsed + flipInterval;
            }
            yield return null;
        }

        // Фаза 2: плавный отсчёт до финального значения (ease-out cubic)
        elapsed = 0f;
        while (elapsed < countDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / countDuration;
            float eased = 1f - Mathf.Pow(1f - t, 3f);
            int current = Mathf.RoundToInt(Mathf.Lerp(0, amount, eased));
            label.text = $"+{current}";
            yield return null;
        }
        label.text = $"+{amount}";
        _current = null;
    }
}
