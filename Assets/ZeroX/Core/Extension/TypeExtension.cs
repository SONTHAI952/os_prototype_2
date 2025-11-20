using System.Reflection;
using System;

namespace ZeroX.Extensions
{
    public static class TypeExtension
    {
        static BindingFlags bindingFlags = BindingFlags.Default | BindingFlags.Instance | BindingFlags.Public |
                                           BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy;
        
        
        
        public static FieldInfo GetFieldDeep(this Type type, string fieldName)
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
        
        /// <summary>
        /// Đối với các private property thì cần phải tìm tới từng base type thì mới thấy
        /// </summary>
        public static PropertyInfo GetPropertyDeep(this Type type, string propertyName)
        {
            do
            {
                var propertyInfo = type.GetProperty(propertyName, bindingFlags);
                if (propertyInfo != null)
                    return propertyInfo;

                type = type.BaseType;
            } while (type != null);

            return null;
        }
    }
}