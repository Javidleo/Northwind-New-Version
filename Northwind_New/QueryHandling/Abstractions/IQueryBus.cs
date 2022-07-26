using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryHandling.Abstractions
{
    public interface IQueryBus
    {
        public Task<TviewModel> Send<TviewModel, TQuery>(TQuery Query) where TviewModel : IAmAViewModel where TQuery : Query<TviewModel>;
    }
}
