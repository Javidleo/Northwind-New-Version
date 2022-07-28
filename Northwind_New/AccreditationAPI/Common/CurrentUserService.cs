using Application.Common;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace AccreditationAPI.Common
{
    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService(IHttpContextAccessor _httpContextAccessor)
        {
            UserId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            IsAuthenticated = UserId != null;
        }

        public string UserId { get; }

        public bool IsAuthenticated { get; }
    }
}
