namespace Application.Common
{
    public interface ICurrentUserService
    {
        string UserId { get; }
        public bool IsAuthenticated { get; }
    }
}
