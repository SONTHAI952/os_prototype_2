using UnityEngine;
using UnityEngine.UI;

namespace Unbound.Core.Extension
{
    public static class RectTransformExtension
    {
        public static RectTransform RectTransform(this Transform trans)
        {
            return trans.GetComponent<RectTransform>();
        }
        public static RectTransform RectTransform(this GameObject gameObject)
        {
            return gameObject.GetComponent<RectTransform>();
        }
        public static RectTransform SetAnchorPositionX(this RectTransform rect, float value)
        {
            Vector2 temp = rect.anchoredPosition;
            temp.x = value;
            rect.anchoredPosition = temp;

            return rect;
        }
        public static Transform SetPositionZ(this Transform trans, float value)
        {
            Vector3 temp = trans.position;
            temp.z = value;
            trans.position = temp;

            return trans;
        }
        public static Transform SetPositionX(this Transform trans, float value)
        {
            Vector3 temp = trans.position;
            temp.x = value;
            trans.position = temp;

            return trans;
        }
        public static Transform SetPositionY(this Transform trans, float value)
        {
            Vector3 temp = trans.position;
            temp.y = value;
            trans.position = temp;

            return trans;
        }
        public static Transform SetPositionXZ(this Transform trans, float valueX, float valueZ)
        {
            Vector3 temp = trans.position;
            temp.x = valueX;
            temp.z = valueZ;
            trans.position = temp;

            return trans;
        }
        
        public static RectTransform SetAnchorPositionY(this RectTransform trans, float valueY)
        {
            Vector3 temp = trans.anchoredPosition;
            temp.y = valueY;
            trans.anchoredPosition = temp;

            return trans;
        }


        public static SpriteRenderer SetAlpha(this SpriteRenderer sprite, float value)
        {
            Color temp = sprite.color;
            temp.a = value;
            sprite.color = temp;

            return sprite;
        }
        public static Graphic SetAlpha(this Graphic sprite, float value)
        {
            Color temp = sprite.color;
            temp.a = value;
            sprite.color = temp;

            return sprite;
        }

        public static Transform SetLocalPositionZ(this Transform trans, float value)
        {
            Vector3 temp = trans.localPosition;
            temp.z = value;
            trans.localPosition = temp;

            return trans;
        }
        public static Transform AddLocalPositionX(this Transform trans, float value)
        {
            Vector3 temp = trans.localPosition;
            temp.x += value;
            trans.localPosition = temp;

            return trans;
        }

        public static Transform SetLocalPositionX(this Transform trans, float value)
        {
            Vector3 temp = trans.localPosition;
            temp.x = value;
            trans.localPosition = temp;

            return trans;
        } 
        
        public static Transform AddLocalPositionY(this Transform trans, float value)
        {
            Vector3 temp = trans.localPosition;
            temp.y += value;
            trans.localPosition = temp;

            return trans;
        }

        public static Transform SetLocalPositionY(this Transform trans, float value)
        {
            Vector3 temp = trans.localPosition;
            temp.y = value;
            trans.localPosition = temp;

            return trans;
        }
        
        public static Transform AddLocalPositionZ(this Transform trans, float value)
        {
            Vector3 temp = trans.localPosition;
            temp.z += value;
            trans.localPosition = temp;

            return trans;
        }

        public static float GetVerticalNormalizedPositionAt(this ScrollRect scrollRect, RectTransform target)
        {
            RectTransform view = scrollRect.viewport != null ? scrollRect.viewport : scrollRect.GetComponent<RectTransform>();

            Rect viewRect = view.rect;
            Bounds elementBounds = target.TransformBoundsTo(view);
            float offset = viewRect.center.y - elementBounds.center.y;

            float scrollPos = scrollRect.verticalNormalizedPosition - scrollRect.NormalizeScrollDistance(1, offset);
            return Mathf.Clamp(scrollPos, 0f, 1f);
        }
        
        static Vector3[] corners = new Vector3[4];
        public static Bounds TransformBoundsTo(this RectTransform source, Transform target)
        {
            var bounds = new Bounds();
            if (source != null)
            {
                source.GetWorldCorners(corners);

                var vMin = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
                var vMax = new Vector3(float.MinValue, float.MinValue, float.MinValue);

                var matrix = target.worldToLocalMatrix;
                for (int j = 0; j < 4; j++)
                {
                    Vector3 v = matrix.MultiplyPoint3x4(corners[j]);
                    vMin = Vector3.Min(v, vMin);
                    vMax = Vector3.Max(v, vMax);
                }

                bounds = new Bounds(vMin, Vector3.zero);
                bounds.Encapsulate(vMax);
            }
            return bounds;
        }


        public static float NormalizeScrollDistance(this ScrollRect scrollRect, int axis, float distance)
        {
            var viewport = scrollRect.viewport;
            var viewRect = viewport != null ? viewport : scrollRect.GetComponent<RectTransform>();
            var viewBounds = new Bounds(viewRect.rect.center, viewRect.rect.size);

            var content = scrollRect.content;
            var contentBounds = content != null ? content.TransformBoundsTo(viewRect) : new Bounds();

            var hiddenLength = contentBounds.size[axis] - viewBounds.size[axis];
            return distance / hiddenLength;
        }
    }
}
