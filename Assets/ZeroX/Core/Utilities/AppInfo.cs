using UnityEngine;

public static class AppInfo
{
    private static string iosAppId = "6444198641";
    
    
    public static string Identifier
    {
        get
        {
#if UNITY_ANDROID
            return Application.identifier;
#elif UNITY_IOS
            return iosAppId;
#else
            return Application.identifier;
#endif
        }
    }
}