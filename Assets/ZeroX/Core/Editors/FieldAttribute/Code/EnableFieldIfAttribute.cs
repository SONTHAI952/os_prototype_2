using UnityEngine;

namespace ZeroX
{
    public class EnableFieldIfAttribute : PropertyAttribute
    {
        public string conditionFieldName;
        public object conditionValue;
        
        
        public EnableFieldIfAttribute(string condition, object conditionValue)
        {
            this.conditionFieldName = condition;
            this.conditionValue = conditionValue;
        }
        
        public EnableFieldIfAttribute(string condition)
        {
            this.conditionFieldName = condition;
            this.conditionValue = true;
        }
    }
}