using UnityEngine;

public static class LevelProgress
{
    public static bool IsCompleted(int index) =>
        PlayerPrefs.GetInt($"level_{index}_completed", 0) == 1;

    public static void SetCompleted(int index)
    {
        PlayerPrefs.SetInt($"level_{index}_completed", 1);
        PlayerPrefs.Save();
    }

    public static bool IsUnlocked(int index) =>
        index == 0 || IsCompleted(index - 1);

    public static void ResetAll()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}
