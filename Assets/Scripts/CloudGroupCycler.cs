using UnityEngine;

// Вешай на CloudLayer. Внутри CloudLayer сделай два дочерних объекта:
// GroupA и GroupB — в каждом 3-4 облака.
// Группы движутся по ветру, когда одна уходит за камеру — телепортируется за другую.
public class CloudGroupCycler : MonoBehaviour
{
    [Header("Groups")]
    [SerializeField] private Transform groupA;
    [SerializeField] private Transform groupB;

    [Header("Wind")]
    [SerializeField] private Vector3 windDirection = Vector3.right;
    [SerializeField] private float speed = 2f;

    [Header("Cycle")]
    [Tooltip("Расстояние между группами в мировых единицах — подбирай так чтобы группа покрывала весь экран")]
    [SerializeField] private float groupSpacing = 40f;
    [Tooltip("Отступ за правым краем экрана (viewport) перед телепортом")]
    [SerializeField] private float offscreenBuffer = 0.1f;

    private Camera cam;
    private Vector3 windDir;

    void Start()
    {
        cam = Camera.main;
        windDir = windDirection.normalized;
    }

    void Update()
    {
        if (cam == null) cam = Camera.main;
        if (cam == null) return;

        Vector3 move = windDir * speed * Time.deltaTime;
        groupA.position += move;
        groupB.position += move;

        CheckAndWrap(groupA);
        CheckAndWrap(groupB);
    }

    // Телепортируем группу за левый край когда она вышла за правый
    private void CheckAndWrap(Transform group)
    {
        float depth = Vector3.Dot(group.position - cam.transform.position, cam.transform.forward);
        if (depth <= 0) return;

        Vector3 rightWorld = cam.ViewportToWorldPoint(new Vector3(1f + offscreenBuffer, 0.5f, depth));
        Vector3 leftWorld  = cam.ViewportToWorldPoint(new Vector3(    -offscreenBuffer, 0.5f, depth));

        float groupAlong = Vector3.Dot(group.position, windDir);
        float rightAlong = Vector3.Dot(rightWorld,     windDir);
        float leftAlong  = Vector3.Dot(leftWorld,      windDir);

        if (groupAlong > rightAlong)
            group.position += windDir * (leftAlong - groupSpacing - groupAlong);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.5f, 0.8f, 1f, 0.4f);
        if (groupA) Gizmos.DrawWireSphere(groupA.position, 2f);
        if (groupB) Gizmos.DrawWireSphere(groupB.position, 2f);
    }
}
