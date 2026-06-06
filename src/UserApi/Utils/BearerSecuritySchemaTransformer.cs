using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace UserApi.Utils;

public sealed class BearerSecuritySchemaTransformer(IAuthenticationSchemeProvider authenticationSchemeProvider)
    : IOpenApiDocumentTransformer
{
    private const string SecuritySchemeId = JwtBearerDefaults.AuthenticationScheme;

    public async Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        var authenticationSchemes = await authenticationSchemeProvider.GetAllSchemesAsync();

        if (authenticationSchemes.All(authScheme => authScheme.Name != SecuritySchemeId))
        {
            return;
        }

        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();

        document.Components.SecuritySchemes[SecuritySchemeId] = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            In = ParameterLocation.Header,
            BearerFormat = "JWT",
        };

        var securityRequirement = new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference(SecuritySchemeId, document)] = []
        };

        document.Security ??= [];
        document.Security.Add(securityRequirement);

        if (document.Paths is null)
        {
            return;
        }

        foreach (var path in document.Paths.Values)
        {
            if (path.Operations is null)
            {
                continue;
            }

            foreach (var operation in path.Operations.Values)
            {
                operation.Security ??= [];
                operation.Security.Add(securityRequirement);
            }
        }
    }
}
