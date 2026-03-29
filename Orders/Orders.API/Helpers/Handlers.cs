using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Orders.Domain.Common;
using Orders.Domain.Entities;

namespace Orders.API.Helpers;

public static class Handlers
{
    public static IActionResult HandleResult<T>(Result<T> result)
    {
        if (result.Value is null)
            return new NotFoundResult();

        if (!result.IsSuccess)
            return new BadRequestObjectResult(result.Error);

        return new OkObjectResult(result.Value);
    }

    public static IActionResult HandleCreateAtResult<T>(Result<T> result,
    string actionName,
    object routeValues)
    {
        return new CreatedAtActionResult(actionName, null, routeValues, result.Value);
    }
}
