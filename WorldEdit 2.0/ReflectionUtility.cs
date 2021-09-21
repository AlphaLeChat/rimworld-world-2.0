using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WorldEdit_2_0
{
    public static class ReflectionUtility
    {

        public static T GetFieldValue<T>(this object instance, string fieldName)
        {
            FieldInfo fieldInfo = instance.GetType().GetField(fieldName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic);
            if (fieldInfo == null)
                return default;

            return (T)fieldInfo.GetValue(instance);
        }
    }
}
