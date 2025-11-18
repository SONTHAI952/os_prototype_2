using UnityEngine;

public static class ManagerData
{
    public static int CURRENT_LEVEL_ID
    {
        get { return PlayerPrefs.GetInt("CURRENT_LEVEL_ID", 1); }
        set { PlayerPrefs.SetInt("CURRENT_LEVEL_ID", value); }
    }
    
    public static int CURRENT_LEVEL_MULTIPLIER
    {
        get { return PlayerPrefs.GetInt("CURRENT_LEVEL_MULTIPLIER", 0); }
        set { PlayerPrefs.SetInt("CURRENT_LEVEL_MULTIPLIER", value); }
    }
    
    /*public static bool SETTINGS_MUSIC_ON
    {
        get { return PlayerPrefs.GetInt("SETTINGS_MUSIC_ON", 1) == 1; }
        set { PlayerPrefs.SetInt("SETTINGS_MUSIC_ON", value ? 1 : 0); }
    }*/
    
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
}
