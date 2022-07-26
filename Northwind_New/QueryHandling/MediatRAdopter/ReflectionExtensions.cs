using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryHandling.MediatRAdopter
{
    public static class ReflectionExtensions
    {
        public static bool Implements(this Type @interface, Type expectedInterface)
            => @interface.IsGenericType && @interface.GetGenericTypeDefinition() == expectedInterface;

        public static IEnumerable<Type> GetInterfaceOfType(this Type @class, Type expectedInterface)
            => @class.GetInterfaces().Where(@interface => @interface.Implements(expectedInterface));
    }
}
