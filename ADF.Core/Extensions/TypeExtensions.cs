using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Adf.Core.Domain;

namespace Adf.Core.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsValueObject(this Type type)
        {
            return typeof(IValueObject).IsAssignableFrom(type);
        }

        public static bool IsNullable(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof (Nullable<>));
        }
    }
}
