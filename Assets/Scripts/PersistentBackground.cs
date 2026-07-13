using UnityEngine;

public class PersistentBackground : MonoBehaviour
{
    void Awake()
    {
        if (FindObjectsByType<PersistentBackground>(FindObjectsSortMode.None).Length > 1)
            Destroy(gameObject);
        else
            DontDestroyOnLoad(gameObject);
    }
}
