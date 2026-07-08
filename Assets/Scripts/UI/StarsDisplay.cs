using UnityEngine;
using UnityEngine.UI;

public class StarsDisplay : MonoBehaviour
{
    [SerializeField] private Image[] starImages;  // ровно 3 слота
    [SerializeField] private Material filledMaterial;
    [SerializeField] private Material emptyMaterial;

    void Awake()
    {
        gameObject.SetActive(false);
    }

    public void SetStars(int count)
    {
        for (int i = 0; i < starImages.Length; i++)
        {
            if (starImages[i] == null) continue;
            bool filled = i < count;
            starImages[i].material = filled ? filledMaterial : emptyMaterial;
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show(int count)
    {
        gameObject.SetActive(true);
        SetStars(count);
    }
}
