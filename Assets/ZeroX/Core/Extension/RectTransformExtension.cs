using System;
using UnityEngine;

namespace ZeroX.Extensions
{
    public static class RectTransformExtension
    {
        /// <summary>
        /// Nếu RenderMode là ScreenSpaceCamera thì cần truyền thêm camera
        /// </summary>
        public static Vector2 ScreenPointToLocalPoint(this RectTransform rectTransform, Vector2 screenPoint, RenderMode canvasRenderMode, Camera canvasCamera = null)
        {
            Vector2 localPoint;
            if (canvasRenderMode == RenderMode.ScreenSpaceOverlay)
                RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, null,
                    out localPoint);
            else if (canvasRenderMode == RenderMode.ScreenSpaceCamera)
                RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, canvasCamera,
                    out localPoint);
            else
                throw new Exception("Cannot support RenderMode: " + canvasRenderMode);

            return localPoint;
        }
        
        public static Vector2 ViewportPointToLocalPoint(this RectTransform rectTransform, Vector2 viewportPoint, RenderMode canvasRenderMode, Camera canvasCamera = null)
        {
            //Đổi sang screenPoint
            viewportPoint.x *= Screen.width; 
            viewportPoint.y *= Screen.height;
            
            Vector2 localPoint;
            if (canvasRenderMode == RenderMode.ScreenSpaceOverlay)
                RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, viewportPoint, null,
                    out localPoint);
            else if (canvasRenderMode == RenderMode.ScreenSpaceCamera)
                RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, viewportPoint, canvasCamera,
                    out localPoint);
            else
                throw new Exception("Cannot support RenderMode: " + canvasRenderMode);

            return localPoint;
        }

        public static RenderMode GetCanvasRenderMode(this RectTransform rectTransform)
        {
            Canvas canvas = rectTransform.GetComponentInParent<Canvas>();
            if (canvas.renderMode == RenderMode.ScreenSpaceCamera || canvas.worldCamera != null)
            {
                return RenderMode.ScreenSpaceCamera;
            }
            else
            {
                return RenderMode.ScreenSpaceOverlay;
            }
        }

        public static void GetCanvasRenderModeAndCamera(this RectTransform rectTransform, ref RenderMode canvasRenderMode, ref Camera canvasCamera)
        {
            Canvas canvas = rectTransform.GetComponentInParent<Canvas>();
            if (canvas.renderMode == RenderMode.ScreenSpaceCamera || canvas.worldCamera != null)
            {
                canvasRenderMode = RenderMode.ScreenSpaceCamera;
                canvasCamera = canvas.worldCamera;
            }
            else
            {
                canvasRenderMode = RenderMode.ScreenSpaceOverlay;
                canvasCamera = null;
            }
        }
    }
}