using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using AceRental.Domain.Common;

namespace AceRental.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? UserId =>
            _httpContextAccessor.HttpContext?.User?
                .Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        public string? UserName =>
            _httpContextAccessor.HttpContext?.User?
                .Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

        public bool IsAuthenticated =>
            _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
        }
}