// Services/ScopedPermissionsAuthorizationHandler.cs

using System.Security.Claims;
using System.Threading.Tasks;
using CouncilsManagmentSystem.Services;
using Microsoft.AspNetCore.Authorization;

public class ScopedPermissionsAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IPermissionsServies _permissionservies;
    private readonly IUserServies _userServies;

    public ScopedPermissionsAuthorizationHandler(IPermissionsServies permissionservies, IUserServies userServies)
    {
        _permissionservies = permissionservies;
        _userServies = userServies;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        var userEmail = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userEmail == null)
        {
            context.Fail();
        }
        else
        {
            var user = await _userServies.getuserByEmail(userEmail);

            var hasPermission = await _permissionservies.CheckPermissionAsync(user.Id, requirement.PermissionName);

            if (hasPermission)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
        }
    }
}
