using UnityEngine;

namespace ZeroX.Utilities
{
    public static class ZRectTransformUtility
    {
        private static Camera GetRenderCameraOfCanvas(Canvas canvas)
        {
            if (canvas.rootCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
                return null;

            return canvas.rootCanvas.worldCamera;
        }

        

        #region Screen To

        public static Vector3 ScreenToLocalPointInRectangle(Vector2 fromScreenPos, RectTransform toInRect, Camera toCamera)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(toInRect, fromScreenPos, toCamera, out Vector2 localPoint);
            return localPoint;
        }
        
        public static Vector3 ScreenToLocalPointInRectangle(Vector2 fromScreenPos, RectTransform toInRect)
        {
            Canvas toCanvas = toInRect.GetComponentInParent<Canvas>();
            Camera toCamera = GetRenderCameraOfCanvas(toCanvas);
            
            RectTransformUtility.ScreenPointToLocalPointInRectangle(toInRect, fromScreenPos, toCamera, out Vector2 localPoint);
            return localPoint;
        }

        #endregion
        
        

        #region World To Local
        
        public static Vector3 WorldToLocalPointInRectangle(Vector3 fromWorldPos, Camera fromCamera, RectTransform toInRect, Camera toCamera)
        {
            var screenPos = fromCamera.WorldToScreenPoint(fromWorldPos);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(toInRect, screenPos, toCamera, out Vector2 localPoint);
            return localPoint;
        }

        public static Vector3 WorldToLocalPointInRectangle(Vector3 fromWorldPos, Camera fromCamera, RectTransform toInRect)
        {
            Canvas toCanvas = toInRect.GetComponentInParent<Canvas>();
            Camera toCamera = GetRenderCameraOfCanvas(toCanvas);
            
            var screenPos = fromCamera.WorldToScreenPoint(fromWorldPos);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(toInRect, screenPos, toCamera, out Vector2 localPoint);
            return localPoint;
        }
        
        public static Vector3 PointToLocalPointInRectangle(Transform fromPoint, RectTransform toInRect)
        {
            Canvas fromCanvas = fromPoint.GetComponentInParent<Canvas>();
            Camera fromCamera = GetRenderCameraOfCanvas(fromCanvas);
            
            Canvas toCanvas = toInRect.GetComponentInParent<Canvas>();
            Camera toCamera = GetRenderCameraOfCanvas(toCanvas);
            
            var screenPos = fromCamera.WorldToScreenPoint(fromPoint.position);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(toInRect, screenPos, toCamera, out Vector2 localPoint);
            return localPoint;
        }
        
        #endregion

        

        #region World To World

        public static Vector3 WorldToWorldPointInRectangle(Vector3 fromWorldPos, Camera fromCamera, RectTransform toInRect, Camera toCamera)
        {
            var screenPos = fromCamera.WorldToScreenPoint(fromWorldPos);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(toInRect, screenPos, toCamera, out Vector2 localPoint);
            return toInRect.TransformPoint(localPoint);
        }
        
        public static Vector3 WorldToWorldPointInRectangle(Vector3 fromWorldPos, Camera fromCamera, RectTransform toInRect)
        {
            Canvas toCanvas = toInRect.GetComponentInParent<Canvas>();
            Camera toCamera = GetRenderCameraOfCanvas(toCanvas);
            
            var screenPos = fromCamera.WorldToScreenPoint(fromWorldPos);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(toInRect, screenPos, toCamera, out Vector2 localPoint);
            return toInRect.TransformPoint(localPoint);
        }
        
        public static Vector3 PointToWorldPointInRectangle(Transform fromPoint, RectTransform toInRect)
        {
            Canvas fromCanvas = fromPoint.GetComponentInParent<Canvas>();
            Camera fromCamera = GetRenderCameraOfCanvas(fromCanvas);
            
            Canvas toCanvas = toInRect.GetComponentInParent<Canvas>();
            Camera toCamera = GetRenderCameraOfCanvas(toCanvas);
            
            var screenPos = fromCamera.WorldToScreenPoint(fromPoint.position);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(toInRect, screenPos, toCamera, out Vector2 localPoint);
            return toInRect.TransformPoint(localPoint);
        }

        #endregion
    }
}