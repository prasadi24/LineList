using LineList.Cenovus.Com.Common;
using LineList.Cenovus.Com.Security;
using LineList.Cenovus.Com.UI.Security;
using Microsoft.AspNetCore.Authorization;

public class AccessRequirementHandler : AuthorizationHandler<AccessRequirements>
{
    private readonly CurrentUser _currentUser;

    public AccessRequirementHandler(CurrentUser currentUser)
    {
        _currentUser = currentUser;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AccessRequirements requirement)
    {
        var roleTypeList = requirement._role;
        bool isMember = false;

        foreach (var role in roleTypeList)
        {
            if (role == RoleType.Administrator && _currentUser.IsCenovusAdmin)
            {
                isMember = true;
                break;
            }
            else if (role == RoleType.Editor && (_currentUser.IsEpAdmin || _currentUser.IsEpUser))
            {
                isMember = true;
                break;
            }
            // Add more logic as needed
        }

        if (isMember)
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }

        return Task.CompletedTask;
    }
}
