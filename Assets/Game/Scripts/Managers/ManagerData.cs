using UnityEngine;

public static class ManagerData
{
    private const int MAX_LEVEL = 20;
    public static int CURRENT_LEVEL_ID
    {
        get { return PlayerPrefs.GetInt("CURRENT_LEVEL_ID", 1); }
        set { PlayerPrefs.SetInt("CURRENT_LEVEL_ID", value); }
    }
    
    public static int MAX_LEVEL_UNLOCKED
    {
        get { return PlayerPrefs.GetInt("MAX_LEVEL_UNLOCKED", 1); }
        set { PlayerPrefs.SetInt("MAX_LEVEL_UNLOCKED", value); }
    }
    
    public static int CURRENT_LEVEL_MULTIPLIER
    {
        get { return PlayerPrefs.GetInt("CURRENT_LEVEL_MULTIPLIER", 0); }
        set { PlayerPrefs.SetInt("CURRENT_LEVEL_MULTIPLIER", value); }
    }
    
    public static bool SETTINGS_MUSIC_ON
    {
        get { return PlayerPrefs.GetInt("SETTINGS_MUSIC_ON", 1) == 1; }
        set { PlayerPrefs.SetInt("SETTINGS_MUSIC_ON", value ? 1 : 0); }
    }
    
    public static bool SETTINGS_SOUND_ON
    {
        get { return PlayerPrefs.GetInt("SETTINGS_SOUND_ON", 1) == 1; }
        set { PlayerPrefs.SetInt("SETTINGS_SOUND_ON", value ? 1 : 0); }
    }
    
    public static bool SETTINGS_VIBRATION_ON
    {
        get { return PlayerPrefs.GetInt("SETTINGS_VIBRATION_ON", 1) == 1; }
        set { PlayerPrefs.SetInt("SETTINGS_VIBRATION_ON", value ? 1 : 0); }
    } 
    
    public static bool TUTORIAL_COMPLETED
    {
        get { return PlayerPrefs.GetInt("TUTORIAL_COMPLETED", 0) == 1; }
        set { PlayerPrefs.SetInt("TUTORIAL_COMPLETED", value ? 1 : 0); }
    }

    public static void UnlockCurrentLevel()
    {
        if (MAX_LEVEL_UNLOCKED < MAX_LEVEL)
        {
            MAX_LEVEL_UNLOCKED++;
            CURRENT_LEVEL_ID = MAX_LEVEL_UNLOCKED;
        }
    }

    public static void SetCompleteTutorial()
    {
        TUTORIAL_COMPLETED = true;
    }
}
