
using System;
using UnityEngine;

[System.Serializable]
public struct Countdowner
{
    [UnityEngine.SerializeField] private float countdown;

    public float Countdown
    {
        get => countdown;
    }

    public void StartCd(float time)
    {
        countdown = time;
    }

    public void Cd()
    {
        if (countdown > 0)
        {
            countdown -= Time.deltaTime;
        }
    }
    public void Cd(float deltaTime)
    {
        if (countdown > 0)
        {
            countdown -= deltaTime;
        }
    }

    public bool IsOut()
    {
        return countdown <= 0;
    }

    public bool IsCd()
    {
        return countdown > 0;
    }

    public void Addtime(float time)
    {
        countdown += time;
    }
    public void EndImmediate()
    {
        countdown = 0;

    }
    
    public static string TimeFormat3(double time)
    {
        if (time <= 0)
            return "00:00";
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);
        if (timeSpan.Days > 0)
            return timeSpan.Hours > 9 ? string.Format("{0:D1}d {1:D2}h", timeSpan.Days, timeSpan.Hours) : string.Format("{0:D1}d {1:D1}h", timeSpan.Days, timeSpan.Hours);
        else
        if (timeSpan.Hours > 0)
            return string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        return string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
    }
}
