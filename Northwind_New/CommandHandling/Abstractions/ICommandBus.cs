using System.Threading.Tasks;

namespace CommandHandling.Abstractions
{
    public interface ICommandBus
    {
        public Task Send<Tcommand>(Tcommand command) where Tcommand : Acommand;
    }
}
