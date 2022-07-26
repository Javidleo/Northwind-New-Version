using System.Threading.Tasks;

namespace CommandHandling.Abstractions
{
    public interface IHandleCommand<Tcommand> where Tcommand : Acommand
    {
        public Task Handle(Tcommand command);
    }
}
