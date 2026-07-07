using UnityEngine;
using UnityEngine.UI;

public class StarsDisplay : MonoBehaviour
{
    [SerializeField] private Image[] starImages;  // ровно 3 слота
    [SerializeField] private Sprite filledStar;
    [SerializeField] private Sprite emptyStar;

    public void SetStars(int count)
    {
        for (int i = 0; i < starImages.Length; i++)
        {
            if (starImages[i] == null) continue;
            starImages[i].sprite = i < count ? filledStar : emptyStar;
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
