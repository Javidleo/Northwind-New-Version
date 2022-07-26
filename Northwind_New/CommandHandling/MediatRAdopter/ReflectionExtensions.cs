using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandHandling.MediatRAdopter
{
    public static class ReflectionExtensions
    {
        public static IEnumerable<Type> GetInterfacesOfType(this Type @class, Type expectedInterface)
        => @class.GetInterfaces().Where(@interface => @interface.Implements(expectedInterface));

        public static bool Implements(this Type @interface, Type expectedInterface)
                => @interface.IsGenericType
                && @interface.GetGenericTypeDefinition() == expectedInterface;
    }
}
