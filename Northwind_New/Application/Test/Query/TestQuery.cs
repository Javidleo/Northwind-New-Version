using QueryHandling.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Test.Query
{
    public record TestQuery(int Id) : Query<TestViewModel>;
}
