using UnityEngine;

// Вешай на корневой объект Background.
// Фон всегда позиционируется перед камерой на фиксированном расстоянии.
public class BackgroundFollowCamera : MonoBehaviour
{
    [SerializeField] private float distance = 50f;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        UpdatePosition();
    }

    void LateUpdate()
    {
        if (cam == null) cam = Camera.main;
        if (cam == null) return;
        UpdatePosition();
    }

    void UpdatePosition()
    {
        transform.position = cam.transform.position + cam.transform.forward * distance;
        transform.rotation = cam.transform.rotation;
    }
}
