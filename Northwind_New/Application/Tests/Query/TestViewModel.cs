using QueryHandling.Abstractions;

namespace Application.Tests.Query
{
    public class TestViewModel : IAmAViewModel
    {
        public string name { get; set; }
        public string family { get; set; }
    }
}