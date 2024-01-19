using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace StoZelisCitati.Helpers;

///These methods should be used only after the user is authenticated
public static class AuthorizationHelper
{
    /// <remarks>only use if user is authenticated</remarks>
    public static int Id(this ClaimsPrincipal user)
        => int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);

    public static bool TryGetId(this ClaimsPrincipal user, [NotNullWhen(true)] out int? id)
    {
        if (int.TryParse(user.FindFirstValue(ClaimTypes.NameIdentifier), out int nameId))
        {
            id = nameId;
            return true;
        }
        id = null;
        return false;
    }
}