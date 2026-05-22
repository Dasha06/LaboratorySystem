using Microsoft.AspNetCore.Mvc;

namespace WebApi.Infrastructure;

/// <summary>
/// POST/PUT принимают поля формы (Swagger показывает отдельные inputs, не JSON).
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class FormInputAttribute : ConsumesAttribute
{
    public FormInputAttribute()
        : base("application/x-www-form-urlencoded", "multipart/form-data")
    {
    }
}
