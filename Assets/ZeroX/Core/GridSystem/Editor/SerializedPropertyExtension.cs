using System;
using System.Collections;
using System.Reflection;
using UnityEditor;

namespace ZeroX.Editors
{
    public static class SerializedPropertyExtension
    {
        static BindingFlags bindingFlags = BindingFlags.Default | BindingFlags.Instance | BindingFlags.Public |
                                           BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy;



        #region Utility

        private static FieldInfo FindFieldInfo(Type type, string fieldName)
        {
            do
            {
                var fieldInfo = type.GetField(fieldName, bindingFlags);
                if (fieldInfo != null)
                    return fieldInfo;

                type = type.BaseType;
            } while (type != null);

            return null;
        }

        #endregion
        
        
        
        public static Type GetFieldType(this SerializedProperty serializeProperty)
        {
            var listFieldName = serializeProperty.propertyPath.Split('.');
            Type currentType = serializeProperty.serializedObject.targetObject.GetType();

            for (int i = 0; i < listFieldName.Length; i++)
            {
                var fieldName = listFieldName[i];
                
                //ElementOfList sẽ có dạng: listRule.Array.data[elementIndex].xxx
                bool isElementOfList = i + 1 < listFieldName.Length &&
                              listFieldName[i] == "Array" &&
                              listFieldName[i + 1] == "data[";

                
                if (isElementOfList == false)
                {
                    var fieldInfo = FindFieldInfo(currentType, fieldName);
                    currentType = fieldInfo.FieldType;
                }
                else
                {
                    currentType = currentType.GenericTypeArguments[0];
                    i++;
                }
            }

            return currentType;
        }
        
        
        public static object GetObject(this SerializedProperty serializeProperty)
        {
            var listFieldName = serializeProperty.propertyPath.Split('.');

            Type currentType = serializeProperty.serializedObject.targetObject.GetType();
            object currentObject = serializeProperty.serializedObject.targetObject;

            for (int i = 0; i < listFieldName.Length; i++)
            {
                string fieldName = listFieldName[i];
                
                //ElementOfList sẽ có dạng: listRule.Array.data[elementIndex].xxx
                bool isElementOfList = i + 1 < listFieldName.Length &&
                                       listFieldName[i] == "Array" && 
                                       listFieldName[i + 1].StartsWith("data[");

                if (isElementOfList) //có nghĩa fieldInfo hiện tại là list
                {
                    var data = listFieldName[i + 1]; //data[xxx];
                    int index = int.Parse(data.Substring(5, data.Length - 6));

                    IList list = (IList)currentObject;

                    currentObject = list[index];

                    if (currentObject == null)
                        return null;

                    currentType = currentObject.GetType();
                    i++;
                }
                else
                {
                    var fieldInfo = FindFieldInfo(currentType, fieldName);
                    var obj = fieldInfo.GetValue(currentObject);

                    currentType = fieldInfo.FieldType;
                    currentObject = obj;
                }
            }

            return currentObject;
        }

       



        #region Call Function

        private static Type[] CreateParameterTypes(object[] parameters)
        {
            if(parameters == null || parameters.Length == 0)
                return new Type[0];
            
            
            Type[] types = new Type[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                types[i] = parameters[i].GetType();
            }

            return types;
        }
        
        public static object InvokeObjectMethod(this SerializedProperty property, string functionName, object[] parameters = null)
        {
            object obj = GetObject(property);
            if (obj == null)
                throw new Exception("Cannot call function because obj is null");
            
            Type objType = obj.GetType();
            var parameterTypes = CreateParameterTypes(parameters);
            var methodInfo = objType.GetMethod(functionName, bindingFlags, null, parameterTypes, null);
            return methodInfo.Invoke(obj, parameters);
        }

        #endregion


        
        #region Get Field
        
        public static T GetFieldValue<T>(this SerializedProperty property, string fieldName)
        {
            object obj = GetObject(property);
            if(obj == null)
                throw new Exception("Cannot get field value because obj is null");
            
            Type objType = obj.GetType();
            var fieldInfo = objType.GetField(fieldName, bindingFlags);
            return (T)fieldInfo.GetValue(obj);
        }

        #endregion
        
        
        
        #region Get Property Value
        
        public static T GetPropertyValue<T>(this SerializedProperty property, string fieldName)
        {
            object obj = GetObject(property);
            if(obj == null)
                throw new Exception("Cannot get field value because obj is null");
            
            Type objType = obj.GetType();
            var fieldInfo = objType.GetProperty(fieldName, bindingFlags);
            return (T)fieldInfo.GetValue(obj);
        }

        #endregion


        #region Misc

        /// <summary>
        /// Nếu trả về chuỗi rỗng thì parent là SerializedObject
        /// </summary>
        public static string GetParentPropertyPath(this SerializedProperty property)
        {
            string path = property.propertyPath;
            int dotIndex = path.LastIndexOf('.');
            
            if (dotIndex == -1)
                return "";

            if (dotIndex + 5 < path.Length && dotIndex - 5 > 0)
            {
                if (path[dotIndex + 1] == 'd' &&
                    path[dotIndex + 2] == 'a' &&
                    path[dotIndex + 3] == 't' &&
                    path[dotIndex + 4] == 'a' &&
                    path[dotIndex + 5] == '[' &&

                    path[dotIndex - 5] == 'A' &&
                    path[dotIndex - 4] == 'r' &&
                    path[dotIndex - 3] == 'r' &&
                    path[dotIndex - 2] == 'a' &&
                    path[dotIndex - 1] == 'y'
                   )
                {
                    return path.Substring(0, dotIndex - 6);
                }
            }
            
            return path.Substring(0, dotIndex);
        }

        /// <summary>
        /// So sánh giá trị của serializedProperty với value
        /// </summary>
        public static bool EqualSpValue(this SerializedProperty property, object value)
        {
            if (property.propertyType == SerializedPropertyType.Enum)
            {
                if (value is Enum)
                {
                    return property.intValue == Convert.ToInt32(value);
                }

                if (value is int)
                {
                    return property.intValue == (int)value;
                }

                return true;
            }
            
            
            return property.boxedValue.Equals(value);
        }

        #endregion
    }
}