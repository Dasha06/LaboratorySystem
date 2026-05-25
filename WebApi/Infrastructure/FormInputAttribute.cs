using Microsoft.AspNetCore.Mvc;

namespace WebApi.Infrastructure;

// показ полей ввода
[AttributeUsage(AttributeTargets.Method)]
public sealed class FormInputAttribute : ConsumesAttribute
{
    public FormInputAttribute()
        : base("application/x-www-form-urlencoded", "multipart/form-data")
    {
    }
}
