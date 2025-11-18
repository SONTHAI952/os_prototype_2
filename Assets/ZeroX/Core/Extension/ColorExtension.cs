using UnityEngine;

namespace ZeroX.Extensions
{
    public static class ColorExtension
    {
        public static bool IsApproximately(this Color color, Color otherColor, float epsilon)
        {
            if (Mathf.Abs(color.r - otherColor.r) > epsilon)
                return false;
            
            if (Mathf.Abs(color.g - otherColor.g) > epsilon)
                return false;
            
            if (Mathf.Abs(color.b - otherColor.b) > epsilon)
                return false;
            
            if (Mathf.Abs(color.a - otherColor.a) > epsilon)
                return false;

            return true;
        }
        
        public static bool IsApproximately(this Color color, Color otherColor)
        {
            if (Mathf.Abs(color.r - otherColor.r) > 0.0001f)
                return false;
            
            if (Mathf.Abs(color.g - otherColor.g) > 0.0001f)
                return false;
            
            if (Mathf.Abs(color.b - otherColor.b) > 0.0001f)
                return false;
            
            if (Mathf.Abs(color.a - otherColor.a) > 0.0001f)
                return false;

            return true;
        }
    }
}