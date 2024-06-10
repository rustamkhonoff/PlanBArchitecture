using System;

namespace PlanB.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsInheritsGenericClass(this Type toCheck, Type target)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                Type cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (target == cur)
                    return true;
                toCheck = toCheck.BaseType;
            }

            return false;
        }
    }
}