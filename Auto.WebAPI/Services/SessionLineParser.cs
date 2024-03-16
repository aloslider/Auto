using System.Text.RegularExpressions;
using Auto.WebAPI.Features.PrintTasks.Models;
using Auto.WebAPI.Services.Interfaces;
using FluentResults;

namespace Auto.WebAPI.Services;

sealed class SessionLineParser : ISessionLineParser
{
    readonly Regex _regex = new("^(?<Name>[^;]+);\\s*(?<EmployeeName>[^;]+);\\s*(?<Order>\\d+)\\s*;\\s*(?<Count>\\d+)\\s*$");

    public Result<Session> Parse(string line)
    {
        var match = _regex.Match(line);

        if (!match.Success)
        {
            return Result.Fail("Line parse error");
        }

        if (!match.Groups.TryGetValue("Name", out var nameGroup))
        {
            return Result.Fail("Line parse error");
        }

        if (!match.Groups.TryGetValue("EmployeeName", out var employeeGroup))
        {
            return Result.Fail("Line parse error");
        }

        if (!match.Groups.TryGetValue("Order", out var orderGroup))
        {
            return Result.Fail("Line parse error");
        }

        if (!match.Groups.TryGetValue("Count", out var countGroup))
        {
            return Result.Fail("Line parse error");
        }

        return
            Result.Ok(
                new Session()
                {
                    Name = nameGroup.Value.Trim(),
                    EmployeeId = int.Parse(employeeGroup.Value.Trim()),
                    DeviceOrderNumber = int.Parse(orderGroup.Value.Trim()),
                    PageCount = int.Parse(countGroup.Value.Trim())
                });
    }
}
