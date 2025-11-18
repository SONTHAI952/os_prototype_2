using UnityEditor;

namespace ZeroX.Editors
{
    public static class FieldAttributeEditorUtility
    {
        public static SerializedProperty FindConditionSp(this SerializedProperty property, string conditionFieldName)
        {
            string parentPropertyPath = property.GetParentPropertyPath();
            
            if (string.IsNullOrEmpty(parentPropertyPath) == false)
            {
                var parentProperty = property.serializedObject.FindProperty(parentPropertyPath);
                return parentProperty.FindPropertyRelative(conditionFieldName);
            }
            else
            {
                return property.serializedObject.FindProperty(conditionFieldName);
            }
        }
    }
}