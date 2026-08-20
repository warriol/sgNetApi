using Microsoft.AspNetCore.Authorization;

namespace sgNetApi.Api.Authorization;

// 1. Atributo personalizado para aplicar en Controllers o Endpoints
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
public class RequirePermissionAttribute : AuthorizeAttribute
{
    public RequirePermissionAttribute(string permiso) : base(policy: permiso)
    {
    }
}

// 2. Requirement que encapsula el nombre del permiso exigido
public class PermissionRequirement : IAuthorizationRequirement
{
    public string Permiso { get; }

    public PermissionRequirement(string permiso)
    {
        Permiso = permiso;
    }
}

// 3. Handler que evalúa si el Token JWT del usuario contiene el claim del permiso
public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        // Obtener todos los claims de tipo "permiso" agregados durante la generación del JWT
        var tienePermiso = context.User.Claims
            .Any(c => c.Type == "permiso" && c.Value.Equals(requirement.Permiso, StringComparison.OrdinalIgnoreCase));

        if (tienePermiso)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}

// 4. PolicyProvider dinámico para crear políticas en tiempo de ejecución sin registrarlas manualmente una a una
public class PermissionPolicyProvider : DefaultAuthorizationPolicyProvider
{
    public PermissionPolicyProvider(Microsoft.Extensions.Options.IOptions<AuthorizationOptions> options) 
        : base(options)
    {
    }

    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        // Buscar primero si existe una política estándar
        var policy = await base.GetPolicyAsync(policyName);
        if (policy != null)
            return policy;

        // Si no existe, asume que es un permiso dinámico y crea la política exigiendo el PermissionRequirement
        return new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .AddRequirements(new PermissionRequirement(policyName))
            .Build();
    }
}