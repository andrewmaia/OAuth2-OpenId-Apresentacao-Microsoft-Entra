using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

public class CheckClaimMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _claimType;
    private readonly string _claimValue;

    public CheckClaimMiddleware(RequestDelegate next, string claimType, string claimValue)
    {
        _next = next;
        _claimType = claimType;
        _claimValue = claimValue;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var user = context.User;
        if (user.HasClaim(c => c.Type == _claimType && c.Value == _claimValue))
            Console.WriteLine($"Possui o claim {_claimType}:{_claimValue} ");
        else
            Console.WriteLine($"Não possui o claim {_claimType}:{_claimValue} ");


        await _next(context);
    }
}