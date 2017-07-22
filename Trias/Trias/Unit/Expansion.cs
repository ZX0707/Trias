using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trias.Unit
{
    public static class Expansion
    {
        public static void CopyFrom(this object currentObj, object obj)
        {
            var type = obj.GetType();
            var properties = type.GetProperties();

            foreach (var property in properties)
            {
                currentObj.GetType().GetProperty(property.Name).SetValue(currentObj, property.GetValue(obj, null), null);
            }
        }
    }
}