using UnityEngine;

namespace ZeroX.Utilities
{
    public class Texture2DSolidColor
    {
        private Color color;
        private Texture2D texture2D;
        
        
        public Texture2DSolidColor(Color color)
        {
            this.color = color;
        }
        
        public Texture2D Texture2D
        {
            get
            {
                if (texture2D == null)
                {
                    texture2D = new Texture2D(1, 1, TextureFormat.RGBA32, false);
                    texture2D.filterMode = FilterMode.Point;
                    texture2D.SetPixel(1, 1, color);
                    texture2D.Apply();
                }

                return texture2D;
            }
        }
        
        public static implicit operator Texture2D(Texture2DSolidColor texture2DSolidColor)
        {
            return texture2DSolidColor.Texture2D;
        }
    }
}