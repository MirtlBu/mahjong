using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    [Header("Чувствительность")]
    public float sensitivity = 0.3f;

    [Header("Ограничения угла")]
    public float maxHorizontal = 30f;
    public float maxVertical   = 20f;

    // Порог в пикселях — если мышь сдвинулась дальше, это drag, а не клик
    [Header("Порог drag (px)")]
    public float dragThreshold = 5f;

    private Vector3    pivot;
    private float      currentX;
    private float      currentY;
    private Quaternion startRotation;
    private Vector3    startPosition;

    private Vector3 mouseDownPos;
    private bool    isDragging;

    // Другие скрипты могут спросить: сейчас drag или нет?
    public bool IsDragging => isDragging;

    void Start()
    {
        pivot         = Vector3.zero;
        startRotation = transform.rotation;
        startPosition = transform.position;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseDownPos = Input.mousePosition;
            isDragging   = false;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - mouseDownPos;

            if (!isDragging && delta.magnitude > dragThreshold)
                isDragging = true;

            if (isDragging)
            {
                currentX += delta.x * sensitivity * Time.deltaTime * 10f;
                currentY -= delta.y * sensitivity * Time.deltaTime * 10f;
                mouseDownPos = Input.mousePosition; // сбрасываем каждый кадр

                currentX = Mathf.Clamp(currentX, -maxHorizontal, maxHorizontal);
                currentY = Mathf.Clamp(currentY, -maxVertical,   maxVertical);
            }
        }

        if (Input.GetMouseButtonUp(0))
            isDragging = false;

        // Плавный возврат в исходное положение когда не тащим
        if (!Input.GetMouseButton(0))
        {
            currentX = Mathf.Lerp(currentX, 0f, Time.deltaTime * 3f);
            currentY = Mathf.Lerp(currentY, 0f, Time.deltaTime * 3f);
        }

        Quaternion additionalRot = Quaternion.Euler(currentY, currentX, 0f);
        transform.position = pivot + additionalRot * (startPosition - pivot);
        transform.rotation = additionalRot * startRotation;
    }
}
