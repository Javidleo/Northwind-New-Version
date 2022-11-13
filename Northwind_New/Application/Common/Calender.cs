namespace Application.Common
{
    public class Calender
    {
        public readonly DateTime CurrentDateWithTime = DateTime.Now;
        public readonly DateTime CurrentDate = DateTime.Now.Date;
        public readonly TimeSpan CurrentTime = DateTime.Now.TimeOfDay;
        public readonly int CurrentYear = DateTime.Now.Year;
    }
}
