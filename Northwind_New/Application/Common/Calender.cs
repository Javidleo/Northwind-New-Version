namespace Application.Common
{
    public static class Calender
    {
        public static readonly DateTime CurrentDateWithTime = DateTime.Now;
        public static readonly DateTime CurrentDate = DateTime.Now.Date;
        public static readonly TimeSpan CurrentTime = DateTime.Now.TimeOfDay;
        public static readonly int CurrentYear = DateTime.Now.Year;
    }
}
