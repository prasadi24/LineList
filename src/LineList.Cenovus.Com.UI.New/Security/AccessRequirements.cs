using LineList.Cenovus.Com.Common;
using Microsoft.AspNetCore.Authorization;

namespace LineList.Cenovus.Com.UI.Security
{
    /// <summary>
    /// Attribute that determines if a user is in the proper role to access a function
    /// within system.
    /// </summary>
    public class AccessRequirements : IAuthorizationRequirement
    {
        public RoleType[] _role { get; }
        public IHttpContextAccessor _httpContext { get; }

        /// <summary>
        /// Initializes a new instance of the Authorize Policy
        /// </summary>
        /// <param name="roleType">Type of the role required for access</param>
        public AccessRequirements(IHttpContextAccessor httpContext, RoleType[] role)
        {
            _httpContext = httpContext;
            _role = role;
        }
    }
}